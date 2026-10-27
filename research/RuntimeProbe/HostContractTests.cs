// SPDX-License-Identifier: MPL-2.0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;

// Executes only metadata inspection and our IL transformations, never game methods.
internal static class HostContractTests
{
    private static string game, output;
    private static int checks;
    private static void Check(bool b) { checks++; if (!b) throw new Exception("Host contract assertion " + checks); }
    private static int Main(string[] args)
    {
        game = args[0]; output = args[1];
        AppDomain.CurrentDomain.AssemblyResolve += Resolve;
        try { Run(); Console.WriteLine("Native metadata/IL contracts: " + checks + " assertions passed (no game execution)."); return 0; }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }
    private static Assembly Resolve(object sender, ResolveEventArgs e)
    {
        string name = new AssemblyName(e.Name).Name + ".dll";
        foreach (string path in new[] { output, Path.Combine(game, "Graveyard Keeper_Data", "Managed"), Path.Combine(game, "BepInEx", "core") })
        { var file = Path.Combine(path, name); if (File.Exists(file)) return Assembly.LoadFrom(file); }
        return null;
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Run()
    {
        var host = Assembly.LoadFrom(Path.Combine(game, "Graveyard Keeper_Data", "Managed", "Assembly-CSharp.dll"));
        var probe = Assembly.LoadFrom(Path.Combine(output, "ExtraSermons.dll"));
        var type = probe.GetType("ExtraSermons.Research.RuntimeProbe", true);
        var specs = new[] {
            new[] { "FlowCanvas.Nodes.Flow_PlayerFlag+<>c__DisplayClass0_0", "<RegisterPorts>b__0", "GateTranspiler", "ReadEntryFlag", "1" },
            new[] { "FlowCanvas.Nodes.Flow_SetPlayerParam+<>c__DisplayClass0_0", "<RegisterPorts>b__0", "GraphWriteTranspiler", "WriteGraphFlag", "1" },
            new[] { "PrayCraftGUI", "OnPrayButtonPressed", "ConfirmTranspiler", "LaunchNative", "2" }
        };
        foreach (var spec in specs)
        {
            var target = AccessTools.Method(host.GetType(spec[0], true), spec[1]);
            Check(target != null);
            var original = PatchProcessor.GetOriginalInstructions(target).ToList();
            var transform = AccessTools.Method(type, spec[2]);
            var changed = ((IEnumerable<CodeInstruction>)transform.Invoke(null, new object[] { original })).ToList();
            Check(changed.Count(c => c.operand is MethodInfo && ((MethodInfo)c.operand).DeclaringType == type) == int.Parse(spec[4]));
            Check(changed.Any(c => c.operand is MethodInfo && ((MethodInfo)c.operand).Name == spec[3]));
            // All call substitutions preserve the stack contract, with only the explicit closure operand added.
            foreach (var c in changed.Where(c => c.operand is MethodInfo && ((MethodInfo)c.operand).DeclaringType == type))
            {
                var called = (MethodInfo)c.operand;
                Check(c.opcode == System.Reflection.Emit.OpCodes.Call && called.IsStatic);
                if (called.Name == "ReadEntryFlag" || called.Name == "WriteGraphFlag")
                    Check(changed[changed.IndexOf(c) - 1].opcode == System.Reflection.Emit.OpCodes.Ldarg_0);
            }
            bool rejected = false;
            try { transform.Invoke(null, new object[] { new List<CodeInstruction>() }); }
            catch (TargetInvocationException e) { rejected = e.InnerException is InvalidOperationException; }
            Check(rejected);
        }
        string version = (string)probe.GetType("ExtraSermons.Research.Identity", true).GetField("Version", BindingFlags.NonPublic | BindingFlags.Static).GetRawConstantValue();
        Check(probe.GetName().Version == new Version(version + ".0"));
        // Every diagnostic hook must resolve unambiguously, not choose a guessed overload.
        foreach (var spec in new[] { new[] { "PrayLogics", "CalculatePray" }, new[] { "PrayCraftGUI", "Open" },
            new[] { "MainGame", "StartGameLoading" }, new[] { "WorldMap", "GetWorldGameObjectByCustomTag" } })
            Check(host.GetType(spec[0], true).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Count(m => m.Name == spec[1]) == 1);
    }
}
