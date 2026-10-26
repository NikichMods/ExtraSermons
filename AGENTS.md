# Extra Sermons — Working Contract

This repository follows [NikichMods/DevRules](https://github.com/NikichMods/DevRules). Start with `WORKING_RULES.md` and its action-based reading map; use `CODEX_WORKFLOW.md` for local checkout work. Read engineering, runtime, Git, licensing and bootstrap rules when the action requires them.

## Identity and current phase

- Product: **Extra Sermons**.
- Repository: `NikichMods/ExtraSermons`.
- Target: the original **Graveyard Keeper**, not Graveyard Keeper 2. Shared research currently covers version 1.407; this project's supported game build, platform and loader must be verified before implementation.
- Current phase: adopted product concept and documentation bootstrap. No implementation, binary, release or runtime acceptance exists.

## Sources of truth

Inspect the actual checkout, branch, changes and remote state before work. Read this contract and `docs/DESIGN.md`; consult `docs/EXISTING_MODS.md` for the initial comparison.

Shared host knowledge is owned by [NikichMods/GraveyardKeeperResearch](https://github.com/NikichMods/GraveyardKeeperResearch), starting at `docs/RESEARCH_INDEX.md`. Relevant routing includes `DIALOGUE_QUEST_AND_FLOWCANVAS.md`, `CRAFTING_INVENTORY_AND_TRADING.md`, `GAME_INTERNALS.md` and `UI_INPUT_TIME_AND_ENVIRONMENT.md`. Check existing knowledge before fresh host inspection; preserve each finding's build, evidence level and limits. This project owns its mechanics, balance, implementation and acceptance; reusable host facts belong in shared research.

## Adopted behavior

`docs/DESIGN.md` owns the exact product scope. Buy a permit through Royal Services for **one additional sermon**, initially **one gold coin** per permit. The fee is provisional. Do not turn this into an unrestricted prayer day, consume/downgrade prayers as an extra charge, replace normal weekly access, or change sermon rewards without explicit agreement.

Permit storage, timing, consumption and persistence semantics are unresolved; do not silently select them as product requirements. Investigate native availability/payment/completion seams before designing production behavior. Do not infer implementation readiness from this bootstrap.

Do not commit game binaries, extracted assets, copied/decompiled game source, saves, private logs or credentials. Use original code and minimal derived evidence.

## Git and acceptance

- `main` holds accepted documentation and, later, accepted runtime source. Documentation-only bootstrap may go directly to `main`.
- Use `codex/<topic>` for research or implementation; unaccepted runtime behavior stays off `main`.
- Runtime behavior needs explicit acceptance of the identified tested build before stable promotion. Follow DevRules for immutable numbered artifacts and exact source/hash identity.
- Intended installed filename: `ExtraSermons.dll`; numbered handoff: `ExtraSermons-<version>.dll`. Choose the semantic-version owner with the actual build system before the first binary. Bootstrap consumes no version.
- GitHub Releases is the intended stable binary download surface. Build tooling and CI are not selected yet.

## Licensing

MPL-2.0 for original project code and software-supporting documentation; see `LICENSE` and `LICENSING.md`. Add SPDX notices when original source files are introduced. Host and third-party material retain their own terms.
