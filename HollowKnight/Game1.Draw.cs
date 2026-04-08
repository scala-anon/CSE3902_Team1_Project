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
        _spriteBatch.Begin(transformMatrix: _camera.GetTransform());

        DrawKnight();
        DrawPlatforms();
        DrawEnemies();
        DrawItems();
        DrawDebugOverlay();

        _spriteBatch.End();
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