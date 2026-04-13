# Sprint 5 Documentation — Boss Fight, Layers, Second Dungeon

## Team

| Role | Member |
|------|--------|
| Project Manager | Niko |
| Developer | Thomas |
| Developer | Zach |
| Developer | Sri |
| Developer | Sudhish |

---

## Sprint 4 Retrospective

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
- **What can be improved?**

### Niko
- **What went well?**
- **What can be improved?**

### Action Plan

**For Devs:**
-
-
-

**For PM (Niko):**
-
-
-

---

## Sprint Info

| | |
|---|---|
| **Sprint Dates** | 4/14 – 4/27 |
| **Final Submission** | Tues 4/27 before midnight |
| **CATME Peer Eval** | Thurs 4/28 before midnight |
| **Functionality Check-in** | None required for Sprint 5 |

**Goal:** Ship a complete, polished game with a working boss fight, fixed HUD, visual and collision layering, ability UI, and a second full dungeon — resolving all Sprint 4 carry-overs in Week 1 before new feature work begins.

**Schedule Note:** This is a two-week sprint with a hard Week 1 gate. Carry-over work must close out by 4/20 before any second-dungeon or polish work begins in earnest.

---

## Carry-Over Bugs

Priority 1 work that did not finish in Sprint 4. These must close before new feature work in Week 2.

| Bug / Incomplete Work | Description | Assignee |
|-----------------------|-------------|----------|
| HUD health display | Health display works in pause screen but is broken in active gameplay (merge regression) | Sri |
| Ability UI | Ability UI was incomplete at end of S4 and needs to be finished and wired into the HUD | Sri |
| Downslash attack | Downslash attack still broken after S4 fix attempt — carried from S4 backlog | Sri |
| First dungeon layout | First dungeon rooms not fully built or playable end-to-end | Thomas (primary), Sri (assist) |

### Known Code TODOs

Surfaced from `TODO`/`FIXME` comments in the current codebase. Owners fold these into the matching feature task for this sprint or file a one-off cleanup as part of the magic-numbers sweep.

| File | Line | TODO |
|------|------|------|
| `Enemies/MantisLordStateMachine.cs` | 5 | "TODO: Implement" |
| `Audio/AudioManager.cs` | 9 | remove comments |
| `Audio/AudioManager.cs` | 15 | remove comments |
| `Audio/AudioManager.cs` | 148 | remove comments |
| `Environment/Spike.cs` | 46 | "Tune width/height to match the actual scaled sprite size" |
| `Controllers/KeyboardBindings.cs` | 15 | "remove developer keybinding and change ability/movement binds if needed" |
| `Player/SwordHitbox.cs` | 23 | "Tune sword hitbox dimensions to match sword sprite visually" |
| `Factories/SpriteFactory.cs` | 257 | "Update to dash animation once we have them" |
| `Factories/SpriteFactory.cs` | 393 | "Fix constants" |

---

## Task Breakdown & Assignments

### 1. Carry-Overs (Sri + Thomas + Sudhish — close out Week 1)

Priority 1. None of the Week 2 features start cleanly until these close. Sri pairs with Thomas on the first dungeon; Sudhish is pre-authorized to pick up boss sprites if Thomas is blocked.

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| HUD health fix | Debug gameplay-vs-pause regression on health display; reconcile with merged HUD layout | Sri | `feature/hud-health-fix` |
| Ability UI finish | Complete ability selection UI and wire into HUD/inventory flow | Sri | `feature/ability-ui` |
| Downslash fix (v2) | Re-fix downslash attack hitbox/animation interaction; regression-test against S4 bug | Sri | `feature/downslash-fix-v2` |
| First dungeon completion | Finish remaining rooms, connections, and enemy/item placement so dungeon is playable end-to-end | Thomas | `feature/first-dungeon-complete` |
| First dungeon assist | Pair with Thomas — build out secondary rooms and transitions | Sri | `feature/first-dungeon-assist` |

### 2. Boss Fight (Zach — primary, Sudhish — assist)

Zach owns attack patterns, phases, and room-entry trigger. Sudhish handles integration with collision/physics and the IBreakable/IInteractable work needed for boss-room interactables.

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Boss behavior / AI | Implement attack patterns, phase transitions, movement state machine | Zach | `feature/boss-behavior` |
| Boss room integration | Trigger fight on room entry, implement win/lose conditions, cleanup on victory | Zach | `feature/boss-room` |
| Boss physics integration | Wire boss hitboxes/hurtboxes into collision system; damage exchange | Sudhish | `feature/boss-integration` |

### 3. Level Layers (Niko + Thomas — visual, Sudhish — collision)

Visual and collision layers ship in parallel. Visual layers are parallax/foreground/background; collision layers are logical layers that determine what collides with what.

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Visual layer system | Parallax + foreground + background rendering pipeline | Niko | `feature/layers-visual` |
| Visual layer assets | Supply and wire background/foreground art for first dungeon | Thomas | `feature/layers-visual-assets` |
| Collision layer system | Logical collision layers (player, enemy, environment, hazard, projectile) | Sudhish | `feature/layers-collision` |

### 4. Second Dungeon (Thomas — primary, Zach — XML assist)

Second dungeon must be visually distinct from the first. Thomas leads layout and art; Zach assists on XML wiring if Thomas falls behind after Week 1.

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Second dungeon layout | Design and build rooms, connections, and enemy/item placement for dungeon 2 | Thomas | `feature/second-dungeon-layout` |
| Second dungeon custom sprite | New tileset and sprite theme distinct from the first dungeon | Thomas | `feature/second-dungeon-sprites` |
| Second dungeon XML | Author level XML for the new dungeon; integrate with room transitions | Zach | `feature/second-dungeon-xml` |

### 5. Polish & Interfaces (Sudhish + Zach — primary, All — magic numbers)

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| IBreakable interface | Formalize and apply to walls/grass/breakables | Sudhish | `feature/ibreakable` |
| IInteractable interface | Formalize for benches, items, triggers | Sudhish | `feature/iinteractable` |
| Audio scoping | Camera-bound SFX — only play when source is within camera view | Zach | `feature/audio-scoping` |
| Camera refactor | Refactor camera system for cleaner follow/bounds/room handling | Zach | `feature/camera-refactor` |
| Custom voice lines | Record and integrate custom voice lines into audio pipeline | Zach | `feature/custom-voice-lines` |
| Magic numbers sweep | Extract remaining magic numbers in each owner's area to `GameConstants` | All | `feature/magic-numbers-s5` |

### 6. Process (Niko — primary, All — code reviews)

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Code reviews (readability) | Each member reviews one class for readability | All | *(PR comments)* |
| Code reviews (quality) | Each member reviews one class for code quality/maintainability | All | *(PR comments)* |
| Code metrics | Run weekly code metrics (VS Analyze / Roslyn); track in spreadsheet | Niko | `core/code-metrics` |
| README update | Controls, known bugs, tools/processes | Niko | `core/readme-update` |
| Sprint reflection | Brief burndown-based reflection | Niko | `core/sprint-reflection` |
| Branch + PR tracking | Daily check on Thomas's branches + flag blockers same-day | Niko | *(no branch)* |

---

## Assignment Summary

| Member | Owns | Shares / Assists |
|--------|------|------------------|
| **Niko (PM)** | PR reviews, visual layer system, code metrics, README, sprint reflection, branch tracking | Magic numbers sweep, code review round |
| **Thomas** | First dungeon completion, second dungeon layout, second dungeon custom sprite, visual layer assets | First dungeon pairing with Sri, code review round |
| **Zach** | Boss AI/behavior, boss room integration, audio scoping, camera refactor, custom voice lines, second dungeon XML | Boss physics integration (assist to Sudhish), code review round |
| **Sri** | HUD health fix, ability UI, downslash fix, first dungeon assist | Ability integration touchpoints, code review round |
| **Sudhish** | Collision layer system, IBreakable, IInteractable, boss physics integration | Boss sprite unblock for Thomas, code review round |

Every member has a primary domain and at least one assist lane. No empty rows.

---

## Git Branching Strategy

```
dev
├── feature/carry-overs                         (Sri + Thomas + Sudhish)
│   ├── feature/hud-health-fix                  (Sri)
│   ├── feature/ability-ui                      (Sri)
│   ├── feature/downslash-fix-v2                (Sri)
│   ├── feature/first-dungeon-complete          (Thomas)
│   └── feature/first-dungeon-assist            (Sri)
├── feature/boss                                (Zach + Sudhish)
│   ├── feature/boss-behavior                   (Zach)
│   ├── feature/boss-room                       (Zach)
│   └── feature/boss-integration                (Sudhish)
├── feature/layers                              (Niko + Sudhish + Thomas)
│   ├── feature/layers-visual                   (Niko)
│   ├── feature/layers-visual-assets            (Thomas)
│   └── feature/layers-collision                (Sudhish)
├── feature/second-dungeon                      (Thomas + Zach)
│   ├── feature/second-dungeon-layout           (Thomas)
│   ├── feature/second-dungeon-sprites          (Thomas)
│   └── feature/second-dungeon-xml              (Zach)
├── feature/polish                              (Sudhish + Zach)
│   ├── feature/ibreakable                      (Sudhish)
│   ├── feature/iinteractable                   (Sudhish)
│   ├── feature/audio-scoping                   (Zach)
│   ├── feature/camera-refactor                 (Zach)
│   ├── feature/custom-voice-lines              (Zach)
│   └── feature/magic-numbers-s5                (All)
└── core/process                                (Niko)
    ├── core/code-metrics                       (Niko)
    ├── core/readme-update                      (Niko)
    └── core/sprint-reflection                  (Niko)
```

### Merge Order
1. Leaf branches merge into their parent feature branch via PR
2. Parent feature branches merge into `dev`
3. All PRs require at least one review before merging
4. Carry-over branches merge first (Week 1) so feature work builds on a clean `dev`

---

## Definition of Done

A task is **done** when:
- Code compiles and runs without errors
- The feature works as described in the task description
- No regressions introduced to existing functionality
- Code has been pushed to the correct feature branch
- A PR has been opened and reviewed by at least one team member
- No magic numbers or strings introduced
- Feature demoed on `dev` merge before Week 2 ends where applicable

---

## Sprint Timeline & Priorities

### Week 1 (4/14 – 4/20) — Carry-Overs + Early Feature Starts

Close every Priority 1 carry-over. Start boss AI and collision-layer scaffolding in parallel so Week 2 begins with feature work, not cleanup.

| Who | Primary Focus | Also Start |
|-----|---------------|------------|
| **Thomas** | First dungeon completion | Second dungeon layout rough-in |
| **Zach** | Boss behavior / AI scaffolding | Audio scoping, camera refactor |
| **Sri** | HUD health fix, downslash fix, ability UI | First dungeon assist rooms |
| **Sudhish** | Collision layer system | IBreakable, IInteractable |
| **Niko** | Visual layer system scaffolding, branch tracking, PR reviews | Code metrics baseline |

**By end of Week 1 (4/20 — Gate):**
- First dungeon fully playable end-to-end
- HUD health display fixed and ability UI functional
- Downslash fix merged
- If any of the above is not met, Sri and Sudhish are pre-authorized to absorb remaining Thomas work without waiting

### Week 2 (4/21 – 4/24) — Feature Push + Check-in Gates

Boss fight and second dungeon carry the week. Layers land mid-week so second-dungeon art has layer support to ship against.

| Who | Must Finish | Stretch |
|-----|-------------|---------|
| **Thomas** | Second dungeon layout, second dungeon custom sprite | Visual layer assets for second dungeon |
| **Zach** | Boss room integration, camera refactor, second dungeon XML assist | Audio scoping complete, custom voice lines |
| **Sri** | Ability UI polish, HUD fixes verified against second dungeon | Magic numbers in own areas |
| **Sudhish** | Collision layers merged, boss physics integration, IBreakable/IInteractable | Magic numbers in collision code |
| **Niko** | Visual layers merged, README draft, first code metrics snapshot | First code review round |

**By end of Week 2 (4/24 — Gate):**
- Boss fight functional (patterns, phases, win/lose)
- Second dungeon layout complete and reachable
- Visual and collision layering both working on at least the first dungeon

### Week 3 (4/25 – 4/27) — Polish, Reviews, Submission

Three days to harden, review, and ship. No new features start this week.

| Who | Primary Focus | Process Tasks |
|-----|---------------|---------------|
| **Thomas** | Second dungeon polish, sprite fixes | Code review (readability + quality) |
| **Zach** | Boss tuning, audio scoping final, custom voice lines polish | Code review (readability + quality) |
| **Sri** | Ability UI polish, HUD regression sweep | Code review (readability + quality) |
| **Sudhish** | Collision layer edge cases, IBreakable/IInteractable sweep | Code review (readability + quality) |
| **Niko** | README, sprint reflection, code metrics, magic numbers coordination | Code review (readability + quality) |

**By 4/26:** All functionality done, both code review rounds complete, README drafted.
**By 4/27 (midnight):** Final submission zipped and uploaded; sprint reflection and code metrics included.

---

## Accountability Notes

Verbatim from the Sprint 5 goals:

- Thomas carried first-dungeon level layout into Sprint 5. Paired with Sri with explicit check-in points.
- If Thomas is behind by end of Week 1, Sri and Sudhish are pre-authorized to take over without waiting.
- Niko tracks Thomas's branch activity daily and flags blockers same-day.

---

## Process Requirements

### Code Reviews

Each member reviews at least one class for **readability** and one for **code quality/maintainability**, and has at least one of their own classes reviewed for each. Reviews live on PRs or in plaintext review docs following course-guideline fields.

### Documentation

- **README:** Program controls, known bugs, tools/processes used
- **Code metrics:** VS Analyze menu or Roslyn analyzers — at least once per week, tracked in spreadsheet
- **Sprint reflection:** Brief report using burndown chart as basis

### Submission

One team member zips and submits on Carmen before 4/27 midnight. No early functionality check-in is required for Sprint 5.

---

## Sprint Extensions Proposal (for Instructor)

To be confirmed with the instructor before Week 1 ends. Roughly four extensions spanning gameplay, level design, rendering, and UI:

1. **Boss fight** — phases, attack patterns, room-entry integration
2. **Second dungeon** — custom sprite theme, distinct layout
3. **Visual + collision layering** — parallax rendering plus logical collision layers
4. **Ability UI** — inventory / ability selection UI

---

## What We Are NOT Doing

- No save/load system
- No RPG elements (stats, leveling)
- No procedural generation
- No horde mode

---

## Open Team Decisions

| Decision | Options | Status |
|----------|---------|--------|
| Boss final move set / phase count | TBD | TBD |
| Second dungeon theme | TBD | TBD |
| Parallax layer count | TBD | TBD |
| Ability UI layout | Grid / Radial | TBD |
| MantisLord stub | Keep as future work / Remove | TBD |
