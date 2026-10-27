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

## Open product and presentation details

- **Physical permit versus visible non-item authorization.** The user proposed a tangible paper like the vanilla exhumation permission, with optional prayer-and-permit slot presentation. No physical item, icon, inventory rule or replacement entitlement representation is adopted yet.
- **Interaction surface:** pulpit with a second tab/choice, a separate church altar/shrine, or a smaller contextual pulpit option. Preserve a recognizable vanilla UX, discoverable instructions and the untouched normal sermon path. Actual world-object interactability and UI implementation/compatibility require evidence.
- How to communicate permit purchase location, owned/available status, price, native refusal conditions and a failed/insufficient-funds purchase through the UI; final localization and art.
- Precise purchase availability/progression gate, including opened church/pulpit and native Royal Services access. Do not make a permit purchasable when the player could unknowingly be prevented from using it.
- Technical item/state identity, current-save storage, invalid-state handling and removal/uninstall behavior.
- Session start/failure/interruption mechanism; sermon-without-bishop viability, native reward/cinematic cleanup and compatibility boundaries.
- Whether later versions should support permit stacking, trading/storage or other behavior. These are *not* first-version commitments.

The user may reconsider an accepted behavior explicitly after evaluating the presentation tradeoffs. A brainstormed physical-paper UI is not itself authorization to change the one-unused-permit/no-expiry rules.

## Current evidence and next milestone

The read-only installed-stock 1.407 investigation at [commit `360917f789e83ba8145a3e0d736118d39312f194`](https://github.com/NikichMods/ExtraSermons/commit/360917f789e83ba8145a3e0d736118d39312f194), [research branch `docs/SERMON_FLOW_RESEARCH.md`](https://github.com/NikichMods/ExtraSermons/blob/codex/royal-sermon-research/docs/SERMON_FLOW_RESEARCH.md), identifies Royal Services flow/payment, the native pulpit/week/bishop writers, early start and later staged rewards, and native per-save numeric state. The installed game inputs were fingerprinted; no Extra Sermons production code, build, runtime integration or compatibility test exists. That evidence supports designing a mechanism, **not** declaring implementation READY or runtime acceptance.

Next choose the player-facing permit representation and interaction route with the user. Then investigate **only** the selected menu/world-entry and safe session-preservation seams, apply the engineering readiness gate, and implement/test through the project's Git/CI/acceptance workflow. Research-only results remain distinct from accepted runtime behavior.
