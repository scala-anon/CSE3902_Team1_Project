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
        _spriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                transformMatrix: _camera.GetTransform());
        DrawBackgrounds();
        _spriteBatch.End();

        // Layer 2: Platforms, enemies, knight, projectiles, items
        _spriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                transformMatrix: _camera.GetTransform());
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
        if (_gameState == GameState.Playing) return;

        _spriteBatch.Begin();

        switch (_gameState)
        {
            case GameState.Paused:
                DebugRenderer.DrawOverlayText(_spriteBatch, "PAUSED", Vector2.Zero, Color.White, 3f);
                DebugRenderer.DrawOverlayText(_spriteBatch, "\n\n\nPress P to resume", Vector2.Zero, Color.Gray, 1f);
                break;

            case GameState.Inventory:
                DebugRenderer.DrawOverlayText(_spriteBatch, "INVENTORY", Vector2.Zero, Color.White, 3f);
                DebugRenderer.DrawOverlayText(_spriteBatch, "\n\n\nPress Tab to close", Vector2.Zero, Color.Gray, 1f);
                break;

            case GameState.GameOver:
                DebugRenderer.DrawOverlayText(_spriteBatch, "GAME OVER", Vector2.Zero, Color.Red, 3f);
                break;

            case GameState.Win:
                DebugRenderer.DrawOverlayText(_spriteBatch, "YOU WIN", Vector2.Zero, Color.Yellow, 3f);
                break;
        }

        _spriteBatch.End();
    }
}
