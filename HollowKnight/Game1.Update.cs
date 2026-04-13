using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using HollowKnight.Audio;

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
        Rectangle knightRect = _knight.GetBounds()[0];
        foreach (Rectangle t in _level.Transitions)
        {
            if (knightRect.Intersects(t))
            {
                TransitionToRoom(_currentRoom == 1 ? 2 : 1);
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
            _level.Enemies,
            _items,
            _projectileManager,
            _navigationGrid);
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
    }

    internal void UpdateProjectiles(GameTime gameTime)
    {
        _projectileManager.Update(gameTime);
    }
}
