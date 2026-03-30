# Sprint 4 Documentation — Completed First Level

## Team

| Role | Member |
|------|--------|
| Project Manager | Niko |
| Developer | Thomas |
| Developer | Zach |
| Developer | Sri |
| Developer | Sudhish |

---

## Sprint 3 Retrospective

*(To be filled in by each team member)*

### Zach
- **What went well?**
- **What can be improved?**

### Thomas
- **What went well?**
- **What can be improved?**

### Sri
- **What went well?**
- **What can be improved?**

### Sudhish
- **What went well?**
  Good collaboration between group for discussing debugging as well as sprint planning
- **What can be improved?**
  Having more scheduled meetings outside of class.
  Having a better work distribution. It was jumbled around, causing a slow start to sprint 3

### Action Plan

**For Devs:**
- Compressed schedule — start immediately, no waiting for dependencies if possible
- Use PRs consistently; no direct pushes to `dev`
- Communicate blockers same-day

**For PM (Niko):**
- Track task board daily, enforce branch discipline
- Coordinate shared tasks (boss, abilities, game states)
- Handle code quality tasks in parallel with dev work

---

## Sprint Info

| | |
|---|---|
| **Sprint Dates** | 3/25 – 4/11 |
| **Functionality Check-in** | Fri 4/4 before midnight |
| **Final Submission** | Fri 4/11 before midnight |
| **CATME Peer Eval** | Sat 4/12 before midnight |

**Goal:** Ship a playable first level with audio, HUD, game state transitions, abilities, boss fight, and a clean codebase.

**Schedule Note:** This is a compressed ~2.5 week sprint. All members should be working on tasks from Day 1. No waiting for Week 2 to start feature work.

---

## Sprint 3 Carry-Over Bugs

| Bug | Description | Assignee |
|-----|-------------|----------|
| Ice-glide movement | Knight glide-on-ice effect after collision when not pressing a key | Sudhish |
| Vengefly squeeze-through | Vengefly can squeeze between two blocks | Sudhish |
| Vengefly platform pass-through | Vengefly moves through platforms — adjust collision | Sudhish |
| Spike collision response | Knight doesn't respond correctly when hitting spikes | Sudhish |
| Death physics on upSlash | Broken death physics when knight uses upSlashAttack | Sri |
| Downslash coordinates | Downslash effect coordinates are wrong | Sri |
| Camera follow (downward) | Camera doesn't follow knight when going down | Sri |
| Knight sprite quality | Some knight sprites are lower quality — need re-extraction | Thomas |
| Knight animations | Update falling/running animations (minor fix) | Thomas |
| Hard-coded collision numbers | Magic numbers in collision code | Sudhish |

---

## Task Breakdown & Assignments

### 1. Collision & Physics (Sudhish — primary, Sri — assist)

Sudhish owns the collision system. Sri picks up physics bugs (death physics, downslash) to reduce Sudhish's load.

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Collision refactor | Unify `CollisionManager`, `CollisionResponses`, `CollisionDetector` into one consistent system | Sudhish | `feature/collision-refactor` |
| IBreakable interface | Implement `IBreakable` for walls and grass | Sudhish | `feature/breakable-objects` |
| IHazard interface | Implement `IHazard` for spikes and enemy projectiles | Sudhish | `feature/hazard-objects` |
| Spike response fix | Fix knight response when hitting spikes; use `isGrounded` | Sudhish | `feature/spike-response` |
| Vengefly collision fixes | Fix squeeze-through and platform pass-through | Sudhish | `feature/vengefly-collision` |
| Ice-glide fix | Fix knight sliding after collision | Sudhish | `feature/knight-movement-fix` |
| Collision magic numbers | Extract hard-coded collision values to constants | Sudhish | `feature/collision-constants` |
| Death physics fix | Fix broken physics on upSlashAttack death | Sri | `feature/death-physics` |
| Downslash coord fix | Fix downslash effect coordinates | Sri | `feature/downslash-fix` |
| Camera fix (downward) | Fix camera to follow knight when moving down | Sri | `feature/camera-follow-fix` |

### 2. Level Design & Loading (Thomas — primary)

| Task | Description | Branch |
|------|-------------|--------|
| Dungeon layout design | Design full dungeon layout with multiple rooms and connections | `feature/dungeon-layout` |
| Level XML updates | Update level XML files for room layout, enemy placement, item locations | `feature/level-xml` |
| Room transitions | Implement transitions between rooms | `feature/room-transitions` |
| Test room | Keep a room with an assortment of all items for testing | `feature/test-room` |
| Mouse teleport (keep) | Keep mouse-click room teleport for testing | *(existing)* |
| Knight sprite re-extraction | Fix lower quality knight sprites | `feature/knight-sprites` |
| Knight animation updates | Update falling/running animations | `feature/knight-animations` |

### 3. Audio System (Zach — primary)

| Task | Description | Branch |
|------|-------------|--------|
| AudioManager | Create `Audio/AudioManager.cs` — central loading/playing/stopping system | `feature/audio-manager` |
| Background music | Music per game state (playing, paused, menu) | `feature/audio-music` |
| Sound effects | SFX: attack, damage, heal, jump, enemy death, item pickup | `feature/audio-sfx` |
| Mute toggle | Mute toggle for background music (course requirement) | `feature/audio-mute` |
| Audio assets | Source and add audio files to `Content/audio/` | `feature/audio-assets` |

### 4. Boss Fight (Zach + Thomas — shared)

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Boss extraction | Extract boss assets from source material | Thomas | `feature/boss-extraction` |
| Boss sprites & animations | Boss sprite sheets, animation states | Thomas | `feature/boss-sprites` |
| Boss behavior/AI | Boss attack patterns, phases, movement | Zach | `feature/boss-behavior` |
| Boss room integration | Place boss in dungeon, trigger fight on room entry | Zach | `feature/boss-room` |

### 5. Abilities (Thomas + Sudhish + Sri — shared)

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Ability sprites | Create/source sprites for each ability | Thomas | `feature/ability-sprites` |
| Ability implementation | Actual ability logic and state changes | Sudhish | `feature/ability-logic` |
| Ability UI integration | Hook abilities into HUD/inventory | Sri | `feature/ability-ui` |

### 6. HUD (Sri — primary, Zach — assist)

Zach helps with HUD after audio is done.

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Health display | Masks or equivalent health indicator | Sri | `feature/hud-health` |
| Soul/mana meter | Visual meter for soul/mana resource | Sri | `feature/hud-soul` |
| Geo counter | Display current geo count | Zach | `feature/hud-geo` |
| HUD layout | Position and style all HUD elements | Sri | `feature/hud-layout` |

### 7. Game State Transitions (Sri — primary, Zach — assist, Thomas — assist)

Shared across the team. Sri owns the core system; Zach and Thomas each take a screen.

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| GameState enum | Define `GameState` enum: Playing, Paused, Inventory, GameOver, Win | Sri | `feature/gamestate-enum` |
| Pause state | Pause overlay, freeze game logic, resume | Sri | `feature/state-pause` |
| Inventory/item selection | Item selection screen with transitions to/from gameplay | Sri | `feature/state-inventory` |
| Game over screen | Game over screen on player death | Zach | `feature/state-gameover` |
| Win state | Triggered on grabbing win condition item | Thomas | `feature/state-win` |
| Level reset on death | Reset the full level when the player dies | Sri | `feature/level-reset` |

### 8. Code Quality & Cleanup (All — ongoing, focus Week 3)

Tasks sourced from the [Sprint3_Into_Sprint4_Cleanup](Sprint3_Into_Sprint4_Cleanup.md) report. Niko handles structural cleanup; everyone handles magic numbers in their own areas.

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Dead code removal | Delete `SetSpriteCommand.cs`, `Interfaces/ICollidable.cs`, commented blocks, unused imports | Niko | `core/dead-code-cleanup` |
| Folder renames / typo fixes | `Enviroment` -> `Environment`, drop `_Classes` suffixes | Niko | `core/folder-renames` |
| Namespace cleanup | Add namespaces to all files missing them | Niko | `core/namespace-cleanup` |
| Consolidate environment files | 10 platform files -> 1, 3 path files -> 1, 3 spike files -> 1 | Niko | `core/consolidate-env` |
| Magic numbers sweep | Extract remaining magic numbers/strings to `GameConstants.cs` or similar | All (own areas) | `feature/magic-numbers` |
| TODO sweep | Fix, track, or delete every TODO in the codebase | All | *(no branch — per-file)* |
| Command pattern decision | If team decides to consolidate: replace 26 command classes with lambdas in `KeyboardBindings.cs` | TBD | `core/command-cleanup` |
| Sprint 5 research | Begin research on feature ideas | All | *(no branch)* |

---

## Assignment Summary

| Member | Owns | Shares / Assists |
|--------|------|------------------|
| **Niko (PM)** | PR reviews, documentation, branch management, dead code & namespace cleanup | Magic numbers sweep, code quality coordination |
| **Thomas** | Level design (layout, XML, room transitions, test room), knight sprites/animations | Boss (extraction, sprites), ability sprites, win state screen |
| **Zach** | Audio system (manager, music, SFX, mute, assets) | Boss (behavior, AI, room integration), game over screen, HUD geo counter |
| **Sri** | HUD (health, soul, layout), game states (enum, pause, inventory, level reset) | Death physics fix, downslash fix, camera fix, ability UI |
| **Sudhish** | Collision (refactor, IBreakable, IHazard, all collision bugs, constants) | Ability logic, ice-glide fix |

---

## Git Branching Strategy

```
dev
├── feature/collision-refactor                (Sudhish)
│   ├── feature/breakable-objects             (Sudhish)
│   ├── feature/hazard-objects                (Sudhish)
│   ├── feature/spike-response               (Sudhish)
│   ├── feature/vengefly-collision            (Sudhish)
│   ├── feature/knight-movement-fix           (Sudhish)
│   └── feature/collision-constants           (Sudhish)
├── feature/level                             (Thomas)
│   ├── feature/dungeon-layout
│   ├── feature/level-xml
│   ├── feature/room-transitions
│   └── feature/test-room
├── feature/audio                             (Zach)
│   ├── feature/audio-manager
│   ├── feature/audio-music
│   ├── feature/audio-sfx
│   ├── feature/audio-mute
│   └── feature/audio-assets
├── feature/boss                              (Zach + Thomas)
│   ├── feature/boss-extraction               (Thomas)
│   ├── feature/boss-sprites                  (Thomas)
│   ├── feature/boss-behavior                 (Zach)
│   └── feature/boss-room                     (Zach)
├── feature/abilities                         (Thomas + Sudhish + Sri)
│   ├── feature/ability-sprites               (Thomas)
│   ├── feature/ability-logic                 (Sudhish)
│   └── feature/ability-ui                    (Sri)
├── feature/hud                               (Sri + Zach)
│   ├── feature/hud-health                    (Sri)
│   ├── feature/hud-soul                      (Sri)
│   ├── feature/hud-geo                       (Zach)
│   └── feature/hud-layout                    (Sri)
├── feature/gamestate                         (Sri + Zach + Thomas)
│   ├── feature/gamestate-enum                (Sri)
│   ├── feature/state-pause                   (Sri)
│   ├── feature/state-inventory               (Sri)
│   ├── feature/state-gameover                (Zach)
│   ├── feature/state-win                     (Thomas)
│   └── feature/level-reset                   (Sri)
├── feature/death-physics                     (Sri)
├── feature/downslash-fix                     (Sri)
├── feature/camera-follow-fix                 (Sri)
├── feature/knight-sprites                    (Thomas)
├── feature/knight-animations                 (Thomas)
├── feature/magic-numbers                     (All)
├── core/dead-code-cleanup                    (Niko)
└── core/namespace-cleanup                    (Niko)
```

### Merge Order
1. Leaf branches merge into their parent via PR
2. Parent branches merge into `dev`
3. All PRs require at least one review before merging

---

## Definition of Done

A task is **done** when:
- Code compiles and runs without errors
- The feature works as described in the task description
- No regressions introduced to existing functionality
- Code has been pushed to the correct feature branch
- A PR has been opened and reviewed by at least one team member
- No magic numbers or strings introduced

---

## Sprint Timeline & Priorities

**This is a compressed ~2.5 week sprint. Everyone starts Day 1. No idle time.**

### Week 1 (3/25 – 3/31) — Foundation + Early Features

Everyone is working in parallel from the start.

| Who | Primary Focus | Also Start |
|-----|---------------|------------|
| **Thomas** | Dungeon layout, level XML, knight sprites/animations | Boss extraction |
| **Zach** | Audio manager, audio assets, background music | Sound effects |
| **Sri** | GameState enum, camera fix, death physics, downslash fix | HUD health display |
| **Sudhish** | Collision refactor, spike response, vengefly fixes | IBreakable, IHazard |
| **Niko** | Task board setup, PR reviews | Dead code removal |

**By end of Week 1:** Collision refactor in progress, level layout designed, audio manager functional, GameState enum merged, carry-over bugs fixed.

### Week 2 (4/1 – 4/4) — Feature Push (check-in Friday)

**Target: 2/3 functionality complete for 4/4 check-in**

| Who | Must Finish | Stretch |
|-----|-------------|---------|
| **Thomas** | Room transitions, level XML complete, boss sprites | Win state screen |
| **Zach** | SFX, mute toggle, audio complete | Game over screen, HUD geo |
| **Sri** | HUD (health, soul, layout), pause state, inventory screen | Level reset |
| **Sudhish** | IBreakable, IHazard, collision constants, ice-glide fix | Ability logic |
| **Niko** | Namespace cleanup, PR reviews | First code review round |

**By 4/4 check-in:** Audio working, HUD visible, pause + at least one game state functional, rooms loadable and transitionable, collision bugs resolved.

### Week 3 (4/5 – 4/11) — Polish, Boss, Process

| Who | Primary Focus | Process Tasks |
|-----|---------------|---------------|
| **Thomas** | Win state screen, test room finalization | Code review (readability + quality) |
| **Zach** | Boss behavior/AI, boss room, game over screen, HUD geo | Code review (readability + quality) |
| **Sri** | Level reset, inventory polish, ability UI | Code review (readability + quality) |
| **Sudhish** | Ability logic, magic numbers in collision code | Code review (readability + quality) |
| **Niko** | Magic numbers sweep, README, sprint reflection | Code review (readability + quality), code metrics |

**By 4/8:** All functionality complete, refactoring done, both code review rounds complete.
**By 4/11:** README, sprint reflection, code metrics, submission.

---

## Process Requirements

### Code Reviews

Each member reviews at least one class for **readability** and one for **code quality/maintainability**, and has at least one of their own classes reviewed for each.

Use pull requests (preferred) or plaintext review docs with the required fields from the course guidelines.

### Documentation

- **README:** Program controls, known bugs, tools/processes used
- **Code metrics:** VS Analyze menu or Roslyn analyzers — at least once per week
- **Sprint reflection:** Brief report using burndown chart as basis

### Functionality Check-in (4/4)

Target: at least 2/3 of functional requirements complete. One team member zips and submits on Carmen. Worth ~7.5% of sprint grade.

---

## Team Decisions Needed

| Decision | Options | Status |
|----------|---------|--------|
| Room transition style | Scroll / Fade / Hard cut | TBD |
| Command pattern cleanup | Keep 26 command classes vs. consolidate to lambdas | TBD |
| Boss design | Which boss, attack patterns, phases | TBD |
| Win condition item | Triforce piece equivalent for Hollow Knight | TBD |
