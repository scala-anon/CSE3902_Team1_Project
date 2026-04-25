using Microsoft.Xna.Framework;
using HollowKnight.Environment;
using HollowKnight.Interfaces;
using HollowKnight.Audio;
using HollowKnight.Shared;

namespace HollowKnight;

public partial class Game1
{
    private void UpdateAudio()
    {
        AudioManager.Instance.Update();
    }

    private void UpdateControllers(GameTime gameTime)
    {
        foreach (IController controller in _controllerList)
        {
            controller.Update(gameTime);
        }
    }

internal void CheckTransitions()
{
    if (_isTransitioning) return;

    Rectangle knightRect = _knight.GetBounds()[0];
    foreach (var zone in _level.Transitions)
    {
        if (knightRect.Intersects(zone.Bounds))
        {
            _isTransitioning = true;

            Vector2 safeSpawn = _knight.position;
            if (zone.Bounds.Width > zone.Bounds.Height)
                safeSpawn.Y -= 100;
            else
                safeSpawn.X += _knight.position.X < zone.Bounds.Center.X ? -100 : 100;

            _roomEntryPoints[_currentRoom] = safeSpawn;
            int dest = zone.DestinationRoom;
            _fader.StartFadeOut(() => TransitionToRoom(dest));
            return;
        }
    }
}


    internal void UpdateKnight(GameTime gameTime)
    {
        _knight.Update(gameTime);
    }

    internal void UpdateRoom(GameTime gameTime)
    {
        _roomManager.Update(gameTime);
    }

    internal void UpdateCollisions()
    {
        _collisionSystem.Update(
            _knight,
            _level.Platforms,
            _level.Interactables,
            _level.Enemies,
            _items,
            _projectileManager,
            _navigationGrid
        );
    }

    internal bool KnightIsDead() => _knight.IsDead();

    internal void UpdateEnemies(GameTime gameTime)
    {
        foreach (IEnemy enemy in _level.Enemies)
        {
            enemy.Update(gameTime);
        }
        // BossFightController must update AFTER enemies so state reads are fresh.
        _level.BossFight?.Update(gameTime);
    }

    internal void UpdateKnightProjectiles(GameTime gameTime)
    {
        _knightProjectile.Update(gameTime);
    }

    internal void UpdatePlatforms(GameTime gameTime)
    {
        foreach (IObject platform in _level.Platforms)
        {
            if (platform != null)
            {
                platform.Update(gameTime);
            }
        }

        foreach (IInteractable interactable in _level.Interactables)
        {
            if (interactable != null)
            {
                interactable.Update(gameTime);
            }
        }
    }

    internal void UpdateProjectiles(GameTime gameTime)
    {
        _projectileManager.Update(gameTime);
    }


    internal void TryInteract()
{
    Rectangle knightBounds = _knight.GetBounds()[0];

    foreach (IInteractable interactable in _level.Interactables)
    {
        if (interactable == null ||
            !interactable.IsActive ||
            interactable.InteractionType != InteractionType.ButtonPress)
        {
            continue;
        }

        foreach (Rectangle rect in interactable.GetInteractionBounds())
        {
            if (!rect.Intersects(knightBounds))
                continue;

            if (interactable.IsInteractable(_knight))
            {
                DebugLogger.LogInteraction(interactable.GetType().Name, "ButtonPress", "Up/W");
                interactable.OnInteract(_knight);
                if (interactable is Bench)
                    _knight.SetBenchRoom(_currentRoom);
                return;
            }
        }
    }
}
}
