using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Abilities;

namespace HollowKnight;

public partial class Game1
{
    private void DrawWorld()
    {
        // Layer 1: Backgrounds (behind everything, no interaction)
        _spriteBatch.Begin(transformMatrix: _camera.GetTransform());
        DrawBackgrounds();
        _spriteBatch.End();

        // Layer 2: Platforms, enemies, knight, projectiles, items
        _spriteBatch.Begin(transformMatrix: _camera.GetTransform());
        DrawPlatforms();
        DrawEnemies();
        DrawProjectiles();
        DrawItems();
        DrawKnight();
        DrawDebugOverlay();
        _spriteBatch.End();
    }

    private void DrawBackgrounds()
    {
        foreach (IObject bg in _level.Backgrounds)
        {
            bg.Draw(_spriteBatch, SpriteEffects.None);
        }
    }

    private void DrawKnight()
    {
        _knight.Draw(_spriteBatch);
    }

    private void DrawPlatforms()
    {
        foreach (IObject obj in _level.Platforms)
        {
            if (obj != null)
            {
                obj.Draw(_spriteBatch, SpriteEffects.None);
            }
        }
    }

    private void DrawEnemies()
    {
        foreach (IEnemy enemy in _level.Enemies)
        {
            enemy.Draw(_spriteBatch, SpriteEffects.None);
        }
    }

    private void DrawItems()
    {
        foreach (Spirit item in _items)
        {
            if (item.IsActive)
            {
                item.Draw(_spriteBatch);
            }
        }
    }

    private void DrawProjectiles()
    {
        foreach (var p in _projectileManager.All)
        {
            p.Draw(_spriteBatch, Direction.Right);
        }
    }

    private void DrawDebugOverlay()
    {
        _debugOverlay.Draw(
            _spriteBatch,
            _knight,
            _level.Platforms,
            _level.Enemies,
            _items,
            _projectileManager,
            _navigationGrid,
            _camera);
    }

    private void DrawOverlay()
    {
        _spriteBatch.Begin();

        switch (_gameState)
        {
            case GameState.Paused:
                DebugRenderer.DrawText(_spriteBatch, "PAUSED", new Vector2(350, 100), Color.White);
                break;

            case GameState.Inventory:
                DebugRenderer.DrawText(_spriteBatch, "INVENTORY", new Vector2(330, 100), Color.White);
                break;

            case GameState.GameOver:
                DebugRenderer.DrawText(_spriteBatch, "GAME OVER", new Vector2(320, 100), Color.Red);
                break;

            case GameState.Win:
                DebugRenderer.DrawText(_spriteBatch, "YOU WIN", new Vector2(340, 100), Color.Yellow);
                break;
        }

        _spriteBatch.End();
    }
}
