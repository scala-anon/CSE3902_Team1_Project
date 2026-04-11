using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Abilities;

namespace HollowKnight;

public partial class Game1
{
    internal void DrawPausedStateOverlay() => DrawCenteredOverlay(PauseTitle, PausePrompt, ScreenTint, Color.White);
    internal void DrawInventoryStateOverlay() => DrawCenteredOverlay(InventoryTitle, InventoryPrompt, InventoryTint, Color.White);
    internal void DrawGameOverStateOverlay() => DrawCenteredOverlay(GameOverTitle, GameOverPrompt, ScreenTint, Color.White);
    internal void DrawWinStateOverlay() => DrawCenteredOverlay(WinTitle, string.Empty, ScreenTint, Color.Yellow);

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
        if (_gameState is PlayingState) return;

        _spriteBatch.Begin();

        if (_gameState.ShowsHealthHud)
        {
            _healthHud.Draw(_spriteBatch, _knight.GetHealth(), _knight.GetMaxHealth());
            _soulHud.Draw(_spriteBatch, _knight.GetSoul(), _knight.GetMaxSoul());
        }

        _gameState.DrawOverlay(this, _spriteBatch);

        _spriteBatch.End();
    }

    private void DrawCenteredOverlay(string title, string prompt, Color tint, Color titleColor)
    {
        Rectangle overlayBounds = new(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight);
        _spriteBatch.Draw(_overlayPixel, overlayBounds, tint);

        Vector2 titleSize = _hudFont.MeasureString(title);
        Vector2 promptSize = _hudFont.MeasureString(prompt);
        Vector2 screenCenter = new(overlayBounds.Width / 2f, overlayBounds.Height / 2f);
        bool hasPrompt = !string.IsNullOrEmpty(prompt);
        float blockHeight = hasPrompt
            ? titleSize.Y + OverlayTextLineSpacing + promptSize.Y
            : titleSize.Y;
        float blockTop = screenCenter.Y - blockHeight / 2f;

        Vector2 titlePosition = new(
            screenCenter.X - titleSize.X / 2f,
            blockTop);

        _spriteBatch.DrawString(_hudFont, title, titlePosition, titleColor);

        if (hasPrompt)
        {
            Vector2 promptPosition = new(
                screenCenter.X - promptSize.X / 2f,
                blockTop + titleSize.Y + OverlayTextLineSpacing);
            _spriteBatch.DrawString(_hudFont, prompt!, promptPosition, Color.White);
        }
    }
}
