# Native extra-sermon runtime experiment

## Question, authorization and boundary

2026-10-27: the user authorized an isolated research prototype and safe testing on a separate test save, subsequently identifying **slot 1** as that test save. Base: `d360a2e3c3c0d3f1ffbb5a69c1af559d669650c2`, accepted design at `main@7422641258aaf27f0f73f9606266c56eed0d1961`. Working branch: **`codex/sermon-runtime-probe`**. Main is not changed.

Question: can the existing prayer window and actual `pray` graph execute an additional service with no bishop, preserve native weekly access and rewards, and finish/repeat without broken visitor cleanup? The dependency `pray FindWGO -> delayed bishop event -> TryRemoveStackedChurchVisitors` is statically established, **not a reproduced defect**. No cleanup fix is included.

Static research cannot establish Unity animation callbacks, actual missing-target behavior, control return or delayed visitor movement/cleanup. Stock gameplay cannot directly exercise a weekly-blocked off-day service. A small callsite-input adapter plus independent native observations is therefore justified. Replacing `pray`, globally resetting/restoring the weekly flag, manually awarding rewards, or fabricating an NPC would manufacture or obscure the answer and is excluded.

No Royal Services, economics, persistent authorization, final pulpit indicator, prayer cost, reward formula or PrayerClarity layout/text changes. The adopted product semantics in DESIGN remain unchanged. This is original research code, **not a first production version**.

## Readiness checkpoint before runtime handoff

The research adapter is READY for bounded implementation/build against the pinned installed inputs below. **Production gates remain BLOCKED; installed behavior remains unverified.**

| Property | Native owner / final consumer | Intervention and blast radius | Preserved invariants / decisive evidence |
| --- | --- | --- | --- |
| Open blocked-week pulpit | `Flow_PlayerFlag` registered input closure's single `WorldGameObject.GetParamInt`; live `on_start_preaching`, semantic neighbors `preaching_is_locked` and `cup_20_reached`; then native OpenCraft | Substitute only that read's operand result, only with bound test save + one permission + active `church_pulpit`. The shared closure otherwise returns the native value. No graph/delegate/template mutation. | Native overhead/drop, preaching/cleric/inactive/busy gates and native window remain. Exact graph hashes, closure/callsite contract and live neighbors are checked. Real open/select/cancel/reopen is pending. |
| Preserve weekly state and consume once at actual start | `PrayCraftGUI.OnPrayButtonPressed` C# write and native `pray` early `Flow_SetPlayerParam`, before teleport/calculation | Replace only these callsites with guarded forwarding helpers. Reserve on valid confirmation; suppress C# write while launching; consume at the actual early graph writer, matching returned/in-progress native graph identity and neighbors. All other writers delegate to native. | Free service re-evaluated at confirmation; retains permission. No flag snapshot/restore, no global setter patch. Bishop/story writes survive, including while an extra service runs. Policy and actual IL transformations checked; real launch/transition behavior pending. |
| Evidence and save containment | current `GameSave._inventory` reference-identical to live player's `_data`; native `save_slot.filename_no_extension`; `StartGameLoading` invalidation | Memory-only permission and graph/UI/save references; default disabled; explicit panel enable; exact slot `1`; no serialization/config writes | Main saves cannot enable the adapter. Any load discards permission/session binding. Real reward/world changes can still enter the test save through normal saving. Back up test save first. |
| Native completion and repeat | native CalculatePray, animation, normal `praying_finished` checkpoint, graph termination, bishop lookup, donation return and cleanup helper | Read-only diagnostic hooks, visitor instance IDs and monotonic timestamps; no cleanup/reward/control synthesis | Graph termination is not a success receipt. Correlate input/camera, donations and later visitor state with user observation. Second grant only after first native graph ends; delayed first cleanup is intentionally left observable. |

References: SERMON_FLOW_RESEARCH sections 3–4 and 11–14; shared GAME_INTERNALS current-save ownership and BepInEx numeric version; shared native FlowCanvas/Pray GUI ownership. `FlowOutput.BindTo` copies delegates, which makes post-bind input replacement unsuitable; the chosen callsite projection avoids that separate lifetime assumption.

Containment: pinned host file hashes at initialization; exact resource graph string hashes when enabling; each transpiler requires exactly one matching native call. Missing initialization contracts unpatch only this probe and retain vanilla. Unknown live entry state returns the native read; invalid extra confirmation is refused. Native service exceptions are reported and rethrown, never turned into fake success/refund. No launch retry, hidden repair or injected cleanup.

## Target, build and exact identity

Verified local preparation: original GK **1.407**, Windows Steam app 599140, manifest build **22583570**; BepInEx startup log identifies **5.4.23.5**, Unity **2020.3.17**. Manifest/log evidence does not prove this probe loaded.

| Local reference | SHA-256 |
| --- | --- |
| Assembly-CSharp.dll | `e72e4270e4b88dd0a87ca23c9cf1750aec4c4a0fedb40b6d2dae7902fc9c7fd8` |
| Assembly-CSharp-firstpass.dll | `9dc6def3b7715dd27eeb168ddc0af47e31c6f38d3fbee24bf592899392026498` |

Source: `research/RuntimeProbe`. Canonical numeric version: `Identity.cs::Version`, **0.1.0**. Separate GUID: `nikich.graveyardkeeper.extrasermons.runtimeprobe`. Canonical installed name **ExtraSermons.dll**; immutable handoff **ExtraSermons-0.1.0.dll**. Version attributes and filename derive from that owner.

Local build: installed .NET SDK **8.0.426** Roslyn; .NET Framework **4.7.1** reference assemblies; optimized deterministic AnyCPU DLL; no downloaded tools/dependencies. Run from a clean committed checkout:

```powershell
./scripts/Build-RuntimeProbe.ps1 -GameRoot '<existing Graveyard Keeper installation>'
```

The script compiles/runs 20 authorization-policy assertions and 23 native metadata/IL contract assertions against actual installed references. The latter exercise our real transpilers, missing-callsite rejection and required signatures without invoking game methods. Generated identity records exact source SHA, version, compiler and DLL SHA-256. `-AllowDirty` is only a scratch compile, never a handed candidate. Hosted CI separately compiles/runs the same host-free policy with .NET 8; it does not emulate Unity or distribute host assemblies. Local clean build is the defined native-integration handoff gate.

**Candidate 0.1.0:** clean source **`71806c52eb553ac78076d1573b207ee36cebdad8`**, optimized deterministic local build passed all **20 policy + 23 native metadata/IL assertions**. DLL SHA-256 **`808227229ae0c193180f5fef4528da560f414a91e9ca58f8ef8dbc780bd174e6`**. Both `ExtraSermons-0.1.0.dll` and canonical installed `ExtraSermons.dll` were independently hashed and match. Assembly/plugin/file version is 0.1.0 (normalized assembly version 0.1.0.0). This is the frozen candidate; later documentation commits do not change its source identity or rebuild its bytes.

**Installation 2026-10-27 14:37 UTC:** game process absent at replacement; no previous ExtraSermons payload found. Installed one canonical DLL, preserving all pre-existing plugin DLL hashes. Test slot 1's data, info and its two existing backups were copied to ignored private evidence storage before installation; originals were not changed. The previous BepInEx session log was also snapshotted there before any restart. Machine paths/reference manifests remain in ignored `private/runtime-probe/installed-0.1.0.json`, `private/runtime-probe/before-0.1.0/` and `artifacts/runtime-probe/identity.json`, not public records. **Status: installed, not yet loaded/operated; awaiting user gameplay.** No probe session exists to analyze yet.

Hosted policy run: [40522750158](https://github.com/NikichMods/ExtraSermons/actions/runs/40522750158), exact candidate source above: **policy job completed successfully**, including compilation and the host-free authorization assertions. This is additional evidence, not installed-host acceptance.

The existing local mod set is preserved. Preparation found PrayerClarity **Rebalanced 0.2.52** installed, SHA-256 `b1724151a9a76b9f72b6e0f91b02be1e7400e0e396d5495caa715ed3c57843bd`; its compatibility is not established by prior 0.2.51 source research. This first run is explicitly in the current modded installation, not stock acceptance or a compatibility claim. No other plugin is removed/swapped.

## Minimal user-operated experiment

Codex handles build, verified installation, test-save backup and log collection. The user handles gameplay only. The panel is visible initially and can be toggled with **F8**. Buttons use short English diagnostic labels, separate from native pulpit UI. This is not the product's final indicator.

1. Launch the prepared game and load test **slot 1**. Open the church on a day when the bishop is absent; do not use another slot. Click **Enable for loaded test slot 1**, then **Grant ONE extra sermon**. Permission must show 1. Enable refusal means stop and report it; do not bypass the guard.
2. Interact with the existing pulpit. Open/select/change/cancel/back out and reopen the native prayer window. Permission must remain 1. Choose a real available prayer and confirm once.
3. Observe real sermon animation, faith/effects, camera/control return, visitors leaving and donations. Do not save over the pre-test state for recovery. If it hangs/errors or control does not return, stop the experiment; report where. Do not retry or grant another permission.
4. If the first ceremony completes normally, promptly click **Grant ONE extra sermon**, choose a prayer at the same pulpit and confirm a second ceremony. This targets delayed first-service cleanup. Observe whether the new visitors survive and the second service completes. Click **Capture native state** after donations settle.
5. Report the two visible outcomes. Codex reads and snapshots the actual local log. Further bishop-arrival/free-week boundary checks follow only after this basic off-day return is understood. They remain required evidence for eventual product acceptance.

After a completed test, **Disable / discard unused permission** returns the adapter to vanilla. Any save loading discards the in-memory permission and requires explicit re-enable. The actual ceremony's native world/reward effects are save-persistent if the player saves; removing a permission cannot undo those effects. Reload the untouched test baseline when done. A running/hung native service is never forcibly “completed” by the panel.

## Evidence and unresolved hypotheses

`ES_PROBE` lines carry monotonic `t`, sequence and, where relevant, native graph or visitor instance identity. Important events: READY, ENABLED, GRANTED, ENTRY_PROJECT, OPEN/SELECT/HIDE, CONFIRM_NATIVE_FREE/EXTRA, SUPPRESS_CONFIRM_WEEKLY, LAUNCH_RETURN, STARTED, native calculation/animation/normal-finale snapshots, NATIVE_DONATION_RETURN, NATIVE_BISHOP_LOOKUP, GRAPH_TERMINATING, native cleanup before/after and NATIVE_WEEKLY writers. Missing markers are evidence gaps, not implicit passes.

| Hypothesis | Current result | Next evidence / consequence |
| --- | --- | --- |
| One scoped weekly read can reach native picker without bypassing other locks | Static/native-contract support; no installed return | Exact ENTRY_PROJECT + native OPEN/cancel/reopen. Failure localizes to this adapter before changing service logic. |
| Two scoped start writes can preserve real bishop/week changes | Policy/IL checks only | STARTED + native weekly markers and later bishop/free-week transition; no blind restoration. |
| Bishop absence permits normal late completion/deposits | **Unverified** | Native normal-finale, donation/termination markers and user control/camera observation. Missing bishop lookup is not automatically a fatal defect. |
| Repeated service survives prior delayed cleanup | **Unverified** | Two sequence IDs, per-visitor IDs, cleanup before/after and user second-service observation. If failing, localize before proposing a separate cleanup hypothesis. |

No runtime hypothesis has yet been confirmed or refuted. No runtime blocker is reproduced. Pending installed execution is the next evidence gate; compilation does not establish safe game integration. Reusable **confirmed** host findings will be distilled into GraveyardKeeperResearch after the return; no speculative shared-knowledge update is justified yet.
