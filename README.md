# Hollow Knight Clone

A 2D action-adventure game inspired by *Hollow Knight*, built with MonoGame on .NET 9. Created as the term project for CSE 3902 at The Ohio State University.

## Features

- **Multi-room world** with parallax backgrounds, foreground parallax pillars, breakable walls and doors, and screen-fade transitions between rooms
- **Knight movement** — running, variable-height jumping, air dash with cooldown, sword combat (side, up, and down slashes)
- **Pogo bouncing** — down-slash on enemies or spikes propels the knight upward
- **Vengeful Spirit spell** — soul-fueled projectile that damages enemies
- **Healing at benches** — sit at a bench to set a checkpoint; heal in place using soul
- **Enemies** — patrolling Crawlids with edge and spike detection, A\*-pathfinding Vengeflies, and a three-phase Mantis Lords boss fight
- **Boss fight** — three Mantis Lords with state machines, melee attacks, and projectile attacks
- **Audio** — XML-driven sound effects and music with screen-aware playback (sounds outside the camera don't play)
- **Pickups** — soul orbs (Spirit) restore the soul gauge

## Controls

### Gameplay

| Key | Action |
|-----|--------|
| A or Left Arrow | Move left |
| D or Right Arrow | Move right |
| W or Up Arrow | Look up / interact |
| Space | Jump (release for short hop) |
| C | Dash (one air dash per jump, ground cooldown) |
| Z | Side slash |
| Z + W or Z + Up | Up slash |
| Z + S or Z + Down | Down slash (airborne; pogo on enemy or spike) |
| X (hold) | Heal — consumes 33 soul, must be grounded |
| F | Cast Vengeful Spirit — consumes 36 soul |
| 1 / 2 / 3 | Use inventory item in slot |
| U / I | Cycle inventory selection |

### System

| Key | Action |
|-----|--------|
| Enter | Start game (from title screen) |
| P | Pause |
| Tab | Inventory |
| R | Restart |
| M | Toggle mute |
| Q or Escape | Quit |

### Boss Fight

| Key | Action |
|-----|--------|
| L | Trigger Mantis Lords fight (when in boss room) |
| V / B / N | Cycle state of left / middle / right Mantis Lord (debug) |
| Shift + V / B / N | Kill the corresponding Mantis Lord (debug) |
| Ctrl + V / B / N | Freeze the corresponding Mantis Lord (debug) |
| Shift + F | Toggle fight pause (debug) |

### Debug

| Key | Action |
|-----|--------|
| E | Take damage |
| Y | Give full soul |
| H | Toggle hitbox display |
| G | Toggle navigation grid |
| K | Toggle godmode |
| F1 / F2 / F3 / F4 | Jump directly to room 1 / 2 / 3 / 4 |
| Ctrl + Left / Right | Switch to previous / next room |
| Ctrl + 1..4 | Jump to room (alternative) |
| Tilde (\`) | Toggle goofy SFX mode |

## Architecture

### Project Structure

```
HollowKnight/
  Abilities/        Spirit pickup
  Audio/            AudioManager, AudioLoader (XML-driven), SoundEffects
  Collision/        CollisionSystem, CollisionDetector, CollisionManager,
                    CollisionHandler, CollisionGroupBuilder, CollisionSide
  Commands/         Command-pattern implementations for input
  Content/          Sprite atlases, audio files, level XMLs, fonts
  Controllers/      KeyboardController, KeyboardBindings, MouseController
  Enemies/          BaseEnemy + Crawlid, Vengefly, MantisLord, BossFightController,
                    state machines, EnemyProjectile
  Environment/      BaseEnvironmentObject + platforms, spikes, grass, doors,
                    breakable walls, benches, bricks, flags, pillars, transition zones
  Factories/        SpriteFactory (singleton, atlas-driven sprite creation)
  Graphics/         TextureAtlas, ParallaxBackground, ScreenFader,
                    DebugOverlay, DebugRenderer
  Hud/              HealthHud, SoulHud, inventory and title screen rendering
  Interfaces/       IEnemy, IObject, IPlayer, ISprite, ICommand, IInteractable,
                    IBreakable, IPickup, ICollidable
  Levels/           LevelLoader (XML-driven), RoomManager
  Pathfinding/      A* pathfinder, NavigationGrid
  Player/           TheKnight + KnightPhysics, KnightCombat, KnightHealth,
                    KnightDash, KnightProjectile, Camera, SwordHitbox
  Projectiles/      ProjectileManager, ProjectileSpawner, MantisLordProjectile,
                    DashEffect, LowHealthEffect
  Shared/           GameConstants, KnightConstants, EnemyConstants,
                    CollisionConstants, CameraConstants, EnvironmentConstants,
                    HudConstants, GameState, DebugLogger
  Sprites/          AnimatedSprite, StaticSprite, MovingSprite,
                    MovingAnimatedSprite, TextSprite
  Storage/          Persistence helpers (item manager, etc.)
  Game1.cs          Partial: fields and entry point
  Game1.Initialize.cs  Partial: setup, level loading, room transitions
  Game1.Update.cs   Partial: per-frame update, transition checks
  Game1.Draw.cs     Partial: rendering
  Game1.State.cs    Partial: pause / inventory / game-state transitions
  Game1.Hud.cs      Partial: HUD rendering
```

### Design Patterns

- **Command pattern** — all input is routed through `ICommand` implementations bound in `KeyboardBindings`
- **Factory pattern** — `SpriteFactory` singleton handles all sprite creation from atlas data
- **State machines** — `CrawlidStateMachine`, `VengeflyStateMachine`, `MantisLordStateMachine` manage enemy AI
- **Component decomposition** — `TheKnight` delegates to `KnightPhysics`, `KnightCombat`, `KnightHealth`, `KnightDash`
- **Abstract base classes** — `BaseEnemy` and `BaseEnvironmentObject` deduplicate health, draw, and bounds logic
- **XML-driven data** — levels, audio definitions, and sprite atlases all load from XML

### Constants

All numeric tuning lives in `Shared/`. No magic numbers in gameplay code.

| File | Scope |
|------|-------|
| `KnightConstants` | Knight physics, combat, dash, soul, health, slash effects, projectiles, pogo |
| `EnemyConstants` | Shared enemy stats, per-enemy tuning, mantis throne offsets, projectile velocities |
| `CollisionConstants` | Hitbox dimensions, debug colors, collision rectangles |
| `GameConstants` | Screen dimensions, level size, navigation grid, transition zones, layer depths, sprite opacity defaults |
| `EnvironmentConstants` | Breakable hit counts, pillar tints, environmental tuning |
| `CameraConstants` | Camera lerp and follow tuning |
| `HudConstants` | HUD layout offsets and dimensions |

## Build and Run

### Requirements

- .NET 9.0 SDK
- Windows, macOS, or Linux

### Commands

```bash
git clone https://github.com/scala-anon/CSE3902_Team1_Project.git
cd CSE3902_Team1_Project
dotnet build HollowKnight/HollowKnight.csproj
dotnet run --project HollowKnight
```

## Development Workflow

- **Integration branch:** `dev` — all feature work merges here
- **Branch naming:** `feature/<kebab-case>`, `fix/<kebab-case>`, `hotfix/<kebab-case>`
- **Pull requests:** require at least one review before merging into `dev`
- **No magic numbers:** all numeric values live in the appropriate `*Constants.cs` file
- **Namespaces required** on every file

## Known Issues

- Vengefly A\* pathfinding can produce frame drops at large chase radii
- Some asset loading paths are case-sensitive on Linux

## Team

- Nicholas Mamais (Project Manager)
- Srinivas Sankaranarayanan
- Zachary Verley
- Sudhish Kumar Gopalakrishnan
- Thomas Sobodosh

## Credits

- Sprite assets adapted from *Hollow Knight* by Team Cherry
- Audio sourced from *Hollow Knight* by Team Cherry
- Boss music: *Mantis Lords* by Christopher Larkin
- CSE 3902 course staff at The Ohio State University

## License

Educational use only. All *Hollow Knight* assets are property of Team Cherry.
