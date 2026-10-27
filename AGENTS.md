# Extra Sermons — Working Contract

This repository follows [NikichMods/DevRules](https://github.com/NikichMods/DevRules). Start with `WORKING_RULES.md` and its action-based reading map; use `CODEX_WORKFLOW.md` for local checkout work. Read engineering, runtime, Git, licensing and bootstrap rules when the action requires them.

## Identity and current phase

- Product: **Extra Sermons**.
- Repository: `NikichMods/ExtraSermons`.
- Target: the original **Graveyard Keeper**, not Graveyard Keeper 2. Shared research currently covers version 1.407; this project's supported game build, platform and loader must be verified before implementation.
- Current phase: adopted product concept plus an explicitly authorized **research-only runtime probe** on `codex/sermon-runtime-probe`. See `docs/SERMON_RUNTIME_PROBE.md`. No production implementation, stable binary/release or runtime acceptance exists.

## Sources of truth

Inspect the actual checkout, branch, changes and remote state before work. Read this contract and `docs/DESIGN.md`; consult `docs/EXISTING_MODS.md` for the initial comparison.

Shared host knowledge is owned by [NikichMods/GraveyardKeeperResearch](https://github.com/NikichMods/GraveyardKeeperResearch), starting at `docs/RESEARCH_INDEX.md`. Relevant routing includes `DIALOGUE_QUEST_AND_FLOWCANVAS.md`, `CRAFTING_INVENTORY_AND_TRADING.md`, `GAME_INTERNALS.md` and `UI_INPUT_TIME_AND_ENVIRONMENT.md`. Check existing knowledge before fresh host inspection; preserve each finding's build, evidence level and limits. This project owns its mechanics, balance, implementation and acceptance; reusable host facts belong in shared research.

## Adopted behavior

`docs/DESIGN.md` owns the exact product scope. Buy a permit through Royal Services for **one additional sermon**, initially **one gold coin** per permit. The fee is provisional. Do not turn this into an unrestricted prayer day, consume/downgrade prayers as an extra charge, replace normal weekly access, or change sermon rewards without explicit agreement.

`docs/DESIGN.md` now owns adopted first-version permit semantics: one non-expiring unused authorization per save, native free sermon first, consume at a valid extra-service start, and a contextual native-looking pulpit with a document indicator rather than a physical inventory item or separate altar/tab. These are product choices, **not** proof of a safe game integration. Exact UI placement/localization, Royal Services insertion, sermon-without-bishop execution, session preservation and coexistence with both PrayerClarity editions remain evidence/acceptance gates. Do not silently revise the adopted rules or infer production readiness from research.

Do not commit game binaries, extracted assets, copied/decompiled game source, saves, private logs or credentials. Use original code and minimal derived evidence.

## Git and acceptance

- `main` holds accepted documentation and, later, accepted runtime source. Documentation-only bootstrap may go directly to `main`.
- Use `codex/<topic>` for research or implementation; unaccepted runtime behavior stays off `main`.
- Runtime behavior needs explicit acceptance of the identified tested build before stable promotion. Follow DevRules for immutable numbered artifacts and exact source/hash identity.
- Intended installed filename: `ExtraSermons.dll`; numbered handoff: `ExtraSermons-<version>.dll`. Choose the semantic-version owner with the actual build system before the first binary. Bootstrap consumes no version.
- Research version owner: `research/RuntimeProbe/Identity.cs::Version`, derived into assembly/plugin metadata and the numbered filename. The probe uses a separate research GUID and numeric BepInEx version; it is not a stable release.
- Research handoff gate: clean committed source, `scripts/Build-RuntimeProbe.ps1` Release-equivalent optimized deterministic net471 compilation against installed host/loader references, policy assertions and native metadata/IL contract assertions. Hosted CI independently checks the host-free policy; it cannot compile the actual game integration without proprietary local references. Never upload those references. Exact local source/DLL identity must be recorded before installation.
- GitHub Releases is the intended stable binary download surface. Production build/release CI remains unselected.

## Licensing

MPL-2.0 for original project code and software-supporting documentation; see `LICENSE` and `LICENSING.md`. Add SPDX notices when original source files are introduced. Host and third-party material retain their own terms.
