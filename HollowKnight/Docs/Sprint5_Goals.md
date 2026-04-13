# Sprint 5 Goals

## Sprint Overview

**Sprint Dates:** 4/14 – 4/27
**Due Date:** Tues 4/27 before midnight
**Team Eval:** Thurs 4/28 before midnight
**No early functionality check-in required for Sprint 5.**

---

## Sprint Goal

Ship a complete, polished game with a working boss fight, fixed HUD, visual and collision layering, ability UI, and a second full dungeon — resolving all Sprint 4 carry-overs in Week 1 before new feature work begins.

---

## Priority Order

### 🔴 Priority 1 — Carry-Overs (must finish Week 1)

These were supposed to ship in Sprint 4. They block new feature work and must be resolved first.

| Item                         | Owner            | Notes                                                                                                                |
| ---------------------------- | ---------------- | -------------------------------------------------------------------------------------------------------------------- |
| HUD health display bug       | Sri              | Works in pause, broken in gameplay — fix the merge regression                                                        |
| Ability UI                   | Sri              | Was incomplete at end of S4                                                                                          |
| Downslash attack fix         | Sri              | Carried from S4 backlog                                                                                              |
| Level layout (first dungeon) | Thomas + Sri     | Thomas owns but Sri assists — first dungeon rooms must be fully built and playable before second dungeon work starts |
| Boss sprites & animations    | Thomas + Sudhish | Thomas continues extraction; Sudhish steps in to unblock if Thomas falls behind again                                |

### 🟡 Priority 2 — Core Sprint 5 Features (Week 1–2)

New features for this sprint. Do not start until carry-overs are either done or actively unblocked.

| Item                         | Owner          | Notes                                                                                |
| ---------------------------- | -------------- | ------------------------------------------------------------------------------------ |
| Boss fight behavior & AI     | Zach + Sudhish | Zach owns AI/attack patterns; Sudhish owns integration and physics interactions      |
| Boss room integration        | Zach           | Trigger fight on room entry, win/lose conditions                                     |
| Level layers — visual        | Niko + Thomas  | Parallax, foreground, background rendering layers                                    |
| Level layers — collision     | Sudhish        | Logical collision layers — which objects pass through what                           |
| Second dungeon layout & XML  | Thomas + Zach  | Thomas owns layout design; Zach assists with XML and room wiring if Thomas is behind |
| Second dungeon custom sprite | Thomas         | New tileset/sprite theme distinct from first dungeon                                 |

### 🟢 Priority 3 — Polish & Process (Week 2–3)

| Item                                  | Owner           | Notes                                                          |
| ------------------------------------- | --------------- | -------------------------------------------------------------- |
| IBreakable / IInteractable interfaces | Sudhish         | Backlog item, fits alongside collision layer work              |
| Audio scoping (camera-bound SFX)      | Zach            | Only play sounds for enemies/objects visible in camera         |
| Code reviews (readability + quality)  | All             | Each member reviews one class for readability, one for quality |
| Code metrics                          | Niko            | Run weekly, record in spreadsheet                              |
| README update                         | Niko            | Controls, known bugs, tools used                               |
| Sprint reflection                     | Niko            | Burndown-based, brief                                          |
| Magic numbers sweep                   | All (own areas) | Each person cleans their own code                              |

---

## Team Ownership — Sprint 5

| Member        | Primary Owns                                                                           | Assisted By                                    |
| ------------- | -------------------------------------------------------------------------------------- | ---------------------------------------------- |
| **Niko (PM)** | PR reviews, visual layers, documentation, code metrics, README                         | —                                              |
| **Thomas**    | Boss sprites (finish), second dungeon layout + custom sprite, first dungeon completion | Sri (level), Sudhish (boss sprites if blocked) |
| **Zach**      | Boss AI/behavior, boss room integration, second dungeon XML assist, audio scoping      | Sudhish (boss integration)                     |
| **Sri**       | HUD fix, ability UI, downslash fix, first dungeon assist                               | —                                              |
| **Sudhish**   | Collision layers, boss sprite unblock, IBreakable/IInteractable, boss integration      | —                                              |

---

## Accountability Notes

**Thomas** carried two unfinished areas into Sprint 5 (level layout, boss sprites). This sprint:

- He is paired with Sri on level layout and Sudhish on boss sprites with explicit check-in points
- If Thomas is behind by end of Week 1, Sri and Sudhish are pre-authorized to take over their respective areas without waiting
- Niko tracks Thomas's branch activity daily and flags blockers same-day

**Check-in gates:**

- End of Week 1 (4/20): First dungeon fully playable, boss sprites complete, HUD and ability UI fixed. If not, reassign immediately.
- End of Week 2 (4/24): Boss fight functional, second dungeon layout complete, both layer types working.
- 4/26: All functionality done, code reviews complete, README drafted.

---

## Backlog Items Included This Sprint

From the provided backlog:

- [x] Fix HUD health display (only shows in pause state)
- [x] Layers for the level (visual + collision)
- [x] Downslash attack fix
- [x] Audio scoping to camera view
- [x] IBreakable, IInteractable interfaces
- [x] Ability UI

Carried from Sprint 4:

- [x] Boss fight (behavior, AI, room integration)
- [x] Level layout (first dungeon rooms barely exist)
- [x] Second dungeon (new goal — full layout + custom sprite)

---

## What We Are NOT Doing This Sprint

To keep scope realistic given Thomas's carry-over debt and the 2.5-week window:

- No save/load system
- No RPG elements
- No procedural generation
- No horde mode

These remain in the backlog for consideration if the team finishes early.

---

## Sprint 5 Extension Justification (for instructor contact)

Per the sprint 5 brief, the team should contact the instructor with proposed topics. Proposed extensions:

1. **Boss fight** — Full boss with behavior phases, attack patterns, and room integration
2. **Second dungeon** — New level with custom sprite theme, distinct layout
3. **Visual + collision layering** — Parallax background/foreground rendering and logical collision layer system
4. **Ability UI** — In-game inventory/ability selection interface

Combined scope: ~4 meaningful extensions across gameplay, level design, rendering, and UI. Recommend confirming with instructor that this is sufficient before Week 1 ends.
