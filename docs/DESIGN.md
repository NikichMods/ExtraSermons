# Extra Sermons — Product Design

## Adopted concept — 2026-10-26

The player wants an additional opportunity to hold a sermon without waiting for the next normal church day. Extra Sermons provides a paid, in-world route through the existing **Royal Services** box by the church.

The user adopted:

- Product name: **Extra Sermons**; repository/project identifier: `ExtraSermons`.
- Add an option in Royal Services to buy permission for **one additional sermon**.
- Initial price: **one gold coin per permit**. This is an adopted starting value for exploration, not verified final balance.
- The purchase grants one extra service opportunity, rather than a day of unlimited services.

Preserve the ordinary weekly service and the native prayer/reward behavior unless a necessary integration change is explicitly agreed. The purchase itself is the proposed additional cost; consuming or downgrading the prayer is not part of the concept.

## Adopted first-version permit behavior — 2026-10-27

The user accepted this coherent default behavior after the bounded installed-game sermon/Royal Services research:

- An unused authorization stays available until used; it does **not** expire with the day or week.
- The player may hold **one unused authorization at a time**. A second cannot be purchased until the first has been used; buying another after use remains possible.
- The unused authorization belongs to the **current save** and survives ordinary save/load. Its visual representation (physical inventory paper versus non-item entitlement displayed as a document) is **not** chosen by this rule.
- If the normal free sermon is currently available, the first sermon uses the **native weekly opportunity** without spending the permit.
- Once the normal sermon is no longer available, a permit permits **one extra sermon**, including after the regular service on the bishop's day or on another day, subject to the church's normal unlock/progression locks. Off-day execution and ordinary-entitlement preservation are **design requirements pending proof**, not tested host capabilities.
- Opening/closing the pulpit GUI, selecting/changing a prayer, or canceling before the confirmed start does **not** consume the permit.
- The permit is used **when the selected sermon is validly confirmed and actually begins**, rather than after every reward/donation animation has finished. Once started, interruption does not automatically refund it. Ordinary save loading restores whatever authorization state was last saved; crash-safe disk transactionality is not promised.
- An extra sermon must not consume the normal weekly opportunity, add a second prayer cost, downgrade the selected prayer, duplicate prayer/reward calculations, or bypass ordinary church progression locks.

The implementation must establish a bounded, once-only start/reservation rule and preserve genuine intervening bishop/calendar changes. The native weekly `prayed_this_week` flag cannot be treated as a permit counter or blindly snapshot-restored.

## Adopted pulpit presentation — 2026-10-27

The user chose a **single contextual church pulpit** with the familiar native prayer-selection window and a clearly visible permit indicator. Favor vanilla-like, self-explaining interactions over manual insertion, extra tabs or a separate altar:

- The permit is a **non-inventory per-save authorization**, represented visibly as a royal paper/document icon and owned/required status (normally `1/1`) when relevant. It is not a transferable physical inventory item, does not need dragging into a second slot, and must not be duplicated as a second mutable source of truth.
- Royal Services offers the purchase; its confirmation makes acquisition apparent. While one unused permit exists, repeat purchase is unavailable with a clear explanation. Final wording/art remain subject to review.
- **Ordinary sermon available:** pulpit uses the original flow, selected prayer and native calculation. A held permit is not spent. Avoid adding routine navigation or clutter to the ordinary path.
- **Ordinary sermon unavailable, permit owned:** interacting with the **same pulpit** opens the familiar prayer-selection flow in clearly marked *additional sermon* mode, with one prayer selection and a compact permit/document requirement indication near the relevant action. Confirming valid start consumes the permit under the previously adopted rule. This off-day session and the preservation of native weekly state still require technical proof.
- **Ordinary sermon unavailable, no permit:** provide a short, native-style contextual refusal that tells the player the regular church-day rule and where to buy the extra authorization. A returning player should not need an external mod description.
- Do not add a mandatory altar interaction, a second prayer picker/tab, two manual input slots, or a physical paper inventory/storage search for this first version. These were considered as alternatives, not silent additional features.

**Compatibility is a required design and acceptance property, not a verified outcome.** Both public PrayerClarity editions (Vanilla 1.0.57 and Rebalanced 0.2.51) modify `PrayCraftGUI.RedrawTextValues`, including the `l_total_values` region and pulpit layout/height. Extra Sermons must avoid taking over PrayerClarity-owned text or fixed geometry and must preserve the clarity information/reward-reveal semantics. Investigate an independently owned, small indicator anchored to the actual prayer/action UI with correct redraw/hide/resize and controller behavior. Do not treat a sketch, fixed coordinates or Harmony ordering alone as compatibility proof. Test stock UI, PrayerClarity: Vanilla and PrayerClarity: Rebalanced as separate supported configurations before claiming compatibility; only one PrayerClarity edition is installed at a time. Extra Sermons must invoke existing sermon calculation/reward owners so Rebalanced mechanics are not inadvertently replaced.

The exact indicator shape, copy, art, position and optional visibility outside extra mode remain open to layout evidence and user visual acceptance. No custom UI code or runtime test exists yet.

## Open product and presentation details

- Final permit icon/art, compact indicator placement, native-style text and localization without confusing manual item-slot affordances.
- Proof of the selected context-sensitive pulpit opening/refusal path, including native church locks, bishop/calendar transitions and safe preservation of the ordinary weekly service.
- Installed UI coexistence with both PrayerClarity editions and vanilla: independent lifecycle/ownership, dynamic layout, repeated open/redraw/close, mouse/gamepad and representative text lengths.
- How to communicate purchase status, price and insufficient-funds/refusal conditions without creating an unnecessary separate UI flow.
- Precise purchase availability/progression gate, including opened church/pulpit and native Royal Services access. Do not make a permit purchasable when the player could unknowingly be prevented from using it.
- Technical item/state identity, current-save storage, invalid-state handling and removal/uninstall behavior.
- Session start/failure/interruption mechanism; sermon-without-bishop viability, native reward/cinematic cleanup and compatibility boundaries.
- Whether later versions should support permit stacking, trading/storage or other behavior. These are *not* first-version commitments.

The user may reconsider an accepted behavior explicitly after evaluating the presentation tradeoffs. A brainstormed physical-paper UI is not itself authorization to change the one-unused-permit/no-expiry rules.

## Current evidence and next milestone

The read-only installed-stock 1.407 investigation at [commit `360917f789e83ba8145a3e0d736118d39312f194`](https://github.com/NikichMods/ExtraSermons/commit/360917f789e83ba8145a3e0d736118d39312f194), [research branch `docs/SERMON_FLOW_RESEARCH.md`](https://github.com/NikichMods/ExtraSermons/blob/codex/royal-sermon-research/docs/SERMON_FLOW_RESEARCH.md), identifies Royal Services flow/payment, the native pulpit/week/bishop writers, early start and later staged rewards, and native per-save numeric state. The installed game inputs were fingerprinted; no Extra Sermons production code, build, runtime integration or compatibility test exists. That evidence supports designing a mechanism, **not** declaring implementation READY or runtime acceptance.

Next investigate **only** the selected contextual-pulpit entry, independent permit indicator compatible with the current PrayerClarity UI, and safe session/weekly-state preservation seams. Use existing PrayerClarity and shared host evidence before a local-only Codex assignment; apply the engineering readiness gate before any production change. Build, test and accept through the project's Git/CI/acceptance workflow; research-only results are not accepted runtime behavior.
