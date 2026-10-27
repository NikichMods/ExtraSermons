# Extra Sermons — Product Design

## Adopted concept — 2026-10-26

The player wants an additional opportunity to hold a sermon without waiting for the next normal church day. Extra Sermons provides a paid, in-world route through the existing **Royal Services** box by the church.

The user adopted:

- Product name: **Extra Sermons**; repository/project identifier: `ExtraSermons`.
- Add an option in Royal Services to buy permission for **one additional sermon**.
- Initial price: **one gold coin per permit**. This is an adopted starting value for exploration, not verified final balance.
- The purchase grants one extra service opportunity, rather than a day of unlimited services.

Preserve the ordinary weekly service and the native prayer/reward behavior unless a necessary integration change is explicitly agreed. The purchase itself is the proposed additional cost; consuming or downgrading the prayer is not part of the concept.

## Open product details

Resolve the following before committing to their implementation:

- Whether the permit is used immediately, remains available until a later sermon, or expires.
- Whether multiple unused permits can be bought and accumulated.
- How the extra opportunity behaves on the regular sermon day, including after the ordinary service.
- The exact point at which permission is consumed: opening the prayer picker must not be assumed equivalent to completing a service.
- Purchase availability/progression requirements and the final localized menu text.
- Save/load and removal behavior for unused permission.

Technical research should establish native Royal Services menu/payment ownership, sermon availability and completion state, cancellation behavior, and safe per-save storage options if persistence is needed. Findings may inform these choices but do not adopt new product semantics by themselves.

## Current evidence and next milestone

The concept and initial public-mod comparison are established. This research branch's [SERMON_FLOW_RESEARCH.md](SERMON_FLOW_RESEARCH.md) records installed 1.407 static Royal Services/payment, pulpit/calendar, service lifecycle and native per-save carrier evidence. This is not implementation, supported-build/runtime compatibility acceptance, or adoption of the open permit rules above.

The next step is to choose permit consumption/interruption and ordinary-day semantics using that evidence, then close only the selected menu-insertion and state-preservation seam. Production implementation and game testing require their applicable gates and authorization.
