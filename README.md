# Hollow Knight Clone

A 2D action-adventure game inspired by Hollow Knight, built with MonoGame (.NET 9).

## About

Explore a dark, atmospheric world filled with challenging enemies and hidden secrets. This game was created as a project for CSE3902 at The Ohio State University.

## How to Play

### Controls

| Key | Action |
|-----|--------|
| A / Left Arrow | Move left |
| D / Right Arrow | Move right |
| W / Up Arrow | Look up |
| Space | Jump (release for short hop) |
| C | Dash |
| Z | Side slash attack |
| Z + W/Up | Up slash attack |
| Z + S/Down | Down slash (airborne only) |
| X (hold) | Heal (consumes soul, must be grounded) |
| T | Cast Vengeful Spirit spell (consumes soul) |
| M | Toggle mute |
| P | Pause |
| Tab | Inventory |
| Q | Quit |

### Debug Controls

| Key | Action |
|-----|--------|
| E | Take damage (debug) |
| Y | Give full soul (debug) |
| H | Toggle hitbox display |
| G | Toggle navigation grid |
| F1/F2/F3 | Jump to room 1/2/3 |
| Ctrl + Left/Right | Switch room |
| F5 | Game over screen |
| F6 | Win screen |
| Mouse click | Teleport to room (debug) |

### Gameplay

- **Health**: The knight starts with 5 health masks. Taking damage from enemies or spikes removes 1 mask. At 0 health, the knight respawns at the last bench.
- **Soul**: Gained by hitting enemies. Used to heal (33 soul) or cast Vengeful Spirit (36 soul).
- **Dash**: Press C to dash. Has a cooldown on the ground and one use per jump in the air.
- **Combat**: Side slash, up slash, and down slash (air only). Each hit on an enemy grants soul.
- **Enemies**: Crawlids patrol platforms. Vengeflies detect and chase using A* pathfinding. Mantis Lord boss (WIP).

## Architecture

### Project Structure

```
HollowKnight/
  Enemies/         BaseEnemy abstract class + Crawlid, Vengefly, MantisLord
  Environment/     BaseEnvironmentObject + platforms, spikes, grass, doors, walls
  Player/          TheKnight + KnightPhysics, KnightCombat, KnightHealth, KnightDash
  Audio/           AudioManager, AudioLoader (XML-driven), SoundEffects
  Collision/       CollisionSystem, CollisionManager, CollisionDetector
  Commands/        Command pattern for input binding
  Controllers/     Keyboard and mouse input
  Factories/       SpriteFactory (centralized sprite creation)
  Graphics/        TextureAtlas, DebugOverlay, DebugRenderer
  Interfaces/      IEnemy, IObject, IPlayer, ISprite, ICommand, etc.
  Levels/          LevelLoader (XML-driven level loading), RoomManager
  Pathfinding/     A* pathfinder, NavigationGrid
  Projectiles/     ProjectileManager, VengefulSpiritProjectile
  Shared/          GameConstants, KnightConstants, EnemyConstants, CollisionConstants
  Sprites/         AnimatedSprite, StaticSprite, MovingSprite, TextSprite
  Content/         Sprite sheets, atlases, audio files, level XMLs, fonts
```

### Key Design Patterns

- **Abstract base classes**: `BaseEnemy` and `BaseEnvironmentObject` eliminate duplicated health/damage/draw/bounds logic across all enemies and environment objects
- **Command pattern**: All input is routed through `ICommand` implementations bound in `KeyboardBindings`
- **Factory pattern**: `SpriteFactory` singleton handles all sprite creation from atlas data
- **State machines**: `CrawlidStateMachine`, `VengeflyStateMachine` manage enemy AI states
- **Component decomposition**: `TheKnight` delegates to `KnightPhysics`, `KnightCombat`, `KnightHealth`, `KnightDash`
- **XML-driven data**: Levels, audio files, and sprite atlases are all loaded from XML

### Constants Organization

| File | Scope |
|------|-------|
| `KnightConstants` | Knight physics, combat, dash, soul, health, slash effects, projectiles |
| `EnemyConstants` | Shared enemy stats, per-enemy tuning (Crawlid, Vengefly), enemy projectiles |
| `CollisionConstants` | Hitbox dimensions, debug colors, spike collision boxes |
| `GameConstants` | Screen dimensions, level size, navigation, audio, physics thresholds |

## Known Bugs

- Vengefly A* pathfinding can cause lag at large chase radius (1200px)
- Mantis Lord boss AI not yet implemented (sprite placeholder only)
- Some room transitions not yet functional

## Installation

### Requirements

- Windows, macOS, or Linux
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Download & Run

```bash
git clone git@github.com:scala-anon/CSE3902_Team1_Project.git
cd CSE3902_Team1_Project/HollowKnight
dotnet run
```

## Tools & Processes

- **Framework**: MonoGame (DesktopGL)
- **Language**: C# / .NET 9
- **Version Control**: Git with feature branch workflow
- **Code Reviews**: Pull requests with at least one reviewer
- **IDE**: Visual Studio / VS Code / JetBrains Rider

## Credits

### Development Team

- Nicholas Mamais (PM)
- Srinivas Sankaranarayanan
- Zachary Verley
- Sudhish Kumar Gopalakrishnan
- Thomas Sobodosh

### Assets

- Sprite assets extracted/adapted from Hollow Knight by Team Cherry
- Audio SFX sourced from Hollow Knight by Team Cherry
- Mantis Lords OST by Christopher Larkin

### Special Thanks

- Team Cherry for the original Hollow Knight inspiration
- CSE3902 course staff

## License

This project is for educational purposes only. All Hollow Knight assets are property of Team Cherry.
