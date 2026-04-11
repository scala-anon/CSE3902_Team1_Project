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

    private void UpdatePlayingLogic(GameTime gameTime)
    {
        CheckTransitions();
        UpdateKnight(gameTime);
        UpdateRoom(gameTime);
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
        bool won = true;
        foreach(var enemy in _level.Enemies)
        {
            if(enemy.IsActive)
            {
                won = false;
            }
        } 
        if(won)
        {
            SetWin();
        }
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

    private void CheckTransitions()
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
}