# Extra Sermons — Royal Services and Sermon Flow Research

Research checkpoint: **2026-10-27**. **Static investigation complete within the scope below; production changes remain BLOCKED.** No game execution, installed probe, production code, build, game/config/plugin/save mutation or release was performed. Permit rules in `DESIGN.md` remain unresolved. Sections 1–8 below supersede the initial public-source checkpoint preserved at commit `b79e95b8973a2425b5f9d104c952b5094ebc61e6`.

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

`ReceiveSmartRes(GameRes)` adds its atom to player Item params. A native priced answer could grant a namespaced numeric authorization without a new physical item or duplicated currency formula. This is a **technical option**, not adoption of persistence/stacking.

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

**User decision remains open:** consumption at accepted start follows vanilla's early flag commitment; consumption at normal125 avoids confusing open UI with use but needs an in-flight reservation/reentrancy/interruption rule, since rewards started earlier. Null launch, repeated callbacks, scene/load interruption and bishop transitions are material cases. No reservation/refund policy is adopted.

## 5. Per-save state capability

**Status: current IL corroboration + reused native serializer fixtures/previous bounded installed evidence.**

WorldGameObject.data is an Item; Item._params is GameRes. Item.SetParam(string,float) delegates to GameRes.Set. Beyond special hp/progress/money/durability fields, GameRes keeps string/float lists `_res_type/_res_v`. A namespaced numeric key does not require a new item definition or manual save-file edits.

GameSave.PrepareForSave assigns live player.data to private serialized **GameSave._inventory** (IL0013..0023). GetSavedPlayerInventory returns it; the previously established startup path restores it via WorldGameObject.RestoreSavedInventory. Shared actual native binary fixtures preserve namespaced metadata; zero values can be removed by Item.RemoveZeroParams ->GameRes.RemoveZeroValues, so absent=zero must be deliberate. These are host capabilities, not a tested Extra Sermons schema.

Use the shared current-save ownership boundary: old scene/player references may survive during A/B/A loading. At the verified startup boundary live player `_data` must belong to current GameSave `_inventory`, with consumer-specific guards. A process-global count or merely non-null player is insufficient.

An unused authorization could technically use a boolean/count in that root. Expiry, accumulation, invalid-value bounds, schema/cleanup and interrupted-operation data depend on user rules. A single flag cannot independently represent unused permit, ordinary entitlement and extra operation in flight. Keep native weekly availability canonical; do not persist a duplicate mirror without need. No sidecar or manual save rewrite is justified here. Ordinary saving owns disk persistence, which does not promise crash-safe disk atomicity. No user save or new serializer fixture was touched.

## 6. Technical approach comparison — no production selection

| Family | Supported capability | Remaining cost / limitation |
| --- | --- | --- |
| Native Royal answer + numeric reward | Stock price/refusal/debit/GameRes grant | Scoped insertion or callback wrapper; fresh-funds/receipt/rollback, icon/localization, repeat/Back proof |
| Physical permit item in native shop | Existing permission purchase shows item reward/drop delivery | New definition and inventory/pickup/loss semantics; different UX, not adopted |
| Preserve native gate, scope one extra-service session | Exact reader and both start writers are known; native pray graph can run rewards | Current-save/graph/session binding, once-only reservation and preservation through calendar transitions |
| Separate extra-entry route opening native picker | Reuses PrayCraftGUI and pray graph | Must preserve progression/overhead handling and both weekly writes; opening UI alone solves neither |
| Broad weekly reset / whole graph replacement | Can lift a restriction | Loses calendar/ordinary-entitlement semantics or duplicates host logic; unjustified for the adopted scope |

First and third/fourth rows are composable parts, not complete competing implementations. Evidence favors preserving native currency/reward owners with narrow authorization input, but no mechanism is yet selected.

“Save old weekly flag and restore it later” is **not proved sufficient**: bishop arrival/departure while UI/service is active can make the snapshot stale. Restoring 0 after departure reopens access; leaving 1 after an extra started with native 0 can consume the normal opportunity. Ordinary-versus-extra priority remains a user decision, and genuine intervening native writes must survive.

## 7. Independent production evidence gates

READY would mean technical readiness for production mutation, not runtime acceptance. The task separately prohibits production. No independent change reaches READY.

| Observable change | Known owner / final writer | Blast radius and preserved invariants | Missing evidence / status |
| --- | --- | --- | --- |
| Buy one authorization for 100 money in Royal Services | graph2/7/13, AnswerData/SmartRes; OnChosen ->RemoveSmartRes ->ReceiveSmartRes ->ID callback | Shared answers serve all NPCs; preserve parent access, other choices, Back, prices and localization | Chosen isolated insertion/representation, fresh-funds/failure/receipt and repeat/Back proof missing. **BLOCKED** |
| Permit opens extra service, keeps ordinary service | entry28/30/32/38; Flow_PlayerFlag; pressed writer, pray6, bishop15/441/story writers | Inactive pulpit, cleric/lock gates, overhead, tutorial and calendar refresh | Session/projection and two-writer/calendar preservation proof missing; ordinary-day priority unresolved. **BLOCKED** |
| Exactly-once consumption | confirmation, normal125/131 identified; animation77 Finished earlier; deposit has separate owner | Stock prayer reuse, failed-service rewards/effects, cinematic/input cleanup | Start/finale/interruption policy and replay/null-launch proof absent. **BLOCKED** |
| Persist unused permission if adopted | Item/GameRes ->GameSave._inventory ->native serialization ->restored root | Per-save isolation; unrelated money/metadata; no old-scene writes | Persistence/expiry/stacking not adopted; Extra Sermons schema/validation/cleanup and A/A/B/A/full-restart evidence missing. Carrier itself established. **BLOCKED** |

Cheapest representative future checks, **only after separate authorization**: one purchase/Back/insufficient-funds sequence with native money/state before/after; picker cancel versus actual service; ordinary-before-extra and extra-before-ordinary if supported; relevant bishop boundary; repeated start/callback refusal; save/root isolation if persistence is adopted. Select checks for the eventual mechanism, not a universal matrix or automatic diagnostic DLL.

## 8. Remaining blockers and next minimum step

**Closed at static-owner level:** installed 1.407 identity; Royal object/graph/access/menu; native price/visual check and debit/grant order; money unit; active/inactive pulpit gates; located weekly writers/reset/schedule; selection/cancel/start/normal finale; staged reward/donation owner; native per-save numeric carrier.

**Open:** permit lifetime, accumulation/persistence, regular-day priority, consumption/reservation/refund boundary, final progression/text; selected isolated insertion/state-preservation mechanism; Animator interruption/multiplicity if chosen; mod interactions and Extra Sermons runtime/failure acceptance. `praying_finished` is not atomic all-rewards delivery.

**Next:** choose user-owned consumption/interruption and unused-normal-service rules using this map. They determine whether one numeric carrier suffices or an in-flight operation is needed. Then inspect only the selected menu-insertion/session seam and design its narrow negative-path proof. No repeated generic formula/calendar investigation, automatic installed probe, game launch or implementation is justified by this checkpoint alone.
