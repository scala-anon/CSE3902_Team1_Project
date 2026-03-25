# Sprint 4 Plan — Completed First Level

**Sprint Dates:** 2026-10-28 through 2026-11-17
**Functionality Check-in:** Mon 11/10 before midnight
**Final Submission:** Mon 11/17 before midnight
**CATME Peer Eval:** Tues 11/18 before midnight

---

## Goals

Ship a playable first level with audio, HUD, game state transitions, and a clean codebase. The cleanup work from [Sprint3_Into_Sprint4_Cleanup.md](Sprint3_Into_Sprint4_Cleanup.md) feeds directly into this — structural fixes first, then features on top.

---

## 1. Codebase Cleanup (Week 1)

Carry-over from the Sprint 3 cleanup report. Complete these before building new features.

| Priority | Task                                                                                                            | Cleanup Report Section |
| -------- | --------------------------------------------------------------------------------------------------------------- | ---------------------- |
| 1        | Dead code removal (delete `SetSpriteCommand.cs`, `Interfaces/ICollidable.cs`, commented blocks, unused imports) | Section 1              |
| 2        | Folder renames and typo fixes (`Enviroment` -> `Environment`, drop `_Classes` suffixes)                         | Section 7              |
| 3        | Add namespaces to all 21 files missing them                                                                     | Section 4              |
| 4        | Consolidate duplicate environment files (10 platform files -> 1, 3 path files -> 1, 3 spike files -> 1)         | Section 2              |
| 5        | Merge `CollisionManager.cs` and `CollisionResponses.cs` into one                                                | Section 2              |
| 6        | Create `Shared/GameConstants.cs` — extract all magic numbers                                                    | Section 5              |
| 7        | Split `TheKnight.cs` into `KnightPhysics`, `KnightCombat`, `KnightHealth` components                            | Section 3              |
| 8        | Split `Game1.cs` — extract `CollisionSystem`, `DebugOverlay`                                                    | Section 3              |
| 9        | Unify state management — `KnightState` enum, `GameState` enum, enemy state enums                                | Section 6              |
| 10       | Sweep all TODOs — fix, track, or delete each one                                                                | Section 8              |

See the cleanup report's **Suggested Order of Operations** (bottom of Section 10) and **Appendix B** for the target directory structure.

---

## 2. Level Design and Loading (Week 1-2)

- Update level XML files to define the full dungeon layout (multiple rooms, connections)
- Keep mouse-click room teleport for testing
- Keep a test room with an assortment of items
- Implement room transitions (scroll, fade, or cut — team decision)
- Implement level reset on player death

---

## 3. Audio (Week 2)

Depends on cleanup items 7-9 being done (state enums, split classes = clean hooks).

- Add `Audio/AudioManager.cs` — central system for loading, playing, stopping sounds
- Add `Audio/SoundEffects.cs` — enum or constants for all SFX/music IDs
- Add `Content/audio/` directory for sound assets
- Background music per game state (playing, paused, menu)
- SFX triggers: player attack, damage, heal, jump, enemy death, item pickup
- Mute toggle for background music (testing requirement)

---

## 4. HUD (Week 2)

- Health display (masks or equivalent)
- Soul/mana meter
- Geo counter
- Font does not need to match the original game

---

## 5. Game State Transitions (Week 2-3)

Requires `GameState` enum from cleanup.

| State     | Description                                          |
| --------- | ---------------------------------------------------- |
| Playing   | Normal gameplay                                      |
| Paused    | Pause overlay, freeze game logic                     |
| Inventory | Item selection screen, transitions to/from gameplay  |
| GameOver  | Game over screen on player death (after level reset) |
| Win       | Triggered on grabbing the win condition item         |

---

## 6. Polish and Extras (Week 3)

- Remove remaining magic numbers and strings
- Additional items, enemies, or NPCs (encouraged, not required)
- Begin Sprint 5 feature research

---

## 7. Process Requirements

### Code Reviews

- Each member reviews at least one class for **readability** and has one reviewed
- Each member reviews at least one class for **code quality/maintainability** and has one reviewed
- Use pull requests or plaintext review docs (see course guidelines for required fields)

### Documentation

- README with program controls, known bugs, tools/processes used
- Code metrics (Analyze menu in VS) or Roslyn analyzer output — at least once per week
- Sprint reflection using burndown chart

### Recommended Timeline

| When                  | Milestone                                           |
| --------------------- | --------------------------------------------------- |
| Day 1 (10/28)         | Tasks on board, cleanup started                     |
| End of Week 2 (11/7)  | All functionality complete, first code reviews done |
| Mid Week 3 (11/12)    | Refactoring complete, second code reviews done      |
| End of Week 3 (11/17) | Documentation, reflection, submission               |

---

## Team Decision Needed

**Command pattern (Section 9 of cleanup report):** 26 command classes are mostly trivial one-line wrappers. Could be replaced with lambdas in `KeyboardBindings.cs`, eliminating ~20 files. Keep if it's a course requirement; otherwise consolidate.
