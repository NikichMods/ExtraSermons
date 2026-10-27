# Extra Sermons — Royal Services and Sermon Flow Research

Research checkpoint: 2026-10-27. **Research only; production gate BLOCKED.** This document records bounded source findings and missing evidence, not accepted runtime implementation or product semantics.

## Question and evidence boundary

Find the native purchase, sermon-availability, consumption and completion owners needed for one paid extra sermon without changing the ordinary weekly service or prayer/reward mechanics. The target game build, runtime loader and installed compatibility for Extra Sermons are not yet verified.

Evidence examined:

- `NikichMods/GraveyardKeeperResearch@5d534fe2594c01b4245c548e9bb8df7935e5b4f2`: `docs/RESEARCH_INDEX.md`, `docs/DIALOGUE_QUEST_AND_FLOWCANVAS.md`, `docs/CRAFTING_INVENTORY_AND_TRADING.md`, `docs/GAME_INTERNALS.md`, `docs/UI_INPUT_TIME_AND_ENVIRONMENT.md`.
- `NikichMods/PrayerClarity@ce915344064655cde5273cea8a10ec4ec2beb2e5`: `docs/PRAYER_MECHANICS.md` and relevant pulpit source. Its original-game 1.407 sermon calculation, payouts and native pulpit presentation have existing bounded evidence.
- Public source inspection only: `Kupie/GYK_DECOMP@6abf79199d92482af1c7573870dd9a20ec2270b9`, particularly `Assembly-CSharp/PrayCraftGUI.cs`, `PrayLogics.cs`, `ChurchPulpit.cs` and the `Flow_*Pray*` nodes. This is not a fresh installed-game verification.
- Existing alternative, **not** native evidence or an adopted design: `p1xel8ted/Graveyard-Keeper-Mods@92b99b0dbe717caa9a895cce37e98d32ad9082a7`, `src/PrayTheDayAway/Patches.cs`.

## Established within those evidence limits

1. `PrayCraftGUI.OnPrayButtonPressed` sets player parameter `prayed_this_week` to 1, hides the resource-craft UI, then launches `GS.RunFlowScript("pray", null)`. Opening `PrayCraftGUI.Open` is a distinct prior action; that inspected method does not set the weekly flag. This **does not** yet establish the permit-consumption boundary, final completion, or all upstream guards.
2. `PrayCraftGUI.OnResourcePickerClosed` returns without selecting a new prayer when the item argument is null. A cancelled picker must not be treated as a completed sermon, but the larger cancellation lifecycle still needs inspection.
3. `PrayCraftGUI.OnFinishedPrayBuffAnimation` invokes a completion callback; `ChurchPulpit.OnPrayAnimationDone` calls it. This identifies a late animation boundary, **not yet** a verified transaction/permit commit point. PrayerClarity separately documents native `FlowCanvas/pray` calculation and reward distribution, which Extra Sermons should preserve.
4. The shared FlowCanvas research verifies generic graph resource addressing, `FlowOutput.Append` continuation semantics, and native SmartRes/player sufficiency evaluation. None of that verifies the **Royal Services-specific** graph, payment, or button lifecycle.
5. The third-party Pray the Day Away mod clears `prayed_this_week` and sometimes intercepts pulpit interaction. It demonstrates an existing mod's approach, not that broadly resetting the stock weekly flag is safe for our one-use permit and ordinary sermon invariant.

## Still unknown — do not infer

- Royal Services world object, authored FlowCanvas graph/resource path, menu answer owner, native currency price/spend success/cancel callbacks, progression conditions and localization route.
- Stock pulpit access/weekly-state guard and reset owner, including normal-day behavior and interactions with a permit.
- A final reliable outcome/commit boundary for consuming **exactly one** extra authorization, including cancellation, interrupted animations, scene changes and save/load.
- Safe per-save storage if unused authorization persists, and whether using native player/quest state is adequate without mirroring mutable data.
- Compatibility with other pulpit/sermon mods, target loader/build support and an installation-level acceptance method.

## Engineering checkpoint

**BLOCKED** for production changes: the observable goal and preserved invariants are defined in `docs/DESIGN.md`, but canonical Royal Services owners, final money/sermon commit writers, full affected interactions and representative real-path acceptance evidence remain missing. Do not add speculative patches, duplicate reward logic or the unrelated prayer-degradation cost used by other mods.

**Next bounded investigation:** inspect the real original-game 1.407 authored Royal Services and church-pulpit resources plus their native C# call paths. Record only derived identifiers, graph routing/ports, payment writer, weekly gate/reset, save owner and source identity; do not commit extracted assets or decompiled game payloads. Establish the least-complex safe mechanism from that evidence, then revisit the user's unresolved permit lifetime, stacking, normal-day and consumption choices. The absence of those resources in current GitHub evidence is not itself a runtime conclusion.
