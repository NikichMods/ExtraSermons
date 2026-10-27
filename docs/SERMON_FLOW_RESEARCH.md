# Extra Sermons — Royal Services and Sermon Flow Research

Research checkpoint: **2026-10-27**. **Static investigation complete within the scope below; production changes remain BLOCKED.** No game execution, installed probe, production code, build, game/config/plugin/save mutation or release was performed. Sections1–8 preserve the native-flow evidence from `360917f789e83ba8145a3e0d736118d39312f194`; sections9–14 add the contextual pulpit/PrayerClarity investigation. Product status has been reconciled with accepted `main@7422641258aaf27f0f73f9606266c56eed0d1961` instead of retaining superseded open-decision claims.

**Adopted scope:** one non-expiring, non-inventory authorization per save; free native service first; consumption at valid actual extra-service start, no automatic refund after interruption; same native pulpit/window and compact document indicator. Representation, safety and compatibility still require proof. This branch incorporates the accepted main design/contract while retaining the original research. The only merge conflict was the old DESIGN evidence/next-step paragraph; accepted main's complete DESIGN text was preserved.

## 1. Question and exact source identity

Find the native purchase, sermon-availability, consumption and completion owners needed for one paid extra sermon without changing the ordinary weekly service or prayer/reward mechanics. The original-game **installed stock file identity is now verified**. Extra Sermons implementation and supported loader/build combinations are not verified.

Installed Windows Steam inputs, read-only:

| Input | Identity |
| --- | --- |
| Steam install | app ID `599140`, locally recorded build ID `22583570`; manifest metadata is not runtime acceptance |
| `Managed/Assembly-CSharp.dll` | SHA-256 `e72e4270e4b88dd0a87ca23c9cf1750aec4c4a0fedb40b6d2dae7902fc9c7fd8`; assembly version `11.0.0.0`; MVID `6f50b8e7-156b-49ac-bbe8-7505894b2364` |
| Game version | `LazyConsts.get_VERSION`, IL0000 returns float `1.406999945640564`, the float representation of **1.407** |
| `resources.assets` | SHA-256 `215c7981901a4b72d5db717666ba47ad3cc032527c95f58dc39d8af1293a69ca`; Unity `2020.3.17f1` |
| `Managed/Assembly-CSharp-firstpass.dll` | SHA-256 `9dc6def3b7715dd27eeb168ddc0af47e31c6f38d3fbee24bf592899392026498`; dependency for type-tree generation, not exhaustively disassembled |

The two primary hashes exactly match shared 1.407 research. These are stock file facts, not behavior after loaded mods patch them.

Method: `dnfile 0.18.0` targeted metadata/IL reads with .NET opcode metadata; `UnityPy 1.23.0` and existing TypeTreeGenerator using local Managed definitions. No game methods were invoked. Private original scripts, selected IL and extracted inputs are ignored under `work/`; no binaries, raw graphs or copied game source are committed. No broad decompilation or universal infrastructure was created.

An initial indiscriminate MonoBehaviour type-tree read failed on unrelated layouts. Graph inspection was narrowed to the verified FlowScript reference `(fileID=1,pathID=608)` and relevant byte strings before parsing `_serializedGraph`. This was an inspection-scope correction, not a game defect. Node numbers below are JSON `$id`, not editor indices; disconnected/no-ID authored nodes are not assumed reachable.

Reused evidence:

- Shared research at local HEAD **`98b6f9a521a9d375f2ce72da7237660d4fa652ec`**: `docs/RESEARCH_INDEX.md`, dialogue/FlowCanvas, crafting/inventory/trading, game-internals and relevant UI/economy entries. Initial checkpoint used `5d534fe2594c01b4245c548e9bb8df7935e5b4f2`.
- [PrayerClarity stock mechanics](https://github.com/NikichMods/PrayerClarity/blob/ce915344064655cde5273cea8a10ec4ec2beb2e5/docs/PRAYER_MECHANICS.md) at **`ce915344064655cde5273cea8a10ec4ec2beb2e5`**: bounded accepted read-only probes establish calculation, separate base/bonus payouts and buffs. Its exact Animator asset edge remains unverified. Rebalanced behavior is not stock evidence.
- Shared [GAME_INTERNALS.md](https://github.com/NikichMods/GraveyardKeeperResearch/blob/98b6f9a521a9d375f2ce72da7237660d4fa652ec/docs/GAME_INTERNALS.md), native per-save player-root carrier/current-save ownership sections: actual serializer fixtures and bounded prior installed acceptance. Those experiments were not repeated or turned into Extra Sermons acceptance.
- Historical comparisons only: `Kupie/GYK_DECOMP@6abf79199d92482af1c7573870dd9a20ec2270b9` and Pray the Day Away at `p1xel8ted/Graveyard-Keeper-Mods@92b99b0dbe717caa9a895cce37e98d32ad9082a7`. Neither proves current stock behavior. Findings below use installed IL/resources or explicitly reused native evidence.

### Graph locator manifest

`CustomFlowScript.GetGraph(id)` resolves `FlowCanvas/` + `id` through `SmartResourceHelper.GetResourceAs<FlowScript>`. Selected MonoBehaviours in the pinned resources:

| Script ID | Asset path ID | Role |
| --- | ---: | --- |
| `npc_royal_box` | 150551 | Royal Services dialogue |
| `on_start_preaching` | 150795 | active pulpit gates |
| `church_pulpit_inactive_event` | 150615 | inactive pulpit refusal |
| `npc_bishop` | 150512 | arrival/departure flag writes |
| `on_key_char_spawn` / `on_key_char_remove` | 150767 / 150766 | periodic arrival/departure event routes |
| `bishop_spawn` | 150311 | separate authored spawn script, not assumed the scheduler route |
| `pray` / `npc_church_visitor` | 150856 / 150516 | sermon and donation lifecycle |
| `change_pulpit` / `final_ceremony` | 150609 / 150633 | additional flag writers |
| `test_multi_answer` | 150428 | test graph, not ordinary gameplay evidence |

SHA-256 of decoded `_serializedGraph` string encoded UTF-8 without BOM, without reformatting:

- Royal: `c8f77ffd76de10db44484e9fdbe3f736be1fe768a83191786baf867804f11bfd`.
- Pulpit entry: `90ba2594e739ef464eb835c86ba6429a693fabf364a050a4f391ce18143628d3`.
- Bishop: `0ee96a35dc3710ef7771ad1ae8f1ab09e5f9869a2aff7fdd5f012222b9103b3d`.
- Pray: `4c89fa682164cee81718f406b05b2c3c5ff1d4788b33a9cf1982b5d7ad2e8880`.

## 2. Royal Services: object, menu and transaction

**Status: installed authored-data + static IL, not a new live test.**

`game_data.objs_data[id=npc_royal_box]`: `attached_script=npc_royal_box`, NPC type, `npc_in_list=true`, `custom_head_spr=head_royal`. `InteractionComponent.Interact` calls target `WorldGameObject.Interact`; its attached-script branch sends `interaction` (or the applicable stored custom event) through `WorldGameObject.FireEvent`.

Royal graph normal entry: CustomEvent **2** (`interaction`) -> **7** `Flow_ParamFlag`, object parameter **`box_accesible`** (native spelling). `==0` -> refusal **9** (`box_closed`); `>=1` -> player-disable **11** -> root `Flow_MultiAnswer` **13**. Preserve that parent access/progression boundary.

Root13 contains `mailbox_permission`, `mailbox_1a_1`, `mailbox_1a_2`, `mailbox_1a_3`, two Souls quest entries and `Back`. `Back/out_6` reaches player-enable106. Submenus40/63/90 own tier documents.

Two concrete native purchases:

| Purchase | Answer-data wiring | Selected execution |
| --- | --- | --- |
| Repeatable exhumation permission | money SmartRes276 (`GameRes,money,1.75`) -> Answer279.price; item SmartRes285 (`permission,1`) ->279.reward;279.Value -> root13 answer#0 | root out_0 -> function88; debit/grant happen before continuation, not on menu opening |
| Building permission, explicit confirmation | menu40#2 -> menu45 (`Buy`,`Back`); price345 (`money,20`) and reward336 (`building_permission,1`) -> Answer339 ->45#0 | Buy/out_0 -> blacklist48 -> function50; Back/out_1 ->50 without selecting the priced answer or running48 |

Some authored data connections are duplicated, e.g.345->339; do not infer that adding duplicate execution wiring is safe.

### Sufficiency, spending and success are separate

1. `Flow_Answer.Invoke(price,lock,reward)` only constructs `AnswerData.d_price/d_lock/d_reward`.
2. `Flow_MultiAnswer.RegisterPorts` creates indexed inputs/outputs. Its flow-input closure builds visual answers and calls `AnswerData.FillVisualData`/`MultipleAnswerData.FillVisualData`, then `WorldGameObject.ShowMultianswer` with an ID callback. `MyFlowNode.wgo` is the owning NPC context; unset Talker defaults to the **player**. Generic player/talker identity does not isolate Royal Services.
3. `AnswerData.FillVisualData` calls current `player.IsEnough` for price and lock. Failure sets `can_be_picked=false` and red price state. GameRes sufficiency delegates to `Item.IsEnoughParam`; item sufficiency delegates to native item checks. Shared compound AND evidence remains applicable.
4. `MultiAnswerGUI.ShowAnswers` filters locked `@` IDs and blacklisted phrases and resolves text through `SpeechBubbleGUI.SpeechText`. Presentation performs no purchase.
5. **Actual debit/grant owner: `MultiAnswerOptionGUI.OnChosen`.** It returns when `_can_be_picked=false` or widget alpha<0.5; then clears `_can_be_picked`. For a normal answer: `player.RemoveSmartRes(d_price)` -> if reward exists, `player.ReceiveSmartRes(d_reward,null)` -> `_answer_gui.OnChosen(_answer_id)`. Compound answers remove each constituent price. Pinned IL offsets:003b clears pickability,00ca removes normal price,00fc receives reward,010d invokes selection continuation.
6. `MultiAnswerGUI.OnChosen` invokes `_on_chosen(id)` before starting disappearance. Flow_MultiAnswer's callback maps `answers.IndexOf(id)` into previously registered `outs` and calls that output.

`WorldGameObject.RemoveSmartRes` returns **void**. For GameRes it writes current param minus cost through `Item.SetParam`; GameRes.Set maps `money` to `_money`. It does **not** recheck sufficiency or return a transaction receipt. OnChosen trusts the earlier visual pickability snapshot. There is no inspected atomic price+reward rollback protocol. Callback arrival proves native call ordering, not crash-safe completion; a future hook must avoid stale-funds spending and double debit.

Currency unit is **silver**: installed `Trading.FormatMoney` computes gold as floor(value/100), remaining whole silver, fractional hundredths as copper. Thus **one gold=`money:100`**. Existing20/50/1200 prices agree with shared economy evidence.

### Extension capability and its limits

`ReceiveSmartRes(GameRes)` adds its atom to player Item params. A native priced answer could grant a namespaced numeric authorization without a new physical item or duplicated currency formula. Main subsequently adopted a persistent non-item authorization limited to one unused unit; this carrier remains a technical capability rather than an implemented transaction/schema.

Two gaps prevent calling this an implemented extension seam:

- New GameRes reward presentation defaults to `res_<param-name>` in `SmartRes.FillVisualData`; no icon exists here by proof. Localization/icon/price display need a scoped design.
- Appending strings after Flow_MultiAnswer port registration breaks the consistency of `answers`, `ins` and `outs`; an injected unknown ID reaches `IndexOf=-1` in the stock callback. Viable families are a bounded graph/data extension before registration or a Royal-specific visual-answer/callback wrapper that intercepts only its own ID and delegates existing answers unchanged. Clone/cache lifetime, context binding, Back/confirmation and rollback are not yet proved. No generic “add shop service” API was established; replacing the whole graph is unjustified.

## 3. Pulpit gate and weekly/calendar ownership

**Status: installed authored-data + static IL.**

Object `church_pulpit`: group `pulpit`, `has_craft=true`, `interaction_1.script=on_start_preaching`, hint `start_praying_hint`; its ordinary interaction definition has no weekday expression. `church_pulpit_inactive` instead runs `church_pulpit_inactive_event`: Start2 -> refusal4 (`church_pulpit_infobox`) -> End6. A permit must not silently activate an unopened/inactive pulpit.

Reachable normal `on_start_preaching` route:

1. Start2 -> overhead check4; if carrying an overhead object, Drop6 and clear17 precede the gates.
2. Player flag28 `preaching_is_locked`:>=1 -> refusal42 -> End36. Preserve this lock.
3. Otherwise flag30 `prayed_this_week`:>=1 -> InfoBox40 -> End36;==0 ->32.
4. Flag32 `cup_20_reached`:==0 -> cleric refusal34 -> End36;>=1 -> `Flow_OpenCraft`38 -> End36.

The authored cinematic branch beginning44 has no route from Start2 in the inspected graph; do not assume its writes run on every interaction.

Flow_OpenCraft's input closure calls `GUIElements.OpenCraftGUI(MyFlowNode.wgo)` and immediately its output. End36 therefore means **end of the opening script**, not a service. GUIElements preserves a busy-craft check, recognizes object group `pulpit` and calls `PrayCraftGUI.Open` (IL0062..0075). It also has a later PrayCraft fallback for other craft routing: broad craft-UI interception has other consumers.

### The weekly flag also represents the church-day window

- `npc_bishop` event2 **on_came_to_finish** -> refresh quality7/9 -> clear travel state11 -> direction13 -> **15 sets prayed_this_week=0** -> presence17 -> story gates.
- Event434 **going_home** -> presence439 -> **441 sets prayed_this_week=1** -> departure/story branches. Access closes even if no sermon occurred.
- `game_data.logics_data[bishop_spawn]`: start_time≈6.9, period6, primary condition1, `SetPpar("cur_key_char",1)`, `:on_key_char_spawn`.
- `logics_data[bishop_remove]`: start_time≈1.9, period6, same key-character selection, `:on_key_char_remove`.
- `LogicData.GetNextExecutionTime/CheckPeriod/Update/Execute` owns periodic game-time execution. `RunScripts` interprets a sole leading-colon entry as a **FlowScriptEngine event**, not starting a same-named script.
- `on_key_char_spawn`: event159 -> param46 -> switch49 branch1 -> helper89(`bishop`). Helper sets `finish_event=on_came_to_finish` at115; movement44 takes it via136 and fires it on destination arrival.
- `on_key_char_remove`: branch1 ->82(`npc=bishop`); movement23 toward start ->25 fires **going_home** on that NPC. The weekly lock is set at departure, not after disappearance.

These are native schedule coordinates, **not measured wall-clock arrival times or universal load/catch-up guarantees**. Actual reset follows arrival movement. The ordinary pulpit gate consumes the bishop-controlled flag rather than separately querying a weekday enum; preserve that ownership instead of inventing a parallel calendar.

Bounded direct flag search: Assembly-CSharp literal use is OnPrayButtonPressed. Six matching serialized FlowScripts: npc_bishop, change_pulpit, on_start_preaching, pray, final_ceremony, test_multi_answer. Additional writes are change_pulpit4=1, entry cinematic67=1, pray6=1, final_ceremony231=1 and test86=1 (test reader55). Test reachability is not gameplay evidence. This does not cover computed names, other assemblies, mods or future builds. Do not globally patch all GetParam/SetParam/Flow_PlayerFlag consumers.

## 4. Service lifecycle and consumption boundaries

**Status: installed static routing plus reused PrayerClarity payout evidence; no new runtime acceptance.**

| Stage | Native owner / graph route | Permit implication |
| --- | --- | --- |
| Open | pulpit gates ->38 -> GUIElements -> PrayCraftGUI.Open | No service committed |
| Initialize | Open calls ResourceBasedCraftGUI.Open as PrayCraft, clears `pray_craft`, reads church quality; no weekly write | Opening is not consumption |
| Select/cancel | ResourceBasedCraftGUI.ChooseItem/OnResourcePickerClosed -> PrayCraftGUI.OnResourcePickerClosed(item); null returns immediately in the latter, non-null assigns selected linked craft | Selection is not consumption; cancelling a replacement picker need not clear an earlier selection |
| Close | ResourceBasedCraftGUI.OnPressedBack -> BaseGUI.OnClosePressed; Hide consults PrayCraftGUI.CanClose=`!_block_close` | Not completion; no weekly write found on this close route |
| Confirm | ResourceBasedCraftGUI.OnCraftButtonPressed calls PrayCraftGUI.OnPrayButtonPressed only if linked `pray_craft` is non-null | Bypasses ordinary BaseCraftGUI.OnCraft spending; preserve prayer reuse |
| Early native commit | OnPrayButtonPressed returns if `_block_close`; then SetParam(prayed_this_week,1), `_animating=false`, hides `_res_craft`, calls GS.RunFlowScript("pray",null) | Explicit service-start commitment, before launch outcome is known |
| Graph start | pray Start2 -> player/cinematic disable4 -> **second weekly write6=1** -> teleport8/direction10 -> first_sermon12 | Intercepting only the C# writer is insufficient |
| Tutorial | first_sermon>=1 ->clear16 ->talks18..24 ->faith26/28/30 ->bishop event32 ->End34 | Does not traverse the normal late finale |
| Normal calculation | first_sermon==0 ->InitChurchPray36 ->CalculatePrayEvent38 ->visitor creation48/50 | Reuse native PrayLogics calculation/result |
| Faith/effects | base spread67 and Drop Faith function135; PrayForBuff77; reactions ->bonus spread86/function99; ChurchPulpit.DoBuffSuccessAnimation invokes buff-flight and DropPrayItems | Rewards are staged, not atomic |
| Animation done | OnPulpitPrayAnimationState.OnStateExit(type0) ->ChurchPulpit.OnPrayAnimationDone ->PrayCraftGUI.OnFinishedPrayBuffAnimation ->stored callback ->77 Finished | Earlier than money and final cleanup |
| Donations allocated |77 Finished ->wait104 ->chance107 ->SpreadMoneyIncome109 ->Visitor Finale182 via116 | Allocation is not physical deposit |
| Donation deposit | npc_church_visitor9(out_and_donate) ->talk14 ->GoTo16 **Came to dest** ->DonateToBox18; amount from visitor `_money`24, box from46 | Flow_DonateToBox adds `_money` to `donat_box_inside`; movement completes independently |
| Normal sequence finale | pray112 ->camera114 ->visitor finale116 ->wait118(8s) ->camera121 On Finished ->player enable123 -> **CheckKeyQuest125(praying_finished)** ->AddInteractionEvent127(prayed) to donation box ->delayed bishop cleanup129 ->End131 | Strong late normal-path checkpoint, not a native exactly-once permit transaction or a join proving all visitor deposits |

Pinned OnPrayButtonPressed IL: flag write0013..0027; hide0033..003a; launch003f..0045. It discards launch result. `CustomFlowScript.Create(id)` can return null on missing graph; this body has no compensating flag rollback. No error was induced here.

`OnFinishedPrayBuffAnimation` logs Stats and invokes `_on_finished_animation`; it does not clear that callback. The middle callback is likewise retained. State-exit type1 invokes middle. Exact Animator asset multiplicity/interruption behavior remains unverified; a callback's name does not establish one invocation per service.

`CustomFlowScript.TerminateMe` pauses graph, clears it, destroys through base termination and invokes `_on_finished` if supplied. **GS.RunFlowScript("pray",null) supplies none.** Flow_EndScript calls termination; FlowScriptEngine.StopAllBehaviours simply stops its GraphOwners. Generic termination is not a successful-service receipt or a rollback promise on exit/load.

Pressed handler checks `_block_close`, but does not set it true, revalidate weekly access or install an operation latch. Its ordinary caller checks prayer selection and hides UI; this reduces ordinary repeat clicks but does not prove direct/reentrant idempotence. A future operation needs bounded identity/ownership if callbacks are used. No duplicate-service exploit was tested.

**Decision now adopted on main:** consume on valid confirmation that actually starts an extra service, not at normal125 or an animation callback; no automatic refund after interruption. Opening, selection and cancellation are free. Null launch, repeated invocation, scene/load changes and bishop transitions remain technical start/ownership questions, not invitations to choose another consumption policy.

## 5. Per-save state capability

**Status: current IL corroboration + reused native serializer fixtures/previous bounded installed evidence.**

WorldGameObject.data is an Item; Item._params is GameRes. Item.SetParam(string,float) delegates to GameRes.Set. Beyond special hp/progress/money/durability fields, GameRes keeps string/float lists `_res_type/_res_v`. A namespaced numeric key does not require a new item definition or manual save-file edits.

GameSave.PrepareForSave assigns live player.data to private serialized **GameSave._inventory** (IL0013..0023). GetSavedPlayerInventory returns it; the previously established startup path restores it via WorldGameObject.RestoreSavedInventory. Shared actual native binary fixtures preserve namespaced metadata; zero values can be removed by Item.RemoveZeroParams ->GameRes.RemoveZeroValues, so absent=zero must be deliberate. These are host capabilities, not a tested Extra Sermons schema.

Use the shared current-save ownership boundary: old scene/player references may survive during A/B/A loading. At the verified startup boundary live player `_data` must belong to current GameSave `_inventory`, with consumer-specific guards. A process-global count or merely non-null player is insufficient.

The adopted non-expiring single authorization can use a numeric0/1 carrier in that root. Exact key/schema, invalid-state handling, cleanup and any temporary in-flight operation remain unimplemented. One flag must not simultaneously mean unused permit, native weekly availability and in-flight operation. Keep native weekly availability canonical; do not persist a duplicate mirror without need. No sidecar or manual save rewrite is justified here. Ordinary saving owns disk persistence, which does not promise crash-safe disk atomicity. No user save or new serializer fixture was touched.

## 6. Technical approach comparison — no production selection

| Family | Supported capability | Remaining cost / limitation |
| --- | --- | --- |
| Native Royal answer + numeric reward | Stock price/refusal/debit/GameRes grant | Scoped insertion or callback wrapper; fresh-funds/receipt/rollback, icon/localization, repeat/Back proof |
| Physical permit item in native shop | Existing permission purchase shows item reward/drop delivery | Excluded from the adopted first version; retained only as historical comparison |
| Preserve native gate, scope one extra-service session | Exact reader and both start writers are known; native pray graph can run rewards | Current-save/graph/session binding, once-only reservation and preservation through calendar transitions |
| Separate extra-entry route opening native picker | Reuses PrayCraftGUI and pray graph | Must preserve progression/overhead handling and both weekly writes; opening UI alone solves neither |
| Broad weekly reset / whole graph replacement | Can lift a restriction | Loses calendar/ordinary-entitlement semantics or duplicates host logic; unjustified for the adopted scope |

First and third/fourth rows are composable parts, not complete competing implementations. Sections9–14 recommend a bounded integration candidate; it is not an implemented or accepted mechanism.

“Save old weekly flag and restore it later” is **not proved sufficient**: bishop arrival/departure while UI/service is active can make the snapshot stale. Restoring0 after departure reopens access. Main now adopts native-free-first priority: re-evaluate it at confirmation rather than spending a permit based on an old UI mode. Genuine intervening native writes must survive.

## 7. Independent production evidence gates

READY would mean technical readiness for production mutation, not runtime acceptance. The task separately prohibits production. No independent change reaches READY.

| Observable change | Known owner / final writer | Blast radius and preserved invariants | Missing evidence / status |
| --- | --- | --- | --- |
| Buy one authorization for 100 money in Royal Services | graph2/7/13, AnswerData/SmartRes; OnChosen ->RemoveSmartRes ->ReceiveSmartRes ->ID callback | Shared answers serve all NPCs; preserve parent access, other choices, Back, prices and localization | Isolated insertion, exact state identity, fresh-funds/failure/receipt and repeat/Back proof missing; non-item presentation adopted. **BLOCKED** |
| Permit opens extra service, keeps ordinary service | entry28/30/32/38; Flow_PlayerFlag; pressed writer, pray6, bishop15/441/story writers | Inactive pulpit, cleric/lock gates, overhead, tutorial and calendar refresh | Session/projection and two-writer/calendar preservation proof missing; free-first rule adopted. **BLOCKED** |
| Exactly-once consumption at valid actual start | confirmation, normal125/131 identified; animation77 Finished earlier; deposit has separate owner | Stock prayer reuse, failed-service rewards/effects, cinematic/input cleanup | Selected valid-start/launch receipt, replay/null-launch and interrupted-operation proof absent. **BLOCKED** |
| Persist one unused authorization | Item/GameRes ->GameSave._inventory ->native serialization ->restored root | Per-save isolation; unrelated money/metadata; no old-scene writes | Exact schema/validation/cleanup and consumer A/A/B/A/full-restart evidence missing. Native carrier established; product policy adopted. **BLOCKED** |

Cheapest representative future checks, **only after separate authorization**: one purchase/Back/insufficient-funds sequence with native money/state before/after; picker cancel versus actual service; ordinary-before-extra and extra-before-ordinary if supported; relevant bishop boundary; repeated start/callback refusal; save/root isolation if persistence is adopted. Select checks for the eventual mechanism, not a universal matrix or automatic diagnostic DLL.

## 8. Remaining blockers and next minimum step

**Closed at static-owner level:** installed 1.407 identity; Royal object/graph/access/menu; native price/visual check and debit/grant order; money unit; active/inactive pulpit gates; located weekly writers/reset/schedule; selection/cancel/start/normal finale; staged reward/donation owner; native per-save numeric carrier.

**Open:** final progression/text/art, selected isolated purchase and contextual entry/state-preservation mechanisms, exact valid-start/failure receipt, consumer schema/cleanup and runtime acceptance. Lifetime, one-unused cap, persistence, free-first priority and no automatic refund after a valid start are adopted. `praying_finished` is not atomic all-rewards delivery and is not the selected consumption point.

**Next:** use the bounded UI/compatibility findings and check plan in sections9–14. Do not repeat generic purchase/calendar/reward research or automatically build a diagnostic plugin.

## 9. Pulpit UI continuation: exact sources and evidence boundary

**Task boundary:** static inspection only, specifically contextual native UI entry, independent document indicator, and the two accepted PrayerClarity editions. No production/source changes were made to PrayerClarity, the game or shared research. The reference checkout and new inspection outputs are ignored research inputs under `work/`.

Installed primary hashes/MVID from section1 were rechecked and still match. New scene input **`Graveyard Keeper_Data/level2` SHA-256 `1f75a0328dc4987f3020688a0feb0d310c2bf3f6e288c6e4a64ad09db1f39041`** was parsed read-only with the existing UnityPy/Managed type-tree tooling. Only the `UI Root/Pray GUI` subtree and targeted method bodies were examined; serialized positions are not screenshots or resolved live geometry.

Both remote immutable target refs resolve to **`13c85c824bd2932b1175200368d212e03bbae79f`**:

- Vanilla: `accepted/vanilla-1.0.57`, tag `v1.0.57`; `src/PrayerClarityPlugin.cs` declares version1.0.57 and GUID `nikich.graveyardkeeper.prayerclarity`.
- Rebalanced: `accepted/rebalanced-0.2.51`, tag `rebalanced-v0.2.51`; `src/PrayerClarityRebalancedPlugin.cs` declares version0.2.51 and GUID `nikich.graveyardkeeper.prayerclarity.rebalanced`.

This common source revision contains both separately compiled editions, not a requirement to load them together. Their csproj Compile lists include the same PulpitPresentation/PulpitLayoutV5/PulpitPolish/PulpitTuning/PrayerForecast/PresentationText sources. The snapshot AGENTS still names older stable versions; exact accepted refs, tags and version constants above establish the versions inspected here. No new binary identity/compatibility acceptance is inferred from source identity.

Pinned source entry points:

- [PrayerClarityPlugin.cs](https://github.com/NikichMods/PrayerClarity/blob/13c85c824bd2932b1175200368d212e03bbae79f/src/PrayerClarityPlugin.cs), [PrayerClarityRebalancedPlugin.cs](https://github.com/NikichMods/PrayerClarity/blob/13c85c824bd2932b1175200368d212e03bbae79f/src/PrayerClarityRebalancedPlugin.cs).
- [PulpitPresentation.cs](https://github.com/NikichMods/PrayerClarity/blob/13c85c824bd2932b1175200368d212e03bbae79f/src/PulpitPresentation.cs), [PulpitLayoutV5.cs](https://github.com/NikichMods/PrayerClarity/blob/13c85c824bd2932b1175200368d212e03bbae79f/src/PulpitLayoutV5.cs), [PulpitPolish.cs](https://github.com/NikichMods/PrayerClarity/blob/13c85c824bd2932b1175200368d212e03bbae79f/src/PulpitPolish.cs).
- Same revision: `PulpitTuning.cs`, `ReflectionBridge.cs`, `PrayerForecast.cs`, `PresentationText.cs`, `RebalancedTierState.cs`, `RebalancedSoulsRepose.cs`, and both csproj Compile lists.
- Reused accepted host anchor evidence: shared `UI_INPUT_TIME_AND_ENVIRONMENT.md`, **Pray GUI craft-button anchor ownership**; PrayerClarity Geometry Probe0.1.1 source `d33c141f132b430d79140b96c683886851b9cc0a`, observed on Rebalanced0.2.35. Current accepted shared font evidence covers native `GJL.EnsureLabelHasCorrectFont` on mod-owned pulpit labels. Neither is a test of the proposed Extra Sermons indicator.

## 10. Installed native UI owner and lifecycle map

**Static facts from scene + IL:**

| Owner | Concrete scene identity / native behavior |
| --- | --- |
| Existing window instance | Scene GameObject1389 `UI Root/Pray GUI`, serialized inactive; ResourceBasedCraftGUI23820 and PrayCraftGUI23821 share that GameObject; UIPanel37601 has clipping=None, depth2 |
| Initialization | GUIElements.Init calls its referenced PrayCraftGUI.Init (IL00b8..00bd). PrayCraftGUI.Init gets the colocated ResourceBasedCraftGUI, initializes it and deactivates its GameObject. This is not creation of a new window for each prayer selection |
| Window rectangle | `window`: Transform10802, UIWidget23494, centered274x241, no anchor targets. `container`: Transform10506, UIWidget23175,274x199, four-side anchors to window (+3/+3 horizontal, +7 bottom/-35 top) |
| Action reference | PrayCraftGUI.l_button -> UILabel23094, `window/craft button` Transform10476. Width88,height16; horizontal anchors target window center, absolute-44/+44; vertical anchors target window bottom,+28/+44; updateAnchors=OnUpdate |
| Action visual/input | `craft button back` Transform10477/UI2DSprite32808 anchors to label, stock94x24. It also owns UIButton, UIEventTrigger and auto-sized PolygonCollider2D. Its gamepad-frame sibling under craft button is UI2DSprite32598, stock104x34, anchored around the background |
| PrayerClarity template reference | l_total_values -> UILabel28973, `window/container/total values`, initial242x68. No anchoring in this label itself; not a free area owned by Extra Sermons |
| Selector / prompt | main_ingredient ->BaseItemCellGUI23990 under `window/container/table`; l_pick_res ->UILabel22621 `txt_pick_a_res`. Native ingredients/table have cell creation/destruction/reposition paths; do not add a fake second ingredient |
| Button tips | `window/buttons tips`, UILabel24413,260x16, has native platform visibility and ButtonTipsStr behavior. Keep input tips and focus framing independent |

**Open/update/close sequence:**

- GUIElements.OpenCraftGUI(pulpit) retains the busy/progression path from section3 and selects PrayCraftGUI.Open for object group `pulpit`.
- PrayCraftGUI.Open first calls ResourceBasedCraftGUI.Open(wgo,PrayCraft), then clears current prayer/animation presentation, captures church quality and invokes RedrawTextValues(0,0). ResourceBasedCraftGUI.Open has an already-shown/active guard; a void Open call is not an open-success receipt. Avoid re-entering it and resetting an existing selection.
- BaseCraftGUI.CommonOpen and BaseGUI.Open own the actual shown instance/input stack. BaseGUI.Open sets `_is_shown`, activates the GO, updates localization/pixel scaling/input source and opened_windows, then emits `on_window_opened`. That event can occur **before the outer PrayCraftGUI.Open has completed all resets**; use it for identity/visibility observation, not assuming final selected content.
- ResourceBasedCraftGUI.Open configures the native picker/cells, clears picker result delegate, sets state0 and redraws; a stored last craft can be restored only when the object allows it. Native PrayCraftGUI.Open still resets its own `pray_craft`; do not promise selected prayer persistence across opens without an actual test.
- Direct PrayCraftGUI.RedrawTextValues callers found in Assembly-CSharp are Open and OnResourcePickerClosed. The latter updates the selected linked craft, redraws values and then assigns localized `l_button` text (`btn_pray`/`btn_try_pray`). A postfix on RedrawTextValues is **not** the final writer of the action label text on this selection path.
- ResourceBasedCraftGUI.Redraw owns table/cell rendering, callback binding and inactive-cover state; its Update repositions its craft-list table and can dispatch a keyboard craft action, but returns immediately if that table is null. Do not use this broad Update as an assumed per-frame indicator refresh owner.
- Selecting through ResourceBasedCraftGUI calls PrayCraftGUI.OnResourcePickerClosed; cancellation does not consume anything. BaseGUI.OnHiddenByAnotherGUI disables controller navigation, not the whole underlying GO; a picker can cover a still-active pulpit. `BaseGUI.is_shown_and_top` checks `_is_shown` and last opened_windows entry. Observe native window-open/close/top state so a noninteractive indicator hides under the picker and returns even on cancel without a PrayCraftGUI redraw.
- Actual closure is ResourceBasedCraftGUI.Hide -> CanClose -> BaseGUI.Hide, which synchronously deactivates the shared GO, updates shown/stack/navigation and emits on_window_closed. A blocked Hide attempt must not clear a still-open session. Closing for sermon start is not the same as cancel: keep any start token separately from the visible indicator lifetime.
- BaseGUI.RecalcAllAnchors/DoRecalcAllAnchors/LateUpdate and native NGUI update own later anchor refresh. Own children should follow native parent destruction and be recreated only on a genuinely new UI instance; cancellation/hide must not accumulate objects/callbacks or retain another save's mode.

## 11. PrayerClarity ownership and final writers

**Exact accepted-source facts, common to both editions unless noted:**

1. Each plugin patches `PrayCraftGUI.RedrawTextValues(float,float)` with a postfix. `R.Patch` delegates to ReflectionBridge.PatchHooks; the inspected helper sets no explicit Harmony priority/before/after fields. An Extra Sermons “last postfix” alone does not establish final geometry.
2. That postfix reads `l_total_values`, builds PrayerForecast, then calls **PulpitPresentation.Render ->PulpitLayoutV4.Apply ->PulpitPolish.Apply**. The file `PulpitLayoutV5.cs` deliberately still declares class **PulpitLayoutV4**. Null forecast and errors run Polish.Restore, Layout.Restore and Presentation.Hide. Initial no-prayer and selected-prayer layouts therefore differ.
3. Presentation captures/restyles/rewrites the native total-values template; creates sibling root `PrayerClarity.PulpitForecast` under its container with Result, Effect, Effect.Icon and DependencyNote; moves native selector/prompt and writes an attempted prayer-button transform position. It refreshes owned label fonts on redraw. These are not Extra Sermons content slots.
4. Layout captures original root/container/back/header/tips/close-button state. It grows the **unanchored root UIWidget with SetDimensions**, then updates the anchored container; it does not make the container's width/height setters the owner. Tuning adds10 width and100 height plus content clearance. It adds ResultHeader, lays out forecast rows and restores captured geometry on the no-forecast/error path.
5. Polish replaces result text with PresentationText.BuildPulpitResultRows, suppresses DependencyNote, positions effects below **processed** result height and schedules a generation-guarded coroutine. After **yield return null**, it measures settled effect bounds and the union of **all active UIWidgets in the craft-button subtree**, then asks Layout.GrowWindowForButtonClearance to grow the root by roughly twice the missing downward clearance. Every redraw resets dynamic growth to baseline before recomputation; Restore invalidates pending generations.
6. The action UILabel is bottom-anchored OnUpdate. Thus Presentation's transform move is not final ownership; root growth/restore and native anchors move it later. A one-frame position copy, hard-coded y, or Extra Sermons competing root resize can be overwritten or feed repeated geometry changes.
7. **Specific collision hazard:** an indicator nested under `craft button` is included in Polish.TryGetVisibleButtonBounds. Extending its vertical bounds can change window growth, even if it has no click handler. A sibling under `window` avoids this specific descendant scan. Likewise placing it in the ingredient table exposes it to native table/cell management, and placing it under PrayerClarity.PulpitForecast gives another mod hide/destruction ownership.
8. Preserve the accepted reveal model: PresentationText shows dependencies/current success chance/prayer-owned contributions instead of exposing exact resolved future totals. Extra Sermons must not append a resource forecast, overwrite l_total_values, replace Effect/Result text, or reuse PrayerForecast as a permit-state owner.
9. Rebalanced also patches **PrayLogics.CalculatePray** for Souls conversion and **PlayerComponent.StartPrayAnimation** for tier capture. It uses selected `GUIElements.pray_craft.pray_craft` and native animation state. Therefore reuse the actual selected native CraftDefinition and normal pray/calculation/animation path; invoking a separate unpatched calculator, cloning a dummy prayer or synthesizing rewards would bypass edition behavior. Source search found no PrayerClarity ownership of `prayed_this_week` or the proposed permit key. That is evidence of no direct conflict in these inspected sources, not a live compatibility guarantee.

## 12. Recommended indicator candidate and alternative

**Recommendation (inference from the owner map; not implemented or visually accepted):** own a small noninteractive `ExtraSermons` root as a **direct child of the native window**, with a document sprite and own count/status UILabel. Anchor its own rectangle to the actual action-background/label edges in NGUI coordinates; keep it outside the button, ingredient table and PrayerClarity forecast subtrees. Read window/action bounds through widget corners transformed into the window's space, not Screen pixels or stored coordinates. Do not mutate the native action text, parent dimensions/anchors, selector, tips, collider or navigation lists.

Place the compact badge in a measured side lane at the same vertical band as the visible button. In the stock serialized model the centered274-wide window and104-wide focus frame leave85units per side **before padding**; this is evidence of a plausible candidate, not a promised live width. Use actual active button/frame bounds, window interior margin, measured localized badge width and a small gap. Keep badge height inside the existing action-band height; reject overlap/viewport failure rather than shrinking text unreadably or moving native content. Dynamic PrayerClarity window growth then moves the anchor target, while own OnUpdate anchors follow without joining its clearance scan.

No exact art/copy/offset is adopted. A document+1/1 must also make additional-service mode understandable without requiring mouse hover; the final short localized status and controller readability need user review. Native font/layer/scale/style can be read from the actual current window, but copy only presentation properties into **new own components**; do not clone a whole interactive action/cell with callbacks, colliders or navigation. Rebind the label with the native current-language font owner on redraw/language change. Existing game art may be resolved at runtime after its identity/meaning is reviewed; this investigation does not claim a verified permit sprite.

Lifecycle: bind by current PrayCraftGUI/ResourceBasedCraftGUI identity; create at most one indicator per instance. Observe completed Open/selection and actual Hide plus top-window events, and native layout/language changes. Show it in active additional-service mode; ordinary free-first mode leaves the ordinary path uncluttered. Do not debit on visibility changes. Cancel invalidates UI work, a start transfers operation ownership before Hide, new save/UI invalidates references. A cancellable one-shot settled refresh may measure fit, but **one frame is not a universal settling guarantee** because PrayerClarity also schedules a next-frame pass. Use native anchor dependencies and change-triggered validation; no global per-frame scan, reflection or arbitrary sleep/retry loop.

**Alternative if the side lane fails fit/accessibility:** an independently owned compact requirement strip anchored adjacent to/below the native window/action, still outside the craft-button subtree and without resizing the shared root. It must be clamped/tested against the viewport and native button tips; it is a placement alternative, not another tab, picker, item slot or altar. Reopen the visual choice if that strip cannot remain visible at supported scale. If neither can satisfy fit, a deliberately coordinated single layout owner is required; two independent mods incrementing/restoring the same window height is not an acceptable fallback. No PrayerClarity API for reserving a new row was found; calling its internal layout classes is not a stable extension contract.

Static proof covers component ownership and the avoided scan, not rendered clearance, icon legibility, panel depth ordering, exact anchor-settle order or controller behavior. Optional Harmony ordering/soft dependency on the known GUIDs can assist a later implementation but cannot replace those checks.

## 13. Contextual entry, ordinary entitlement and bishop-absent execution

Reuse section3's exact native entry: retain overhead handling, native lock28 and cleric32; authorize only the weekly-unavailable branch30 for an owned permit, then continue to native38/OpenCraftGUI. Normal flag0 delegates unchanged and retains the permit. A broad prefix on WorldGameObject.Interact or unconditional direct PrayCraftGUI.Open would bypass other gates and is not recommended.

**Narrow candidate:** adapt the exact live entry graph's weekly gate/continuation using verified node type, parameter, graph/current pulpit and neighbor structure. Runtime Node.ID must **not** be assumed equal to JSON `$id`30. Scope any intervention to the actual current-save UI operation, not every Flow_PlayerFlag or every read of the weekly flag. An extra-only contextual refusal can be supplied at the same scoped unavailable branch; it must not replace progression refusals. Concrete binding remains BLOCKED.

Installed firstpass narrows the integration hazards: GraphOwner.GetInstance clones/caches a graph per owner, and StartBehaviour obtains that instance before StartGraph. FlowNode.GatherPorts registers ports and deserializes inputs; BindPorts binds connections. **FlowOutput.BindTo(FlowInput) copies the input's current delegate**, and FlowOutput.Append combines delegates without suppressing the original. Therefore replacing FlowInput.pointer after binding is not enough; appending an extra “open” handler can execute both refusal and opening. A selected pre-bind wrapper or exact existing-bound-delegate adapter needs lifecycle/instance isolation proof and restoration; never mutate the Resources template or assume a public pointer setter solves execution routing.

At confirmation, re-evaluate native locks, selected valid prayer, current save, native weekly availability and permit. A bishop arrival while selection is open can make the free sermon available: native-free-first must win then, with the permit retained and UI mode refreshed. No fixed “extra because the UI opened that way” decision or cached weekly snapshot is sufficient.

For a genuinely extra start, preserve native weekly state by avoiding **only the two extra-operation start writes** (OnPrayButtonPressed's particular SetParam call and live pray node6), while allowing real bishop/story/calendar writers to run. This is a recommended semantic boundary, **not a proved patch**. It preserves a later native reset instead of restoring a stale snapshot. Native extra start usually begins with flag1, so same-value writes are harmless in that simple case, but timing between confirmation/graph start and bishop reset has not been proved atomic. A global SetParam hook with guessed call stacks or global flag reset is unjustified. A narrowly scoped callsite/graph-input projection must be reviewed against the host-native-first gate before implementation.

Keep the selected CraftDefinition, GUIElements singleton, standard GS/CustomFlowScript launch, native CalculatePray and animation callbacks intact so both PrayerClarity editions retain their patches and reward semantics. A valid actual-start receipt and exactly-once reservation/commit have not yet been established; a void UI callback or mere successful resource lookup is not enough. Stop at this gap rather than equating prefab existence with a started service.

### Concrete off-day dependency discovered

The normal pray branch uses the bishop at late **229 ->129**: FindWGO(custom_tag=npc_bishop,ignore_error=false) feeds delayed `TryRemoveStackedChurchVisitors` event, then Out continues to End131. Bishop graph **2161 ->2166** handles only that event by calling WorldMap.TryRemoveStackedChurchVisitors.

Installed IL: Flow_FindWGO uses WorldMap.GetWorldGameObjectByCustomTag, which logs an error on a missing target unless ignored. Flow_FireEvent resolves WGOParamOrSelf; a null resolved WGO logs an error, calls its **Out** and returns. GS launches pray under a global-scripts GO (created without WGO by the native factory); there is no established bishop fallback. Therefore **if lookup is null, this late path continues to End131 but does not schedule cleanup**. Actual bishop lookup results on an off-day save were not observed; do not claim a tested failure or universal absence.

The native cleanup is not just a harmless notification: WorldMap.TryRemoveStackedChurchVisitors destroys stationary npc_church_visitor objects in the church's hard-coded area. Calling it earlier or once per new service could erase visitors still donating. Repeated immediate paid services also need isolation from a previous service's delayed cleanup. No live viability claim follows from the fact that player enable123 and rewards precede129.

A bounded extra-session-only replacement of this missing-target scheduling with the **same native cleanup helper/timing**, if needed, is a candidate for a separate evidence gate. Prove timing and consecutive-service ownership before changing it; do not spawn a dummy bishop, alter NPC schedules, duplicate payout code or globally suppress errors. Tutorial first_sermon>=1 uses a different bishop event; retain the progression/tutorial boundary instead of treating this as a supported extra-service path.

## 14. Current readiness and minimum verification plan

All work below remains research-only. Native scene identity, relevant UI ownership, exact PrayerClarity sources and the identified static hazards are established; this does not make a production intervention READY.

| Independent change | Known owner / final consumer | Invariants / missing evidence | Gate |
| --- | --- | --- | --- |
| Own document indicator | scene window/action widgets; native anchors; PrayerClarity delayed root-height writer | No PC/native text or geometry writes, no descendant-scan participation; exact art/copy, bounded fit and top/hide/language/input lifecycle not rendered/tested | **BLOCKED** |
| Permit-only contextual opening/refusal | live entry weekly gate between native locks and OpenCraftGUI; bound Flow handlers | Exact live/pre-bind isolation and no duplicate old/new route, no template changes; preserve busy/cleric/lock/overhead and native-free-first | **BLOCKED** |
| Actual extra start/weekly preservation | pressed callsite + native pray6; independent bishop writers | Guard re-entry, valid prayer/current save, actual graph start/failure receipt; no snapshot restoration; prove boundary during selection/start | **BLOCKED** |
| Off-day completion / repeat service | pray229/129, bishop2161/2166, native cleanup helper/timer | Determine actual missing-target behavior; any adapter must preserve deposit and next-service visitors | **BLOCKED** |
| Compatibility with each edition | shared redraw/layout/polish, Rebalanced calculator/tier hooks | No bypass of patched native paths or accepted reveal semantics; source map only, no combined-game acceptance | **BLOCKED** |

**Minimum plan, requiring separate permission to implement/test:** use three isolated configurations: stock game+Extra Sermons candidate, Vanilla1.0.57+candidate, Rebalanced0.2.51+candidate. Only one PrayerClarity edition at a time. Identify exact candidate/installed files, supported host and session log; preserve current installation before any authorized swaps. Do not install anything as part of this research.

For **each** configuration:

1. Ordinary service available with one permit: open, select/cancel/reopen and confirm the ordinary service. Original UI/navigation and edition information remain intact; permit remains1. Check duplicate-open/input refusal in the same sequence. Prove the full inactive-church/preaching-lock/cleric/busy guard set once on the stock configuration plus deterministic graph guards; repeat a representative locked entry under the editions, not the full progression matrix without a new dependency.
2. Normal service unavailable with permit, both after that ordinary service and on a bishop-absent day: same pulpit/native picker opens, mode/document status is understandable; cancel and replace prayer without debit; valid actual start consumes exactly once; repeat without permit gives the contextual refusal. Exercise one failed prayer as a real service, not a cancelled start.
3. UI state coverage can be combined with the above: empty/selected/reselected/cancelled picker, ordinary/extra mode, repeated opens, mouse then controller, switch RU/EN and one CJK/long-text locale, normal/small supported UI viewport. Observe the first frame and the delayed PrayerClarity clearance pass. Check badge bounds, action/frame hit area, tips and top-window hiding; no accumulated children, drift, wrong-save status or changed forecast text. Use a long effect such as Prosperity and a multirow/BSS prayer to exercise dynamic height; do not replay every prayer formula.
4. Bishop arrives while extra selection is open: confirmation uses newly available free service and keeps the permit. Bishop changes during actual extra start/service: genuine native weekly refresh survives. Prove save/reload and A/B/A ownership once with stock+candidate, then a short reopen/cleanup check under each edition unless a newly observed interaction warrants repeating the full sequence. Closing/loading must invalidate old UI/session references.
5. Extra service with no live bishop: observe normal start, control/camera restoration, deposits and absence of an unhandled missing-target/cleanup failure. Start the next permitted service promptly enough to overlap the previous delayed-cleanup window; ensure previous cleanup cannot remove its visitors. Test only the eventual minimal cleanup adapter if the actual native path proves it needed.

Additional **Rebalanced-only representative check:** one Souls conversion prayer with observable gratitude change and one specialist tier/buff capture through the normal animation; compare with that edition's established semantics. Do not infer compatibility from Vanilla's result or replace PrayerClarity calculations with a test oracle that bypasses its hooks.

Lower-cost static/deterministic checks should first prove marker visibility/state transitions, no debit on cancel, free-first reclassification, once-only start/failure containment, exact graph matching and delegate binding order. No synthetic test is a substitute for final NGUI rendering, actual bishop absence or combined installed hooks. A diagnostic DLL is not automatically justified; prefer the ordinary candidate, sparse native/component logs and direct observation once authorized.

**Next bounded step:** review indicator copy/art/fit and the proposed live graph/session adapter, then close valid-start and cleanup boundaries with the cheapest sufficient evidence. This research has stopped at the identified runtime/integration gaps; no additional broad Royal Services/calendar/reward audit is required.
