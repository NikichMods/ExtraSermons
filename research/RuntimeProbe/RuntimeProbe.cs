// SPDX-License-Identifier: MPL-2.0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using BepInEx;
using FlowCanvas;
using FlowCanvas.Nodes;
using HarmonyLib;
using NodeCanvas.Framework;
using UnityEngine;

namespace ExtraSermons.Research
{
    [BepInPlugin(Identity.Guid, "Extra Sermons Runtime Probe", Identity.Version)]
    public sealed class RuntimeProbe : BaseUnityPlugin
    {
        private const string Weekly = "prayed_this_week";
        private static RuntimeProbe self;
        private static readonly ProbeState state = new ProbeState();
        private static readonly FieldInfo Root = AccessTools.Field(typeof(WorldGameObject), "_data");
        private static readonly FieldInfo SavedRoot = AccessTools.Field(typeof(GameSave), "_inventory");
        private static readonly FieldInfo Serialized = AccessTools.Field(typeof(Graph), "_serializedGraph");
        private static readonly FieldInfo GatePort = AccessTools.Field(Closure("Flow_PlayerFlag"), "flow_eq0");
        private static readonly FieldInfo WritePort = AccessTools.Field(Closure("Flow_SetPlayerParam"), "flow_out");
        private static readonly FieldInfo QuestPort = AccessTools.Field(Closure("Flow_CheckKeyQuest"), "flow_out");
        private static readonly FieldInfo QuestKey = AccessTools.Field(Closure("Flow_CheckKeyQuest"), "in_quest_key");
        private static readonly FieldInfo DonationNode = AccessTools.Field(Closure("Flow_DonateToBox"), "<>4__this");
        private static readonly FieldInfo DonationMoney = AccessTools.Field(Closure("Flow_DonateToBox"), "par_money");
        private static GameSave boundSave;
        private static WorldGameObject boundPlayer, pulpit;
        private static Item boundRoot;
        private static PrayCraftGUI ui;
        private static CustomFlowScript service;
        private static bool compatible, enabledProbe, inConfirm, panel = true, observing;
        private static bool startWrite, graphWrite, launchHook;
        private static string message = "F8: panel. Load test slot 1, then enable.";
        private Harmony harmony;

        private static Type Closure(string name)
        {
            return typeof(MainGame).Assembly.GetType("FlowCanvas.Nodes." + name + "+<>c__DisplayClass0_0", true);
        }
        private static MethodInfo Handler(string name)
        {
            return AccessTools.Method(Closure(name), "<RegisterPorts>b__0");
        }
        private void Awake()
        {
            self = this;
            try
            {
                if (Hash(File.ReadAllBytes(typeof(MainGame).Assembly.Location)) != Identity.HostHash ||
                    Hash(File.ReadAllBytes(typeof(Graph).Assembly.Location)) != Identity.FirstpassHash)
                    throw new InvalidOperationException("Unverified host assembly hashes");
                if (new[] { Root, SavedRoot, Serialized, GatePort, WritePort, QuestPort, QuestKey, DonationNode, DonationMoney }.Any(f => f == null))
                    throw new MissingMemberException("Required native fields");
                harmony = new Harmony(Identity.Guid);
                Patch(Handler("Flow_PlayerFlag"), transpiler: nameof(GateTranspiler));
                Patch(Handler("Flow_SetPlayerParam"), transpiler: nameof(GraphWriteTranspiler));
                Patch(AccessTools.Method(typeof(PrayCraftGUI), "OnPrayButtonPressed"),
                    prefix: nameof(ConfirmPrefix), finalizer: nameof(ConfirmFinalizer), transpiler: nameof(ConfirmTranspiler));
                Patch(AccessTools.Method(typeof(PrayCraftGUI), "Open"), postfix: nameof(Opened));
                Patch(AccessTools.Method(typeof(PrayCraftGUI), "OnResourcePickerClosed"), postfix: nameof(Selected));
                Patch(AccessTools.Method(typeof(ResourceBasedCraftGUI), "Hide"), prefix: nameof(Hidden));
                Patch(AccessTools.Method(typeof(CustomFlowScript), "TerminateMe"), prefix: nameof(Terminating));
                Patch(AccessTools.Method(typeof(MainGame), "StartGameLoading"), prefix: nameof(Loading));
                Patch(AccessTools.Method(typeof(PrayLogics), "CalculatePray"), postfix: nameof(Calculated));
                Patch(AccessTools.Method(typeof(PrayCraftGUI), "OnFinishedPrayBuffAnimation"), postfix: nameof(AnimationDone));
                Patch(AccessTools.Method(typeof(WorldMap), "TryRemoveStackedChurchVisitors"),
                    prefix: nameof(CleanupBefore), postfix: nameof(CleanupAfter));
                Patch(AccessTools.Method(typeof(WorldMap), "GetWorldGameObjectByCustomTag"), postfix: nameof(BishopLookup));
                Patch(Handler("Flow_CheckKeyQuest"), prefix: nameof(Finale));
                Patch(Handler("Flow_DonateToBox"), postfix: nameof(Donated));
                compatible = startWrite && graphWrite && launchHook;
                if (!compatible) throw new InvalidOperationException("Incomplete callsite installation");
                Log("READY version=" + Identity.Version + " source=" + BuildIdentity.Source +
                    " dll=" + Hash(File.ReadAllBytes(typeof(RuntimeProbe).Assembly.Location)) +
                    " host=1.407 enabled=false test_slot=1");
            }
            catch (Exception e)
            {
                compatible = false; enabledProbe = false;
                if (harmony != null) harmony.UnpatchSelf();
                message = "Probe disabled: " + e.Message;
                Logger.LogError("ES_PROBE INIT_FAILED fallback=vanilla " + e);
            }
        }
        private void Patch(MethodInfo target, string prefix = null, string postfix = null,
            string transpiler = null, string finalizer = null)
        {
            if (target == null) throw new MissingMethodException("Native target missing");
            harmony.Patch(target, HM(prefix), HM(postfix), HM(transpiler), HM(finalizer), null);
        }
        private static HarmonyMethod HM(string name)
        {
            return name == null ? null : new HarmonyMethod(typeof(RuntimeProbe), name);
        }
        private static string Hash(byte[] bytes)
        {
            using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
        }
        private static void Log(string s) { self.Logger.LogInfo("ES_PROBE t=" + Time.realtimeSinceStartup.ToString("F3", System.Globalization.CultureInfo.InvariantCulture) + " " + s); }
        private static void Tell(string s) { message = s; Log(s); }

        // Strict local test envelope. Never infer slot identity from day or live player alone.
        private static bool CurrentTestSave()
        {
            var game = MainGame.me;
            return game != null && MainGame.game_started && !MainGame.game_starting && game.save != null &&
                game.save_slot != null && game.save_slot.filename_no_extension == "1" && game.player != null &&
                SavedRoot.GetValue(game.save) != null &&
                ReferenceEquals(SavedRoot.GetValue(game.save), Root.GetValue(game.player));
        }
        private static bool Bound()
        {
            return compatible && enabledProbe && CurrentTestSave() && ReferenceEquals(MainGame.me.save, boundSave) &&
                MainGame.me.player == boundPlayer && ReferenceEquals(Root.GetValue(boundPlayer), boundRoot);
        }
        private static bool Unlocked()
        {
            return Bound() && boundPlayer.GetParamInt("preaching_is_locked") == 0 &&
                boundPlayer.GetParamInt("cup_20_reached") >= 1 && boundPlayer.GetParamInt("first_sermon") == 0;
        }
        private static bool GraphMatches(string id, string hash)
        {
            var graph = CustomFlowScript.GetGraph(id);
            return graph != null && Hash(Encoding.UTF8.GetBytes((string)Serialized.GetValue(graph))) == hash;
        }
        private static void Enable()
        {
            if (!compatible || !CurrentTestSave()) { Tell("REFUSED need restored test slot 1"); return; }
            if (!GraphMatches("on_start_preaching", Identity.EntryHash) || !GraphMatches("pray", Identity.PrayHash))
            { Tell("REFUSED native graph identity mismatch; vanilla retained"); return; }
            boundSave = MainGame.me.save; boundPlayer = MainGame.me.player;
            boundRoot = (Item)Root.GetValue(boundPlayer);
            state.Clear(); service = null; ui = null; pulpit = null; enabledProbe = true;
            Tell("ENABLED slot=1 day=" + boundSave.day + " permit=0 memory_only=true");
            Snapshot("enabled");
        }
        private static void Disable()
        {
            if (state.Pending || state.Running) { Tell("REFUSED service in flight; allow native completion or reload test save"); return; }
            state.Clear(); enabledProbe = false; ui = null; pulpit = null;
            Tell("DISABLED fallback=vanilla permit=0");
        }
        private void Update() { if (Input.GetKeyDown(KeyCode.F8)) panel = !panel; }
        private void OnGUI()
        {
            if (!panel) return;
            GUILayout.BeginArea(new Rect(18, 18, 435, 300), GUI.skin.box);
            GUILayout.Label("Extra Sermons RESEARCH " + Identity.Version + " (F8)");
            GUILayout.Label("Test slot 1 only. Real sermon changes the world.\nPermission is memory-only. No permit purchase/save.");
            GUILayout.Label(message);
            GUILayout.Label("Permit: " + (state.Permit ? "1" : "0") + " | " +
                (state.Running ? "native service running" : state.Pending ? "awaiting native start" : "idle"));
            GUI.enabled = compatible && !state.Pending && !state.Running;
            if (GUILayout.Button("Enable for loaded test slot 1")) Enable();
            GUI.enabled = enabledProbe && !state.Pending && !state.Running;
            if (GUILayout.Button("Grant ONE extra sermon"))
            {
                if (!Unlocked()) Tell("REFUSED test save / progression not ready");
                else Tell(state.Grant() ? "GRANTED permit=1 memory_only=true" : "REFUSED permit already held");
            }
            if (GUILayout.Button("Capture native state")) Snapshot("manual");
            if (GUILayout.Button("Disable / discard unused permission")) Disable();
            GUI.enabled = true;
            GUILayout.EndArea();
        }

        // Change one operand at a verified callsite; the native flag branch/ports still execute.
        private static IEnumerable<CodeInstruction> GateTranspiler(IEnumerable<CodeInstruction> source)
        {
            return Replace(source, AccessTools.Method(typeof(WorldGameObject), "GetParamInt", new[] { typeof(string) }),
                nameof(ReadEntryFlag), true);
        }
        private static IEnumerable<CodeInstruction> GraphWriteTranspiler(IEnumerable<CodeInstruction> source)
        {
            var result = Replace(source, Setter(), nameof(WriteGraphFlag), true);
            graphWrite = true; return result;
        }
        private static MethodInfo Setter() { return AccessTools.Method(typeof(WorldGameObject), "SetParam", new[] { typeof(string), typeof(float) }); }
        private static IEnumerable<CodeInstruction> ConfirmTranspiler(IEnumerable<CodeInstruction> source)
        {
            var result = Replace(source, Setter(), nameof(WriteConfirmationFlag), false);
            startWrite = true;
            result = Replace(result, AccessTools.Method(typeof(GS), "RunFlowScript"), nameof(LaunchNative), false);
            launchHook = true; return result;
        }
        private static List<CodeInstruction> Replace(IEnumerable<CodeInstruction> source, MethodInfo from, string to, bool addClosure)
        {
            var code = source.ToList();
            var matches = code.Select((c, i) => new { c, i }).Where(x => x.c.Calls(from)).ToArray();
            if (from == null || matches.Length != 1) throw new InvalidOperationException("Callsite mismatch: " + to);
            int index = matches[0].i;
            var call = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RuntimeProbe), to));
            if (addClosure)
            {
                var load = new CodeInstruction(OpCodes.Ldarg_0);
                load.labels.AddRange(code[index].labels); load.blocks.AddRange(code[index].blocks);
                code[index] = load; code.Insert(index + 1, call);
            }
            else { call.labels.AddRange(code[index].labels); call.blocks.AddRange(code[index].blocks); code[index] = call; }
            return code;
        }
        private static MyFlowNode NodeFrom(object closure, FieldInfo port)
        { return ((FlowOutput)port.GetValue(closure)).parent as MyFlowNode; }
        private static CustomFlowScript Script(MyFlowNode node)
        { return node == null || node.graphAgent == null ? null : node.graphAgent.GetComponent<CustomFlowScript>(); }
        private static bool IsEntryGate(MyFlowNode n)
        {
            if (!(n is Flow_PlayerFlag) || Script(n) == null || Script(n).script_name != "on_start_preaching" ||
                n.wgo == null || n.wgo.obj_id != "church_pulpit") return false;
            // Runtime Node.ID is intentionally unused. Match semantic neighbors/parameters.
            return n.inConnections.Any(c => Flag(c.sourceNode, "preaching_is_locked")) &&
                n.outConnections.Any(c => Flag(c.targetNode, "cup_20_reached")) &&
                n.outConnections.Count == 2;
        }
        private static bool Flag(Node n, string key)
        {
            var flow = n as Flow_PlayerFlag;
            var input = flow == null ? null : flow.GetInputPort("param") as ValueInput<string>;
            return input != null && input.value == key;
        }
        private static int ReadEntryFlag(WorldGameObject player, string key, object closure)
        {
            int native = player.GetParamInt(key);
            if (key != Weekly || native < 1 || !Unlocked() || !state.Permit || state.Pending || state.Running || player != boundPlayer)
                return native;
            try
            {
                var node = NodeFrom(closure, GatePort);
                if (!IsEntryGate(node)) return native;
                pulpit = node.wgo;
                Log("ENTRY_PROJECT native=" + native + " effective=0 permit=1 pulpit=" + pulpit.GetInstanceID());
                return 0;
            }
            catch (Exception e)
            {
                enabledProbe = false; state.Clear();
                Tell("ENTRY_DISABLED fallback=vanilla " + e);
                return native;
            }
        }
        private static void Opened(PrayCraftGUI __instance, WorldGameObject __0)
        {
            if (!Bound() || __0 == null || __0.obj_id != "church_pulpit") return;
            ui = __instance; pulpit = __0;
            Log("OPEN weekly=" + boundPlayer.GetParamInt(Weekly) + " permit=" + state.Permit);
        }
        private static void Selected(PrayCraftGUI __instance)
        {
            if (Bound() && ui == __instance) Log("SELECT prayer=" + (__instance.pray_craft == null ? "none" : __instance.pray_craft.id) + " permit=" + state.Permit);
        }
        private static void Hidden(ResourceBasedCraftGUI __instance)
        {
            if (Bound() && ui != null && __instance.gameObject == ui.gameObject)
                Log("HIDE permit=" + state.Permit + " confirming=" + inConfirm);
        }
        private static bool ConfirmPrefix(PrayCraftGUI __instance)
        {
            inConfirm = false;
            if (!enabledProbe || !Bound() || ui != __instance) return true;
            if (state.Pending || state.Running) { Tell("CONFIRM_REFUSED service already in flight"); return false; }
            int weekly = boundPlayer.GetParamInt(Weekly);
            if (weekly == 0) { Log("CONFIRM_NATIVE_FREE permit=" + state.Permit); return true; }
            if (!Unlocked() || pulpit == null || pulpit.obj_id != "church_pulpit" || __instance.pray_craft == null ||
                !__instance.CanClose() || !state.Reserve(false))
            { Tell("CONFIRM_REFUSED invalid extra-service state"); return false; }
            inConfirm = true;
            Log("CONFIRM_EXTRA prayer=" + __instance.pray_craft.id + " weekly=" + weekly);
            Snapshot("before_launch"); return true;
        }
        private static Exception ConfirmFinalizer(Exception __exception)
        {
            inConfirm = false;
            if (__exception != null && (state.Pending || state.Running))
                Tell("NATIVE_EXCEPTION no rewards/cleanup synthesized; reload test save. " + __exception);
            return __exception;
        }
        private static void WriteConfirmationFlag(WorldGameObject player, string key, float value)
        {
            if (inConfirm && state.Pending && Bound() && player == boundPlayer && key == Weekly && value == 1f)
            { Log("SUPPRESS_CONFIRM_WEEKLY actual=" + player.GetParamInt(key)); return; }
            player.SetParam(key, value);
        }
        private static CustomFlowScript LaunchNative(string id, CustomFlowScript.OnFinishedDelegate callback)
        {
            bool extra = inConfirm && state.Pending && Bound() && id == "pray";
            var result = GS.RunFlowScript(id, callback);
            if (!extra) return result;
            if (result == null) { state.LaunchFailed(); Tell("LAUNCH_NULL permit_retained=true"); return result; }
            if (service != null && service != result) throw new InvalidOperationException("Native graph identity changed during launch");
            service = result;
            Log("LAUNCH_RETURN graph=" + result.GetInstanceID() + " awaiting_start=" + state.Pending);
            return result;
        }
        private static void WriteGraphFlag(WorldGameObject player, string key, float value, object closure)
        {
            if (key == Weekly && Bound())
            {
                var node = NodeFrom(closure, WritePort);
                var cfs = Script(node);
                if (state.Pending && player == boundPlayer && value == 1f && cfs != null && cfs.script_name == "pray" &&
                    (service == cfs || (service == null && inConfirm)) &&
                    node.inConnections.Any(c => c.sourceNode is Flow_PlayerEnable) &&
                    node.outConnections.Any(c => c.targetNode is Flow_TeleportWGOToGDPoint))
                {
                    service = cfs;
                    if (!state.Started()) throw new InvalidOperationException("Start reservation missing");
                    Tell("STARTED sequence=" + state.Sequence + " graph=" + cfs.GetInstanceID() +
                        " permit=0 weekly_untouched=" + player.GetParamInt(key));
                    return;
                }
                Log("NATIVE_WEEKLY graph=" + (cfs == null ? "none" : cfs.script_name) + " before=" + player.GetParamInt(key) + " after=" + value);
            }
            player.SetParam(key, value);
        }
        private static void Terminating(CustomFlowScript __instance)
        {
            if (__instance != service || (!state.Pending && !state.Running)) return;
            Snapshot("native_graph_terminating");
            Log("GRAPH_TERMINATING sequence=" + state.Sequence + " started=" + state.Running + " success_not_inferred=true");
            state.Ended(); service = null;
            message = "Native graph ended. Observe control/donations; capture state. A second grant is now possible.";
        }
        private static void Loading()
        {
            if (!enabledProbe) return;
            Log("LOAD_INVALIDATE permit_discarded=true");
            state.Clear(); enabledProbe = false; boundSave = null; boundPlayer = null; boundRoot = null;
            service = null; ui = null; pulpit = null;
        }
        private static void Calculated() { if (Bound() && state.Running) Snapshot("native_calculation_return"); }
        private static void AnimationDone() { if (Bound() && state.Running) Snapshot("native_animation_finished"); }
        private static void CleanupBefore() { if (Bound()) Snapshot("native_cleanup_before"); }
        private static void CleanupAfter() { if (Bound()) Snapshot("native_cleanup_after"); }
        private static void BishopLookup(string __0, WorldGameObject __result)
        {
            if (!observing && Bound() && __0 == "npc_bishop")
                Log("NATIVE_BISHOP_LOOKUP found=" + (__result != null) + " sequence=" + state.Sequence);
        }
        private static void Finale(object __instance)
        {
            if (!Bound() || !state.Running) return;
            if (Script(NodeFrom(__instance, QuestPort)) == service && ((ValueInput<string>)QuestKey.GetValue(__instance)).value == "praying_finished")
                Snapshot("native_normal_finale_reached");
        }
        private static void Donated(object __instance)
        {
            if (!Bound()) return;
            var node = (MyFlowNode)DonationNode.GetValue(__instance);
            if (Script(node) == null || Script(node).script_name != "npc_church_visitor") return;
            Log("NATIVE_DONATION_RETURN visitor=" + (node.wgo == null ? "absent" : node.wgo.GetInstanceID().ToString()) +
                " amount=" + ((ValueInput<float>)DonationMoney.GetValue(__instance)).value.ToString("R", System.Globalization.CultureInfo.InvariantCulture) +
                " latest_sequence=" + state.Sequence);
        }
        private static void Snapshot(string phase)
        {
            if (!Bound()) { Tell("SNAPSHOT_REFUSED current test-save ownership absent"); return; }
            observing = true;
            try
            {
                var visitors = WorldMap.GetWorldGameObjectsByObjId("npc_church_visitor");
                var bishop = WorldMap.GetWorldGameObjectByCustomTag("npc_bishop", true);
                var box = WorldMap.GetWorldGameObjectByCustomTag("donat_box_inside", true);
                Log("SNAPSHOT phase=" + phase + " sequence=" + state.Sequence + " day=" + boundSave.day +
                    " weekly=" + boundPlayer.GetParamInt(Weekly) + " bishop=" + (bishop != null) +
                    " visitors=" + (visitors == null ? 0 : visitors.Count) +
                    " visitor_ids=" + (visitors == null ? "none" : string.Join(",", visitors.Select(v => v.GetInstanceID().ToString()).ToArray())) +
                    " donation_box_money=" + (box == null ? "absent" : box.GetParam("_money").ToString("R", System.Globalization.CultureInfo.InvariantCulture)));
            }
            finally { observing = false; }
        }
    }
}
