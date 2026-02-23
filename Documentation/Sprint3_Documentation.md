# Sprint 3 Documentation

## Team

| Role | Member |
|------|--------|
| Project Manager | Thomas |
| Developer | Zach |
| Developer | Sri |
| Developer | Suhdish |

---

## Sprint 2 Retrospective

### Zach
- **What went well?** Liked the sprite factory, overall felt things were good
- **What can be improved?** Better time management, more organized during the close of the sprint

### Thomas
- **What went well?** Collaboration was good, everyone's accountability
- **What can be improved?** Sprint submissions should be better organized, more consistent calls

### Suhdish
- **What went well?** Everyone collaborated and communicated well
- **What can be improved?** Clarity on submission and time management, git documentation

### Action Plan

**For Devs:**
- Watch video on git documentation (i.e. commit messages and PR requests)
- More detailed Definition of Done (D.O.D)

**For PM:**
- Break down the full project better
- Watching for PRs and git commits
- Keeping up with documentation

---

## Sprint 3 Objectives

### Core Requirements
- Continue to develop more core features of the 2D game framework
- Implement collision handling for all types of collisions that can occur, causing state transitions or position changes when necessary
- Create individual "rooms" of the dungeon each with its own subset of objects; store this information in file(s) (CSV or XML) and write code to initialize objects from them
- Create an artificial level containing an instance of Link and all types of objects found in the first dungeon; include a way to quickly switch to any room for testing

### Controls & UI
- Arrow keys and WASD for player movement
- Consider creating a gamepad class (not a hard requirement)
- Numeric keys (1, 2, 3, etc.) for using different items
- Recommendation: have all item pickups in the starting room, or start player with infinite items
- Mouse controller for room transitions (e.g., clicking a map, or left/right click to cycle rooms)

---

## Task Breakdown & Assignments

### 1. Collision System

#### 1.1 Collision Detection

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Block/Wall Detection | Bounding box detection between player/enemies and environment blocks/walls | Zach | `feature/collision-detection-block` |
| Enemy Detection | Bounding box detection between player and all enemy types | Suhdish | `feature/collision-detection-enemy` |
| Projectile Detection | Bounding box detection for projectiles against player, enemies, and blocks | Sri | `feature/collision-detection-projectile` |
| Item Detection | Bounding box detection between player and item pickups | Zach | `feature/collision-detection-item` |

#### 1.2 Collision Response

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Player-Block Response | Prevent Link from walking through walls, handle locked doors, pushable blocks | Sri | `feature/collision-response-block` |
| Player-Enemy Response | Handle damage, knockback, invincibility frames, state transitions | Zach | `feature/collision-response-enemy` |
| Projectile Response | Projectiles stop/despawn on walls; handle Link hit by enemy projectiles and enemies hit by Link's projectiles | Suhdish | `feature/collision-response-projectile` |
| Item Pickup Response | Handle item pickups (rupees, hearts, keys, etc.) and inventory/state updates | Sri | `feature/collision-response-item` |

### 2. Room/Level Loading System

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Design Room Data File Format | Define the CSV/XML schema: object type, position, room metadata | Thomas | `feature/room-loading-format` |
| Write File Parser / Room Loader | Code that reads the data file(s) and instantiates correct game objects at correct positions | Suhdish | `feature/room-loading-parser` |
| Create Data Files for Each Room | Populate files with object placement data for every room in the dungeon | Zach | `feature/room-loading-data` |
| Room Transition Logic | Unload current room and load next room when Link walks to an edge/door | Sri | `feature/room-loading-transitions` |
| Object Lifecycle Management | Objects created/destroyed properly on room switch; persistent state (e.g., collected items stay collected) | Suhdish | `feature/room-loading-lifecycle` |

### 3. Test Level & Debug Features

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Create Artificial Test Level | A room containing Link and every object type in the first dungeon (all enemies, items, blocks, NPCs) | Zach | `feature/test-level` |
| Room-Switching Debug Feature | Keyboard/mouse mechanism to instantly jump to any room (left/right click or map-based) | Sri | `feature/test-room-switcher` |

### 4. Core Framework (Supporting)

| Task | Description | Assignee | Branch |
|------|-------------|----------|--------|
| Game State Management | Ensure game loop supports room-scoped updates and draws | Suhdish | `feature/core-state-management` |
| Camera/Viewport for Rooms | Each room displays correctly within the game window | Sri | `feature/core-camera` |

---

## Assignment Summary

| Member | Tasks |
|--------|-------|
| **Thomas ** | Design room data file format, PR reviews, documentation, branch management |
| **Zach** | Block detection, item detection, enemy collision response, room data files, test level |
| **Sri** | Projectile detection, block response, item response, room transitions, room switcher, camera |
| **Suhdish** | Enemy detection, projectile response, room parser, object lifecycle, state management |

---

## Git Branching Strategy

All feature work branches off `dev`. Major features get a parent branch, which then has child branches for sub-tasks. PRs merge children back into parent, then parent into `dev`.

```
dev
├── feature/collision
│   ├── feature/collision-detection
│   │   ├── feature/collision-detection-block         (Zach)
│   │   ├── feature/collision-detection-enemy         (Suhdish)
│   │   ├── feature/collision-detection-projectile    (Sri)
│   │   └── feature/collision-detection-item          (Zach)
│   └── feature/collision-response
│       ├── feature/collision-response-block           (Sri)
│       ├── feature/collision-response-enemy           (Zach)
│       ├── feature/collision-response-projectile      (Suhdish)
│       └── feature/collision-response-item            (Sri)
├── feature/room-loading
│   ├── feature/room-loading-format                    (Thomas)
│   ├── feature/room-loading-parser                    (Suhdish)
│   ├── feature/room-loading-data                      (Zach)
│   ├── feature/room-loading-transitions               (Sri)
│   └── feature/room-loading-lifecycle                 (Suhdish)
├── feature/test
│   ├── feature/test-level                             (Zach)
│   └── feature/test-room-switcher                     (Sri)
└── feature/core
    ├── feature/core-state-management                  (Suhdish)
    └── feature/core-camera                            (Sri)
```

### Merge Order
1. Leaf branches merge into their parent via PR (e.g., `feature/collision-detection-block` -> `feature/collision-detection`)
2. `feature/collision-detection` and `feature/collision-response` merge into `feature/collision`
3. `feature/collision`, `feature/room-loading`, `feature/test`, and `feature/core` merge into `dev`
4. All PRs require at least one review before merging

### PR & Commit Guidelines (from Sprint 2 Action Plan)
- Write clear, descriptive commit messages
- PRs should reference the task they complete
- Niko reviews and approves PRs

---

## Definition of Done

A task is considered **done** when:
- Code compiles and runs without errors
- The feature works as described in the task description
- Code has been pushed to the correct feature branch
- A PR has been opened and reviewed by at least one team member
- No regressions introduced to existing functionality

---

## Sprint Timeline & Priorities

**Critical Path (do first):**
1. Collision Detection (all sub-tasks) — foundation for everything else
2. Room Data File Format & Parser — needed before rooms can be built
3. Core State Management — supports room-scoped logic

**Second Priority:**
4. Collision Response (all sub-tasks) — depends on detection
5. Room Data Files & Transitions — depends on parser
6. Object Lifecycle — depends on transitions

**Final Priority:**
7. Test Level & Room Switcher — depends on rooms and collisions working
8. Camera/Viewport — polish
