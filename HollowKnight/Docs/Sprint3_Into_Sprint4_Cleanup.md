# HollowKnight Codebase Cleanup Report

**Date:** 2026-03-23
**Branch:** feature/collision
**Framework:** MonoGame (.NET 9)
**Total Source Files:** 83 `.cs` files across 21 directories

---

## 1. Dead Code Removal

Code that is commented out, never called, or entirely unused. Remove all of it — version control exists for a reason.

### Files to Delete
| File | Reason |
|------|--------|
| `Commands/SetSpriteCommand.cs` | Entire file is commented out. Not referenced anywhere. |
| `Interfaces/ICollidable.cs` | Contains `ICollidableTEMP` with a TODO saying to consolidate. The real interface lives in `Collision/ICollidable.cs`. This file is dead weight. |

### Dead Methods to Remove
| File | Method / Lines | Reason |
|------|---------------|--------|
| `Game1.cs` | `HandleProjectileCollisions()` (~lines 181-206) | Defined but never called. The same logic already exists inline in `Update()`. |

### Commented-Out Code Blocks to Remove
| File | Lines (approx) | Description |
|------|----------------|-------------|
| `Game1.cs` | 83-84 | Commented variable declarations |
| `Game1.cs` | 105-118 | 14-line commented-out collision registration block |
| `Game1.cs` | 161-164 | Commented-out obstacle loading code |
| `Vengefly.cs` | 68-77 | Commented-out `changeDirection()` and `changeMovingState()` methods |
| `CollisionDetector.cs` | 31, 35-38 | Commented-out detection logic |
| `SpriteFactory.cs` | ~145 | Commented-out sprite creation line |
| Multiple environment files | Varies | Commented-out `Rectangle` initialization lines in spike and platform classes |

### Unused Imports to Remove
| File | Import |
|------|--------|
| `Game1.cs` (line 17) | `using System.Runtime.Remoting;` — not used anywhere in the file |

---

## 2. Duplicate Code Consolidation

### `CollisionManager.cs` and `CollisionResponses.cs` Are the Same Thing

These two files are 125 and 128 lines respectively. Both implement:
- `ProjectileHitsCollidable()`
- `ResolveProjectileCollisions()`
- `ResolvePlayerBlockCollision()`

Both are referenced in `Game1.cs`. The differences between them are minor (e.g., accessing `Bounds` vs `GetBounds()[0]`). One needs to absorb the other and the duplicate needs to be deleted. Every collision fix currently has to be applied in two places.

**Files:** `Collision/CollisionManager.cs`, `Collision/CollisionResponses.cs`
**Action:** Merge into one. Update all references in `Game1.cs`.

### 10 Identical Tutorial Platform Files

`Enviroment_Classes/Tutorial_Platform_1.cs` through `Tutorial_Platform_10.cs` are each ~1090 lines and nearly identical. The only difference between them is the class name and which `SpriteFactory.CreateTutorialPlatform*()` method they call.

This also means `SpriteFactory.cs` has 10 copy-pasted factory methods (`CreateTutorialPlatform1()` through `CreateTutorialPlatform10()`) that differ by a single string.

**Files:**
- `Enviroment_Classes/Tutorial_Platform_1.cs` through `Tutorial_Platform_10.cs` (10 files)
- `Factories/SpriteFactory.cs` (10 duplicate methods within the file)

**Action:** Create one `TutorialPlatform` class that takes an ID or variant parameter. Replace the 10 factory methods with one `CreateTutorialPlatform(int id, Vector2 position)`. Delete the 10 individual files.

### Path and Spike Duplicates

Same pattern as the platforms, smaller scale.

| Group | Files | What Differs |
|-------|-------|-------------|
| Paths | `Path_1.cs`, `Path_2.cs`, `Path_3.cs` | Class name and factory call |
| Spikes | `Spike.cs`, `FloorSpike.cs`, `CeilingSpike.cs` | Default position and sprite variant |

**Action:** For each group, create a single base class or parameterized class. The spike classes may warrant separate classes if their collision behavior truly differs — but if only position and sprite change, one class with parameters is enough.

### Shared Environment Boilerplate

All environment classes (`Tutorial_Platform_*`, `Path_*`, spike classes, `Path_Ledge.cs`) repeat the same structure: constructor, `GetBounds()`, `Draw()`, `Update()`, `Bounds` property, `ICollidable` + `IObject` implementation. A `PlatformBase` or `EnvironmentObject` abstract base class would hold all of this, and subclasses would only specify what's unique.

**Action:** Create an abstract base in `Enviroment_Classes/` (or the renamed `Environment/` folder) and have all environment objects inherit from it.

---

## 3. God Classes That Need to Be Split

### `Player/TheKnight.cs` — 436 lines, 31+ public methods, 23+ fields

This class handles:
- Physics (velocity, gravity, grounding)
- Movement (left, right, jump, up, down)
- Combat (side slash, up slash, down slash, cooldowns, sword hitbox)
- Healing (hold to heal, cancel, timer, health restore)
- Damage and invincibility (damage timer, knockback direction, knockback speed)
- Health management
- Item/ability system
- Animation state (22 sprite types via `KnightSpriteType` enum)

The `Update()` method (~lines 74-175) is ~100 lines of nested conditionals that interleave all of these concerns.

**Action:** Extract into focused components:
- `KnightPhysics.cs` — velocity, gravity, grounding, knockback
- `KnightCombat.cs` — attack states, cooldowns, slash types, sword hitbox
- `KnightHealth.cs` — HP, damage, invincibility, healing
- `TheKnight.cs` stays as the orchestrator, owns the components, delegates to them

After extraction, `TheKnight.Update()` should be short and readable — a sequence of component update calls followed by animation state resolution.

### `Game1.cs` — 416 lines

This class handles:
- Game initialization and content loading
- Input controller management
- Collision handler registration (hardcoded, ~lines 91-129)
- Per-frame collision detection and response (~lines 244-305)
- Room/level management
- Projectile updates
- Entity iteration and updates
- Debug rendering (hitboxes, grid, state labels)
- All draw calls

**Action:** Extract into subsystems:
- `CollisionSystem.cs` — handler registration + per-frame detection (replaces the merged `CollisionManager`/`CollisionResponses` from Section 2)
- `DebugOverlay.cs` — all debug drawing consolidated (hitbox outlines, grid, state labels). `DebugRenderer.cs` exists but calls are still scattered through `Game1.Draw()`
- Expand `RoomManager.cs` or create `EntityManager.cs` — owns lists of enemies, platforms, pickups so `Game1` isn't looping over raw lists

`Game1` should be a thin shell: `Initialize()`, `LoadContent()`, `Update()` that calls subsystems, `Draw()` that calls subsystems.

---

## 4. Missing Namespaces

21 files have no `namespace` declaration, meaning their classes sit in the global namespace. This will cause collisions as the project grows and makes the codebase harder to navigate.

| Directory | Files Missing Namespaces | Suggested Namespace |
|-----------|-------------------------|-------------------|
| `Enviroment_Classes/` | All 18 files (platforms, paths, spikes) | `HollowKnight.Environment` |
| `Enemy_Classes/` | `Crawlid.cs`, `Vengefly.cs` | `HollowKnight.Enemies` |
| `Ability_Classes/` | `Spirit.cs` | `HollowKnight.Abilities` |

Additionally, `Spirit.cs` uses C# 10 file-scoped namespace syntax (`namespace X;`) while the rest of the project uses block-scoped (`namespace X { }`). Pick one and use it everywhere.

**Action:** Add namespaces to all 21 files. Update `using` statements in any file that references these classes. Enforce one namespace syntax style project-wide.

---

## 5. Hardcoded Magic Numbers

Raw numeric literals for game parameters are scattered across 10+ files. Changing screen resolution or tuning physics means hunting through the entire codebase.

### Screen Dimensions (1280 x 720)
| File | Line(s) | Value |
|------|---------|-------|
| `Vengefly.cs` | 32 | `ScreenFloor = 720f` |
| `Crawlid.cs` | 32 | `ScreenFloor = 720f` |
| `CrawlidStateMachine.cs` | 62, 64 | `1280` (screen width boundary) |
| `VengeFlyStateMachine.cs` | 87, 89 | `1280` (screen width boundary) |
| `Game1.cs` | 141-142 | `levelWidth = 3200`, `levelHeight = 720` |

### Physics / Gameplay Values
Gravity, move speed, jump speed, attack cooldowns, invincibility duration, knockback speed — all defined as raw numbers inside `TheKnight.cs`, enemy classes, and state machines. No shared source of truth.

**Action:** Create `Shared/GameConstants.cs` with organized `public const` fields grouped by category (Screen, Physics, Combat, Enemy). Replace every magic number with the constant. One file to tune, zero hunting.

---

## 6. Inconsistent State Management

### TheKnight Uses Boolean Flags
`TheKnight` manages mutually exclusive states with independent booleans: `isAttacking`, `isDamaged`, `isHealing`, `isGrounded`. This makes state logic fragile — you can accidentally be attacking AND healing AND damaged simultaneously if you miss a flag reset.

### Enemies Use Magic Integers
`CrawlidStateMachine` and `VengeFlyStateMachine` use raw integers for states (0 = Idle, 1 = Turn, 2 = DeathAir, etc.) with `GetStateName()` translating them to strings. The meaning of `state = 2` is not self-documenting.

### No Game-Level State
`Game1` has no concept of game state (Playing, Paused, Menu). This matters for audio — different music per state, pausing should mute SFX, etc.

**Action:**
- Add `KnightState` enum: `Idle`, `Running`, `Jumping`, `Falling`, `Attacking`, `Healing`, `Damaged`, `Dead`. Replace boolean flags in `TheKnight`.
- Add named enums to `CrawlidStateMachine` and `VengeFlyStateMachine`. Replace integer states.
- Add `GameState` enum to `Game1`: `Playing`, `Paused` at minimum.
- Each state machine should expose `CurrentState` as a typed enum — this gives the future audio system clean hooks for sound triggers.

---

## 7. Naming and Convention Issues

### Typo: "Enviroment"
The folder `Enviroment_Classes/` is misspelled (missing 'n'). This typo also appears in:
- `Commands/ChangeNextEnviromentCommand.cs`
- `Commands/ChangePreviousEnviromentCommand.cs`

**Action:** Rename folder to `Environment/` (drop `_Classes` suffix — no other folder uses it). Rename the two command files and their internal class names. Update all references.

### Inconsistent Variable Naming
| Location | Variable | Convention |
|----------|----------|-----------|
| `Spike.cs` line 37 | `rectangle2` | camelCase |
| `FloorSpike.cs` line 37 | `rectangle_2` | snake_case |
| `CeilingSpike.cs` line 40 | `rectangle_2` | snake_case |

C# convention is `camelCase` for local variables. Pick it and enforce it.

### Public Fields Instead of Properties
Multiple environment classes expose `public ISprite Sprite;` as a raw field. C# convention (and good practice) is `public ISprite Sprite { get; private set; }`. Public fields bypass encapsulation.

**Files affected:** `CeilingSpike.cs`, `FloorSpike.cs`, `Path_Ledge.cs`, `Spike.cs`, and likely the platform classes.

### Confusing Interface Name
`IHollowKnight` is the player interface. Only `TheKnight` implements it. The name suggests it's a game-level interface. Consider renaming to `IPlayer` or `IKnight`.

### Inconsistent Folder Naming
Some folders use PascalCase (`Collision`, `Commands`, `Sprites`), others use underscored names (`Enemy_Classes`, `Ability_Classes`, `Enviroment_Classes`). The `_Classes` suffix adds nothing.

**Suggested renames:**
| Current | Proposed |
|---------|----------|
| `Enemy_Classes/` | `Enemies/` |
| `Enviroment_Classes/` | `Environment/` |
| `Ability_Classes/` | `Abilities/` |

---

## 8. Unresolved TODOs

27 TODO comments across the codebase. Each one is either unfinished work or a known problem. They need to be triaged: fix it, track it, or delete it.

| File | Count | Summary |
|------|-------|---------|
| `Player/TheKnight.cs` | 4 | Combat/physics tuning |
| `Enemy_Classes/Vengefly.cs` | 4 | Incomplete enemy behavior |
| `Player/SwordHitbox.cs` | 3 | Hitbox dimension tuning |
| `Game1.cs` | 3 | Structural issues |
| `Enemy_Classes/Crawlid.cs` | 2 | Enemy behavior |
| `Projectiles/Projectile.cs` | 2 | Projectile behavior |
| `Collision/ICollidable.cs` | 1 | Interface consolidation (addressed in Section 1) |
| `Interfaces/ICollidable.cs` | 1 | Same as above |
| `StateMachines/CrawlidStateMachine.cs` | 1 | State machine |
| Environment files (7 files) | 1 each | Hitbox dimension tuning — will be bulk-fixed once `PlatformBase` exists |

**Action:** Go through each one. If it's small, fix it. If it's real work, convert it to a tracked item. If it's stale, delete it. Zero TODOs should remain after cleanup.

---

## 9. Command Pattern Evaluation

There are 26 command classes in `Commands/`, most are 15-line single-method wrappers:

```csharp
public class PlayerMoveRightCommand : ICommand {
    private IHollowKnight knight;
    public PlayerMoveRightCommand(IHollowKnight k) { knight = k; }
    public void Execute() { knight.MoveRight(); }
}
```

This is valid architecture but generates a lot of files for trivial delegation. If this pattern is a course requirement, keep it. If not, these could be replaced with lambda bindings in `KeyboardBindings.cs`:

```csharp
bindings[Keys.Right] = () => knight.MoveRight();
```

This would eliminate ~20 files. **Team decision required.**

---

## 10. Pre-Audio Considerations

Before the audio sprint begins, these structural issues will directly block clean audio integration:

| Blocker | Why It Matters for Audio |
|---------|------------------------|
| No `GameState` enum | Can't switch music between playing/paused/menu states |
| No `KnightState` enum | Can't trigger SFX on state transitions (attack, damage, heal) without reading boolean flags |
| `TheKnight` is monolithic | Audio hooks for player actions will add even more tangled logic to an already 436-line class |
| `Game1` is monolithic | Audio system initialization, update calls, and resource management will bloat it further |
| No event/callback pattern | Every sound trigger requires direct method calls deep inside entity logic — no clean way to subscribe to "player attacked" or "enemy died" events |

Addressing Sections 3, 4, 5, and 6 of this report before starting audio work will make the audio sprint significantly cleaner.

---

## Suggested Order of Operations

1. **Dead code removal** (Section 1) — fast, zero risk, clears noise
2. **Typo fix and folder renames** (Section 7, naming portion) — do it early while references are minimal
3. **Add namespaces** (Section 4) — foundational, needed before new files are added
4. **Platform/environment consolidation** (Section 2, platform portion) — biggest clutter reduction
5. **Collision dedup** (Section 2, collision portion) — removes a dangerous maintenance trap
6. **GameConstants** (Section 5) — quick, unlocks cleaner refactoring in steps 7-8
7. **TheKnight breakup** (Section 3) — required before audio hooks
8. **Game1 breakup** (Section 3) — required before audio system integration
9. **State management unification** (Section 6) — required for audio triggers
10. **TODO sweep** (Section 8) — clean slate
11. **Remaining naming fixes** (Section 7) — polish
12. **Command pattern decision** (Section 9) — team call

---

## Appendix A — Current Directory Structure

What we have right now. Every file listed.

```
HollowKnight/
├── Game1.cs
├── Program.cs
├── HollowKnight.csproj
├── HollowKnight.sln
├── app.manifest
├── Icon.bmp
├── Icon.ico
├── Information.txt
│
├── Ability_Classes/
│   └── Spirit.cs
│
├── Builders/
│   └── KnightSpriteBuilder.cs
│
├── Collision/
│   ├── CollisionDetector.cs
│   ├── CollisionGroupBuilder.cs
│   ├── CollisionHandler.cs
│   ├── CollisionManager.cs          ← duplicate of CollisionResponses
│   ├── CollisionResponses.cs        ← duplicate of CollisionManager
│   ├── CollisionSide.cs
│   ├── DebugRenderer.cs
│   └── ICollidable.cs               ← the real one
│
├── Commands/
│   ├── ChangeNextEnemyCommand.cs
│   ├── ChangeNextEnviromentCommand.cs      ← typo
│   ├── ChangePreviousEnemyCommand.cs
│   ├── ChangePreviousEnviromentCommand.cs  ← typo
│   ├── CycleItemNextCommand.cs
│   ├── CycleItemPreviousCommand.cs
│   ├── JumpToRoomCommand.cs
│   ├── PlayerDownSlashCommand.cs
│   ├── PlayerHealCancelCommand.cs
│   ├── PlayerHealHoldCommand.cs
│   ├── PlayerJumpCommand.cs
│   ├── PlayerMoveDownCommand.cs
│   ├── PlayerMoveLeftCommand.cs
│   ├── PlayerMoveRightCommand.cs
│   ├── PlayerMoveUpCommand.cs
│   ├── PlayerSideSlashCommand.cs
│   ├── PlayerStopMovingHorizontalCommand.cs
│   ├── PlayerStopMovingVerticalCommand.cs
│   ├── PlayerTakeDamageCommand.cs
│   ├── PlayerUpSlashCommand.cs
│   ├── PlayerUseItemCommand.cs
│   ├── QuitCommand.cs
│   ├── SetSpriteCommand.cs          ← DEAD (entirely commented out)
│   ├── SwitchRoomCommand.cs
│   ├── ToggleGridCommand.cs
│   └── ToggleHitboxCommand.cs
│
├── Content/
│   ├── Content.mgcb
│   ├── fonts/
│   │   └── Credits.spritefont
│   ├── levels/
│   │   ├── levelLoader.cs           ← source code in content dir (misplaced)
│   │   └── levelOne.xml
│   └── sprites/
│       ├── attack-atlas.xml
│       ├── enemy-atlas.xml
│       ├── EnemySpriteSheet.png
│       ├── HollowKnightSheet.png
│       ├── knight_abilities-atlas.xml
│       ├── knight_movement-atlas.xml
│       ├── KnightVarietySpriteSheet.png
│       ├── platform-atlas.xml
│       ├── platformSpriteSheet2.png
│       ├── platformSpritesheet.png
│       ├── SpellsSpriteSheet.png
│       ├── spirit-atlas.xml
│       └── tutorial-platform-atlas.xml
│
├── Controllers/
│   ├── KeyboardBindings.cs
│   ├── KeyboardController.cs
│   └── MouseController.cs
│
├── Enemy_Classes/
│   ├── Crawlid.cs
│   ├── EnemyProjectile.cs
│   └── Vengefly.cs
│
├── Enviroment_Classes/               ← typo in folder name
│   ├── CeilingSpike.cs              ← near-duplicate of Spike/FloorSpike
│   ├── FloorSpike.cs                ← near-duplicate of Spike/CeilingSpike
│   ├── Path_1.cs                    ← near-duplicate of Path_2/3
│   ├── Path_2.cs                    ← near-duplicate of Path_1/3
│   ├── Path_3.cs                    ← near-duplicate of Path_1/2
│   ├── Path_Ledge.cs
│   ├── Spike.cs                     ← near-duplicate of Floor/CeilingSpike
│   ├── Tutorial_Platform_1.cs       ← 10 near-identical files
│   ├── Tutorial_Platform_2.cs
│   ├── Tutorial_Platform_3.cs
│   ├── Tutorial_Platform_4.cs
│   ├── Tutorial_Platform_5.cs
│   ├── Tutorial_Platform_6.cs
│   ├── Tutorial_Platform_7.cs
│   ├── Tutorial_Platform_8.cs
│   ├── Tutorial_Platform_9.cs
│   └── Tutorial_Platform_10.cs
│
├── Factories/
│   └── SpriteFactory.cs
│
├── Graphics/
│   ├── TextureAtlas.cs
│   └── TextureRegion.cs
│
├── Interfaces/
│   ├── ICollidable.cs               ← DEAD (duplicate, contains ICollidableTEMP)
│   ├── ICommand.cs
│   ├── IController.cs
│   ├── IEnemy.cs
│   ├── IHollowKnight.cs
│   ├── IObjects.cs
│   ├── IPickup.cs
│   └── ISprite.cs
│
├── Pathfinding/
│   ├── AStarPathFinder.cs
│   └── NavigationGrid.cs
│
├── Player/
│   ├── Camera.cs
│   ├── KnightProjectile.cs
│   ├── KnightSpriteType.cs
│   ├── SwordHitbox.cs
│   └── TheKnight.cs                 ← god class (436 lines)
│
├── Projectiles/
│   ├── Projectile.cs
│   ├── ProjectileManager.cs
│   └── ProjectileSpawner.cs
│
├── Rooms/
│   └── RoomManager.cs
│
├── Shared/
│   └── Direction.cs
│
├── Sprites/
│   ├── AnimatedSprite.cs
│   ├── MovingAnimatedSprite.cs
│   ├── MovingSprite.cs
│   ├── StaticSprite.cs
│   └── TextSprite.cs
│
├── StateMachines/
│   ├── CrawlidStateMachine.cs
│   └── VengeFlyStateMachine.cs
│
├── Storage/
│   └── ItemManager.cs
│
└── Code Reviews and Guidlines/
    ├── Code_Review_Report .pdf
    └── Team_Process_Management (1).pdf
```

**Current totals:** 83 source files, 21 directories, 18 files in `Enviroment_Classes/` alone

---

## Appendix B — Proposed Directory Structure

What the project should look like after cleanup. Dead files removed, duplicates consolidated, god classes split, folders renamed, misplaced files moved. New `Audio/` directory stubbed for the next sprint.

```
HollowKnight/
├── Game1.cs                          (slimmed — thin orchestrator, <150 lines)
├── Program.cs
├── HollowKnight.csproj
├── HollowKnight.sln
├── app.manifest
├── Icon.bmp
├── Icon.ico
│
├── Audio/                            ★ NEW — next sprint
│   ├── AudioManager.cs               (central sound system: load, play, stop, volume)
│   └── SoundEffects.cs               (enum or constants for all SFX/music IDs)
│
├── Collision/
│   ├── CollisionDetector.cs
│   ├── CollisionGroupBuilder.cs
│   ├── CollisionHandler.cs
│   ├── CollisionManager.cs           (merged — single source of truth, absorbs CollisionResponses)
│   ├── CollisionSide.cs
│   ├── CollisionSystem.cs            ★ NEW — extracted from Game1 (handler registration + per-frame detection)
│   ├── DebugRenderer.cs
│   └── ICollidable.cs
│
├── Commands/
│   ├── ChangeNextEnemyCommand.cs
│   ├── ChangeNextEnvironmentCommand.cs    (typo fixed)
│   ├── ChangePreviousEnemyCommand.cs
│   ├── ChangePreviousEnvironmentCommand.cs (typo fixed)
│   ├── CycleItemNextCommand.cs
│   ├── CycleItemPreviousCommand.cs
│   ├── JumpToRoomCommand.cs
│   ├── PlayerDownSlashCommand.cs
│   ├── PlayerHealCancelCommand.cs
│   ├── PlayerHealHoldCommand.cs
│   ├── PlayerJumpCommand.cs
│   ├── PlayerMoveDownCommand.cs
│   ├── PlayerMoveLeftCommand.cs
│   ├── PlayerMoveRightCommand.cs
│   ├── PlayerMoveUpCommand.cs
│   ├── PlayerSideSlashCommand.cs
│   ├── PlayerStopMovingHorizontalCommand.cs
│   ├── PlayerStopMovingVerticalCommand.cs
│   ├── PlayerTakeDamageCommand.cs
│   ├── PlayerUpSlashCommand.cs
│   ├── PlayerUseItemCommand.cs
│   ├── QuitCommand.cs
│   ├── SwitchRoomCommand.cs
│   ├── ToggleGridCommand.cs
│   └── ToggleHitboxCommand.cs
│       (SetSpriteCommand.cs DELETED — was entirely dead code)
│
├── Content/
│   ├── Content.mgcb
│   ├── audio/                        ★ NEW — next sprint
│   │   └── (sound files: .wav, .ogg, etc.)
│   ├── fonts/
│   │   └── Credits.spritefont
│   ├── levels/
│   │   └── levelOne.xml
│   │       (levelLoader.cs MOVED out — see Levels/)
│   └── sprites/
│       ├── attack-atlas.xml
│       ├── enemy-atlas.xml
│       ├── EnemySpriteSheet.png
│       ├── HollowKnightSheet.png
│       ├── knight_abilities-atlas.xml
│       ├── knight_movement-atlas.xml
│       ├── KnightVarietySpriteSheet.png
│       ├── platform-atlas.xml
│       ├── platformSpriteSheet2.png
│       ├── platformSpritesheet.png
│       ├── SpellsSpriteSheet.png
│       ├── spirit-atlas.xml
│       └── tutorial-platform-atlas.xml
│
├── Controllers/
│   ├── KeyboardBindings.cs
│   ├── KeyboardController.cs
│   └── MouseController.cs
│
├── Enemies/                          (renamed from Enemy_Classes/)
│   ├── Crawlid.cs
│   ├── CrawlidStateMachine.cs        (moved from StateMachines/ — lives with its entity)
│   ├── EnemyProjectile.cs
│   ├── Vengefly.cs
│   └── VengeflyStateMachine.cs       (moved from StateMachines/ — lives with its entity)
│
├── Environment/                      (renamed from Enviroment_Classes/ — typo fixed)
│   ├── EnvironmentBase.cs            ★ NEW — abstract base for all env objects (shared Draw/Update/GetBounds/Bounds)
│   ├── TutorialPlatform.cs           ★ CONSOLIDATED — replaces 10 Tutorial_Platform_*.cs files
│   ├── Path.cs                       ★ CONSOLIDATED — replaces Path_1/2/3.cs
│   ├── PathLedge.cs                  (renamed from Path_Ledge.cs)
│   └── Spike.cs                      ★ CONSOLIDATED — replaces Spike/FloorSpike/CeilingSpike (variant param)
│
├── Factories/
│   └── SpriteFactory.cs              (cleaned — duplicate factory methods replaced with parameterized ones)
│
├── Graphics/
│   ├── DebugOverlay.cs               ★ NEW — extracted from Game1.Draw() (all debug drawing consolidated)
│   ├── TextureAtlas.cs
│   └── TextureRegion.cs
│
├── Interfaces/
│   ├── ICommand.cs
│   ├── IController.cs
│   ├── IEnemy.cs
│   ├── IPlayer.cs                    (renamed from IHollowKnight.cs — clearer name)
│   ├── IObjects.cs
│   ├── IPickup.cs
│   └── ISprite.cs
│       (ICollidable.cs DELETED — dead duplicate, real one is in Collision/)
│
├── Levels/
│   ├── LevelLoader.cs                (moved from Content/levels/ — source code doesn't belong in Content)
│   └── RoomManager.cs                (moved from Rooms/ — same domain)
│
├── Pathfinding/
│   ├── AStarPathFinder.cs
│   └── NavigationGrid.cs
│
├── Player/
│   ├── Camera.cs
│   ├── KnightCombat.cs               ★ NEW — extracted from TheKnight (attack states, cooldowns, sword hitbox)
│   ├── KnightHealth.cs               ★ NEW — extracted from TheKnight (HP, damage, invincibility, healing)
│   ├── KnightPhysics.cs              ★ NEW — extracted from TheKnight (velocity, gravity, grounding, knockback)
│   ├── KnightProjectile.cs
│   ├── KnightSpriteType.cs
│   ├── KnightState.cs                ★ NEW — enum replacing boolean flag soup
│   ├── SwordHitbox.cs
│   └── TheKnight.cs                  (slimmed — orchestrator only, <150 lines)
│
├── Projectiles/
│   ├── Projectile.cs
│   ├── ProjectileManager.cs
│   └── ProjectileSpawner.cs
│
├── Shared/
│   ├── Direction.cs
│   ├── GameConstants.cs              ★ NEW — screen dims, physics values, combat tuning
│   └── GameState.cs                  ★ NEW — enum: Playing, Paused, etc.
│
├── Sprites/
│   ├── AnimatedSprite.cs
│   ├── MovingAnimatedSprite.cs
│   ├── MovingSprite.cs
│   ├── StaticSprite.cs
│   └── TextSprite.cs
│
├── Storage/
│   └── ItemManager.cs
│
└── Docs/
    ├── Code_Review_Report.pdf        (moved from "Code Reviews and Guidlines/")
    └── Team_Process_Management.pdf   (moved, spaces removed from filename)
```

### What Changed — Summary

| Action | Count | Details |
|--------|-------|---------|
| **Files deleted** | 3 | `SetSpriteCommand.cs`, `Interfaces/ICollidable.cs`, `CollisionResponses.cs` |
| **Files consolidated** | 14 → 3 | 10 `Tutorial_Platform_*.cs` → 1, 3 `Path_*.cs` → 1, 3 spike files → 1 |
| **Files moved** | 4 | `levelLoader.cs` → `Levels/`, `RoomManager.cs` → `Levels/`, 2 state machines → `Enemies/` |
| **Files renamed** | 3 | `IHollowKnight` → `IPlayer`, 2 enviroment commands (typo fix) |
| **New files (from splits)** | 9 | `KnightPhysics`, `KnightCombat`, `KnightHealth`, `KnightState`, `CollisionSystem`, `DebugOverlay`, `EnvironmentBase`, `GameConstants`, `GameState` |
| **New files (audio stub)** | 2 | `AudioManager.cs`, `SoundEffects.cs` |
| **Directories removed** | 4 | `Enviroment_Classes/`, `Enemy_Classes/`, `StateMachines/`, `Rooms/`, `Code Reviews and Guidlines/`, `Builders/` |
| **Directories added/renamed** | 5 | `Environment/`, `Enemies/`, `Levels/`, `Audio/`, `Docs/` |
| **Net file count** | 83 → ~65 | Fewer files, each with one clear job |

### Directory Principles Going Forward
1. **No `_Classes` suffix** — the folder name is the domain, not a label
2. **State machines live with their entity** — not in a separate folder
3. **Source code never in `Content/`** — that's for assets only
4. **One folder per domain** — levels + rooms = `Levels/`, not two separate folders for 2 files
5. **Consolidate single-file folders** — `Builders/KnightSpriteBuilder.cs` can move into `Player/` or `Factories/` rather than having its own directory for one file
