using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;

namespace HollowKnight;

public partial class Game1
{
    private void UpdateControllers(GameTime gameTime)
    {
        foreach (IController controller in _controllerList)
        {
            controller.Update(gameTime);
        }
    }

    private void UpdatePlaying(GameTime gameTime)
    {
        UpdateKnight(gameTime);
        UpdateRoom(gameTime);
        UpdateCollisions();
        UpdateEnemies(gameTime);
        UpdateKnightProjectiles(gameTime);
        UpdatePlatforms(gameTime);
        UpdateProjectiles(gameTime);
    }

    private void UpdatePaused(GameTime gameTime)
    {
    }

    private void UpdateInventory(GameTime gameTime)
    {
    }

    private void UpdateGameOver(GameTime gameTime)
    {
    }

    private void UpdateWin(GameTime gameTime)
    {
    }

    private void UpdateKnight(GameTime gameTime)
    {
        _knight.Update(gameTime);
    }

    private void UpdateRoom(GameTime gameTime)
    {
        _roomManager.Update(gameTime);
    }

    private void UpdateCollisions()
    {
        _collisionSystem.Update(
            _knight,
            _level.Platforms,
            _level.Enemies,
            _items,
            _projectileManager,
            _navigationGrid);
    }

    private void UpdateEnemies(GameTime gameTime)
    {
        foreach (IEnemy enemy in _level.Enemies)
        {
            enemy.Update(gameTime);
        }
    }

    private void UpdateKnightProjectiles(GameTime gameTime)
    {
        _knightProjectile.Update(gameTime);
    }

    private void UpdatePlatforms(GameTime gameTime)
    {
        foreach (IObject platform in _level.Platforms)
        {
            if (platform != null)
            {
                platform.Update(gameTime);
            }
        }
    }

    private void UpdateProjectiles(GameTime gameTime)
    {
        _projectileManager.Update(gameTime);
    }
}