using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Abilities;
using HollowKnight.Storage;
using System;
using Microsoft.Xna.Framework.Input;

namespace HollowKnight;

public partial class Game1
{
    internal void DrawTitleStateOverlay() => DrawTitleScreen();
    internal void DrawPausedStateOverlay() => DrawPauseScreen();
    internal void DrawInventoryStateOverlay() => DrawInventoryOverlay();
    internal void DrawGameOverStateOverlay() => DrawGameOverScreen();
    internal void DrawWinStateOverlay() => DrawCenteredOverlay(WinTitle, string.Empty, HudConstants.ScreenTint, Color.Yellow);

    private void DrawWorld()
    {
        // Single world-space batch. FrontToBack sort:
        // layerDepth 0.0 = farthest back, 1.0 = frontmost.
        _spriteBatch.Begin(
                sortMode: SpriteSortMode.FrontToBack,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                transformMatrix: Camera.Instance.GetTransform());

        DrawBackgrounds(GameConstants.LayerDepthBackgroundFar);
        DrawBackgroundMid(GameConstants.LayerDepthBackgroundMid);
        DrawPlatforms(GameConstants.LayerDepthPlatform);
        DrawInteractables(GameConstants.LayerDepthInteractable);
        DrawItems(GameConstants.LayerDepthItem);
        DrawEnemies(GameConstants.LayerDepthEnemy);
        DrawProjectiles(GameConstants.LayerDepthProjectile);
        DrawKnight(GameConstants.LayerDepthKnight);
        DrawForeground(GameConstants.LayerDepthForeground);
        DrawDebugOverlay();

        _spriteBatch.End();
    }

    private void DrawInteractables(float layerDepth)
    {
        int i = 0;
        foreach (IInteractable interactable in _level.Interactables)
        {
            if(interactable != null)
            {
                interactable.Draw(_spriteBatch, SpriteEffects.None, layerDepth + i * GameConstants.LayerDepthEpsilon);
                i++;
            }
        }
    }

    private void DrawBackgrounds(float layerDepth)
    {
        int i = 0;
        foreach (IObject bg in _level.Backgrounds)
        {
            bg.Draw(_spriteBatch, SpriteEffects.None, layerDepth + i * GameConstants.LayerDepthEpsilon);
            i++;
        }
    }

    private void DrawBackgroundMid(float layerDepth)
    {
        int i = 0;
        foreach (IObject obj in _level.BackgroundMid)
        {
            obj?.Draw(_spriteBatch, SpriteEffects.None, layerDepth + i * GameConstants.LayerDepthEpsilon);
            i++;
        }
    }

    private void DrawForeground(float layerDepth)
    {
        int i = 0;
        foreach (IObject obj in _level.Foreground)
        {
            obj?.Draw(_spriteBatch, SpriteEffects.None, layerDepth + i * GameConstants.LayerDepthEpsilon);
            i++;
        }
    }

    private void DrawKnight(float layerDepth)
    {
        _knight.Draw(_spriteBatch, layerDepth);
    }

    private void DrawPlatforms(float layerDepth)
    {
        int i = 0;
        foreach (IObject obj in _level.Platforms)
        {
            if (obj != null)
            {
                obj.Draw(_spriteBatch, SpriteEffects.None, layerDepth + i * GameConstants.LayerDepthEpsilon);
                i++;
            }
        }
    }

    private void DrawEnemies(float layerDepth)
    {
        int i = 0;
        foreach (IEnemy enemy in _level.Enemies)
        {
            enemy.Draw(_spriteBatch, SpriteEffects.None, layerDepth + i * GameConstants.LayerDepthEpsilon);
            i++;
        }
    }

    private void DrawItems(float layerDepth)
    {
        int i = 0;
        foreach (Spirit item in _items)
        {
            if (item.IsActive)
            {
                item.Draw(_spriteBatch, layerDepth + i * GameConstants.LayerDepthEpsilon);
                i++;
            }
        }
    }

    private void DrawProjectiles(float layerDepth)
    {
        int i = 0;
        foreach (var p in _projectileManager.All)
        {
            p.Draw(_spriteBatch, Direction.Right, layerDepth + i * GameConstants.LayerDepthEpsilon);
            i++;
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
            _level.Interactables,
            _level.Transitions);
    }

    private void DrawHud()
    {
        if (!_gameState.ShowsHealthHud) return;

        _spriteBatch.Begin();
        _healthHud.Draw(_spriteBatch, _knight.GetHealth(), _knight.GetMaxHealth());
        _soulHud.Draw(_spriteBatch, _knight.GetSoul(), _knight.GetMaxSoul());
        _spriteBatch.End();
    }

    private void DrawOverlay()
    {
        if (_gameState is PlayingState) return;

        _spriteBatch.Begin();
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
            ? titleSize.Y + HudConstants.OverlayTextLineSpacing + promptSize.Y
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
                blockTop + titleSize.Y + HudConstants.OverlayTextLineSpacing);
            _spriteBatch.DrawString(_hudFont, prompt!, promptPosition, Color.White);
        }
    }

    private void DrawInventoryOverlay()
    {
        Rectangle overlayBounds = GetOverlayBounds();
        MouseState mouseState = Mouse.GetState();
        _spriteBatch.Draw(_overlayPixel, overlayBounds, HudConstants.InventoryTint);
        DrawInventoryBackdrop(overlayBounds);
        DrawReferenceInventoryLayout(overlayBounds);
        Rectangle backButtonBounds = GetInventoryBackButtonBounds(overlayBounds);
        DrawInventoryBackButton(backButtonBounds, backButtonBounds.Contains(mouseState.Position));
    }

    internal bool TryActivateTitleButton(Point mousePosition)
    {
        if (!IsTitleScreenOpen())
        {
            return false;
        }

        if (GetTitleButtonBounds(0).Contains(mousePosition))
        {
            StartGame();
            return true;
        }

        if (GetTitleButtonBounds(1).Contains(mousePosition))
        {
            Exit();
            return true;
        }

        return false;
    }

    internal bool TryActivatePauseButton(Point mousePosition)
    {
        if (_gameState is not PausedState)
        {
            return false;
        }

        if (GetPauseButtonBounds(0).Contains(mousePosition))
        {
            TogglePause();
            return true;
        }

        if (GetPauseButtonBounds(1).Contains(mousePosition))
        {
            ResetGame();
            return true;
        }

        if (GetPauseButtonBounds(2).Contains(mousePosition))
        {
            Exit();
            return true;
        }

        return false;
    }

    internal bool TryActivateGameOverButton(Point mousePosition)
    {
        if (_gameState is not GameOverState)
        {
            return false;
        }

        if (GetGameOverButtonBounds(0).Contains(mousePosition))
        {
            ResetGame();
            return true;
        }

        if (GetGameOverButtonBounds(1).Contains(mousePosition))
        {
            Exit();
            return true;
        }

        return false;
    }

    internal bool TrySelectInventoryItem(Point mousePosition)
    {
        if (_gameState is not InventoryState)
        {
            return false;
        }

        if (GetInventoryBackButtonBounds(GetOverlayBounds()).Contains(mousePosition))
        {
            ToggleInventory();
            return true;
        }

        for (int i = 0; i < ItemManager.Items.Count; i++)
        {
            Rectangle itemBounds = GetInventoryItemBounds(i);
            if (!itemBounds.Contains(mousePosition))
            {
                continue;
            }

            ItemManager.SelectItem(i);
            return true;
        }

        return false;
    }

    private Rectangle GetOverlayBounds()
    {
        return new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight);
    }

    private void DrawReferenceInventoryLayout(Rectangle overlayBounds)
    {
        DrawInventoryHeaderOrnaments(overlayBounds);
        DrawInventoryBodyFrame(overlayBounds);

        Vector2 titleSize = _hudFont.MeasureString(InventoryTitle);
        Vector2 titlePosition = new(
            overlayBounds.Center.X - (titleSize.X * HudConstants.InventoryTitleScale) / 2f,
            HudConstants.InventoryTitleY);
        _spriteBatch.DrawString(_hudFont, InventoryTitle, titlePosition, Color.White, 0f, Vector2.Zero, HudConstants.InventoryTitleScale, SpriteEffects.None, 0f);

        Rectangle centerBounds = GetReferenceInventoryBounds(overlayBounds);
        DrawFramedPanel(centerBounds, HudConstants.InventoryPanelFillColor, HudConstants.InventoryPanelBorderColor, HudConstants.InventoryPanelBorderThickness);

        for (int i = 0; i < ItemManager.Items.Count; i++)
        {
            Rectangle itemBounds = GetInventoryItemBounds(i);
            DrawInventoryItemBox(itemBounds, ItemManager.Items[i], i == ItemManager.CurrentIndex, i + 1);
        }
    }

    private Rectangle GetInventoryItemBounds(int index)
    {
        return GetInventoryGridCellBounds(index);
    }

    private Rectangle GetInventoryGridCellBounds(int index)
    {
        Rectangle overlayBounds = GetOverlayBounds();
        Rectangle centerBounds = GetReferenceInventoryBounds(overlayBounds);
        int visibleRows = Math.Min(HudConstants.InventoryGridMaxRows, Math.Max(1, (int)Math.Ceiling(ItemManager.Items.Count / (double)HudConstants.InventoryGridColumns)));
        int availableWidth = centerBounds.Width - (HudConstants.InventoryGridOuterPadding * 2) - ((HudConstants.InventoryGridColumns - 1) * HudConstants.InventoryGridCellSpacing);
        int availableHeight = centerBounds.Height - (HudConstants.InventoryGridOuterPadding * 2) - ((visibleRows - 1) * HudConstants.InventoryGridCellSpacing);
        int cellSize = Math.Min(availableWidth / HudConstants.InventoryGridColumns, availableHeight / visibleRows);
        int totalWidth = (cellSize * HudConstants.InventoryGridColumns) + ((HudConstants.InventoryGridColumns - 1) * HudConstants.InventoryGridCellSpacing);
        int totalHeight = (cellSize * visibleRows) + ((visibleRows - 1) * HudConstants.InventoryGridCellSpacing);
        int startX = centerBounds.Center.X - (totalWidth / 2);
        int startY = centerBounds.Y + HudConstants.InventoryGridOuterPadding;
        int row = index / HudConstants.InventoryGridColumns;
        int column = index % HudConstants.InventoryGridColumns;

        return new Rectangle(
            startX + (column * (cellSize + HudConstants.InventoryGridCellSpacing)),
            startY + (row * (cellSize + HudConstants.InventoryGridCellSpacing)),
            cellSize,
            cellSize);
    }

    private void DrawInventoryItemBox(Rectangle bounds, string itemName, bool isSelected, int itemNumber)
    {
        Color fillColor = HudConstants.InventoryBoxFillColor;
        Color borderColor = isSelected ? HudConstants.InventorySelectedBoxBorderColor : HudConstants.InventoryBoxBorderColor;
        _spriteBatch.Draw(_overlayPixel, bounds, fillColor);
        DrawPanelBorder(bounds, HudConstants.InventoryBoxBorderThickness, borderColor);

        Vector2 nameSize = _hudFont.MeasureString(itemName);
        float nameScale = itemName.Length > HudConstants.InventoryBoxLongNameThreshold ? HudConstants.InventoryBoxNameScaleLong : HudConstants.InventoryBoxNameScaleShort;
        Vector2 namePosition = new(
            bounds.Center.X - (nameSize.X * nameScale) / 2f,
            bounds.Center.Y - (nameSize.Y * nameScale) / 2f);
        _spriteBatch.DrawString(_hudFont, itemName, namePosition, Color.Black, 0f, Vector2.Zero, nameScale, SpriteEffects.None, 0f);
    }

    private void DrawInventoryBackdrop(Rectangle overlayBounds)
    {
        Rectangle vignetteTop = new(0, 0, overlayBounds.Width, HudConstants.InventoryBackdropTopHeight);
        Rectangle vignetteBottom = new(0, overlayBounds.Bottom - HudConstants.InventoryBackdropBottomHeight, overlayBounds.Width, HudConstants.InventoryBackdropBottomHeight);
        Rectangle leftEdge = new(0, 0, HudConstants.InventoryBackdropSideWidth, overlayBounds.Height);
        Rectangle rightEdge = new(overlayBounds.Right - HudConstants.InventoryBackdropSideWidth, 0, HudConstants.InventoryBackdropSideWidth, overlayBounds.Height);

        _spriteBatch.Draw(_overlayPixel, vignetteTop, HudConstants.InventoryBackdropTopColor);
        _spriteBatch.Draw(_overlayPixel, vignetteBottom, HudConstants.InventoryBackdropBottomColor);
        _spriteBatch.Draw(_overlayPixel, leftEdge, HudConstants.InventoryBackdropSideColor);
        _spriteBatch.Draw(_overlayPixel, rightEdge, HudConstants.InventoryBackdropSideColor);
    }

    private void DrawInventoryHeaderOrnaments(Rectangle overlayBounds)
    {
        Rectangle leftLine = new(overlayBounds.Center.X - HudConstants.InventoryHeaderLineOffsetLeft, HudConstants.InventoryHeaderLineY, HudConstants.InventoryHeaderLineWidth, HudConstants.InventoryHeaderLineHeight);
        Rectangle rightLine = new(overlayBounds.Center.X + HudConstants.InventoryHeaderLineOffsetRight, HudConstants.InventoryHeaderLineY, HudConstants.InventoryHeaderLineWidth, HudConstants.InventoryHeaderLineHeight);
        Rectangle leftCurl = new(overlayBounds.Center.X - HudConstants.InventoryHeaderCurlLeftOffset, HudConstants.InventoryHeaderCurlY, HudConstants.InventoryHeaderCurlWidth, HudConstants.InventoryHeaderCurlHeight);
        Rectangle rightCurl = new(overlayBounds.Center.X + HudConstants.InventoryHeaderCurlRightOffset, HudConstants.InventoryHeaderCurlY, HudConstants.InventoryHeaderCurlWidth, HudConstants.InventoryHeaderCurlHeight);
        Rectangle lowerFlourish = new(overlayBounds.Center.X - HudConstants.InventoryHeaderLowerFlourishOffset, HudConstants.InventoryHeaderLowerFlourishY, HudConstants.InventoryHeaderLowerFlourishWidth, HudConstants.InventoryHeaderLowerFlourishHeight);

        _spriteBatch.Draw(_overlayPixel, leftLine, Color.White);
        _spriteBatch.Draw(_overlayPixel, rightLine, Color.White);
        _spriteBatch.Draw(_overlayPixel, leftCurl, HudConstants.InventoryHeaderCurlColor);
        _spriteBatch.Draw(_overlayPixel, rightCurl, HudConstants.InventoryHeaderCurlColor);
        _spriteBatch.Draw(_overlayPixel, lowerFlourish, Color.White);
    }

    private void DrawInventoryBodyFrame(Rectangle overlayBounds)
    {
        Rectangle centerBounds = GetReferenceInventoryBounds(overlayBounds);
        Rectangle leftRail = new(centerBounds.X, centerBounds.Y, HudConstants.InventoryBodyRailWidth, centerBounds.Height);
        Rectangle rightRail = new(centerBounds.Right - HudConstants.InventoryBodyRailWidth, centerBounds.Y, HudConstants.InventoryBodyRailWidth, centerBounds.Height);
        Rectangle bottomFlourish = new(centerBounds.Center.X - HudConstants.InventoryBodyBottomFlourishHalfWidth, centerBounds.Bottom + HudConstants.InventoryBodyBottomFlourishY, HudConstants.InventoryBodyBottomFlourishWidth, HudConstants.InventoryBodyBottomFlourishHeight);
        Rectangle bottomCenter = new(centerBounds.Center.X - HudConstants.InventoryBodyBottomCenterHalfWidth, centerBounds.Bottom + HudConstants.InventoryBodyBottomCenterY, HudConstants.InventoryBodyBottomCenterWidth, HudConstants.InventoryBodyBottomCenterHeight);

        _spriteBatch.Draw(_overlayPixel, leftRail, HudConstants.InventoryBodyRailColor);
        _spriteBatch.Draw(_overlayPixel, rightRail, HudConstants.InventoryBodyRailColor);
        _spriteBatch.Draw(_overlayPixel, bottomFlourish, Color.White);
        _spriteBatch.Draw(_overlayPixel, bottomCenter, HudConstants.InventoryBodyBottomCenterColor);
    }

    private Rectangle GetInventoryBackButtonBounds(Rectangle overlayBounds)
    {
        return new Rectangle(
            overlayBounds.Right - HudConstants.InventoryBackButtonRightMargin - HudConstants.InventoryBackButtonWidth,
            overlayBounds.Bottom - HudConstants.InventoryBackButtonBottomMargin - HudConstants.InventoryBackButtonHeight,
            HudConstants.InventoryBackButtonWidth,
            HudConstants.InventoryBackButtonHeight);
    }

    private void DrawInventoryBackButton(Rectangle bounds, bool isHovered)
    {
        DrawShadow(bounds, 6, 6);
        _spriteBatch.Draw(_overlayPixel, bounds, isHovered ? HudConstants.TitleButtonHoverColor : HudConstants.TitleButtonColor);
        DrawPanelBorder(bounds, 2, HudConstants.InventorySelectedBoxBorderColor);
        DrawCenteredScaledText(InventoryBackLabel, new Vector2(bounds.Center.X, bounds.Center.Y), HudConstants.InventoryBackButtonScale, HudConstants.TitleButtonTextColor);
    }

    private static Rectangle GetReferenceInventoryBounds(Rectangle overlayBounds)
    {
        return new Rectangle(
            (overlayBounds.Width / 2) - HudConstants.InventoryPanelHalfWidth,
            HudConstants.InventoryPanelTop,
            HudConstants.InventoryPanelWidth,
            overlayBounds.Height - HudConstants.InventoryPanelBottomMargin);
    }

    private void DrawTitleScreen()
    {
        Rectangle overlayBounds = GetOverlayBounds();
        MouseState mouseState = Mouse.GetState();
        _spriteBatch.Draw(_overlayPixel, overlayBounds, HudConstants.TitleBackdropTint);

        DrawCenteredScaledText(LandingTitle, new Vector2(overlayBounds.Center.X, overlayBounds.Center.Y - 118), HudConstants.TitleScale, HudConstants.TitleAccentColor);

        Rectangle startButtonBounds = GetTitleButtonBounds(0);
        Rectangle quitButtonBounds = GetTitleButtonBounds(1);
        DrawTitleButton(startButtonBounds, StartGameLabel, startButtonBounds.Contains(mouseState.Position));
        DrawTitleButton(quitButtonBounds, QuitGameLabel, quitButtonBounds.Contains(mouseState.Position));
    }

    private void DrawPauseScreen()
    {
        Rectangle overlayBounds = GetOverlayBounds();
        MouseState mouseState = Mouse.GetState();
        _spriteBatch.Draw(_overlayPixel, overlayBounds, HudConstants.ScreenTint);

        DrawCenteredScaledText(PauseTitle, new Vector2(overlayBounds.Center.X, overlayBounds.Center.Y - 132), HudConstants.PauseTitleScale, Color.White);

        Rectangle continueButtonBounds = GetPauseButtonBounds(0);
        Rectangle restartButtonBounds = GetPauseButtonBounds(1);
        Rectangle quitButtonBounds = GetPauseButtonBounds(2);
        DrawPauseButton(continueButtonBounds, ContinueGameLabel, continueButtonBounds.Contains(mouseState.Position));
        DrawPauseButton(restartButtonBounds, RestartGameLabel, restartButtonBounds.Contains(mouseState.Position));
        DrawPauseButton(quitButtonBounds, QuitGameLabel, quitButtonBounds.Contains(mouseState.Position));
    }

    private void DrawGameOverScreen()
    {
        Rectangle overlayBounds = GetOverlayBounds();
        MouseState mouseState = Mouse.GetState();
        _spriteBatch.Draw(_overlayPixel, overlayBounds, HudConstants.ScreenTint);

        DrawCenteredScaledText(GameOverTitle, new Vector2(overlayBounds.Center.X, overlayBounds.Center.Y - 118), HudConstants.GameOverTitleScale, Color.White);

        Rectangle restartButtonBounds = GetGameOverButtonBounds(0);
        Rectangle quitButtonBounds = GetGameOverButtonBounds(1);
        DrawGameOverButton(restartButtonBounds, RestartGameLabel, restartButtonBounds.Contains(mouseState.Position));
        DrawGameOverButton(quitButtonBounds, QuitGameLabel, quitButtonBounds.Contains(mouseState.Position));
    }

    private Rectangle GetTitleButtonBounds(int index)
    {
        Rectangle overlayBounds = GetOverlayBounds();
        int buttonsTop = overlayBounds.Center.Y - 2;

        return new Rectangle(
            overlayBounds.Center.X - (HudConstants.TitleButtonWidth / 2),
            buttonsTop + (index * (HudConstants.TitleButtonHeight + HudConstants.TitleButtonSpacing)),
            HudConstants.TitleButtonWidth,
            HudConstants.TitleButtonHeight);
    }

    private Rectangle GetPauseButtonBounds(int index)
    {
        Rectangle overlayBounds = GetOverlayBounds();
        int buttonsTop = overlayBounds.Center.Y - 34;

        return new Rectangle(
            overlayBounds.Center.X - (HudConstants.PauseButtonWidth / 2),
            buttonsTop + (index * (HudConstants.PauseButtonHeight + HudConstants.PauseButtonSpacing)),
            HudConstants.PauseButtonWidth,
            HudConstants.PauseButtonHeight);
    }

    private Rectangle GetGameOverButtonBounds(int index)
    {
        Rectangle overlayBounds = GetOverlayBounds();
        int buttonsTop = overlayBounds.Center.Y - 8;

        return new Rectangle(
            overlayBounds.Center.X - (HudConstants.GameOverButtonWidth / 2),
            buttonsTop + (index * (HudConstants.GameOverButtonHeight + HudConstants.GameOverButtonSpacing)),
            HudConstants.GameOverButtonWidth,
            HudConstants.GameOverButtonHeight);
    }

    private void DrawTitleButton(Rectangle bounds, string label, bool isHovered)
    {
        DrawShadow(bounds, 6, 6);
        _spriteBatch.Draw(_overlayPixel, bounds, isHovered ? HudConstants.TitleButtonHoverColor : HudConstants.TitleButtonColor);
        DrawPanelBorder(bounds, 2, HudConstants.TitleAccentColor);
        DrawCenteredScaledText(label, new Vector2(bounds.Center.X, bounds.Center.Y), HudConstants.TitleButtonScale, HudConstants.TitleButtonTextColor);
    }

    private void DrawPauseButton(Rectangle bounds, string label, bool isHovered)
    {
        DrawShadow(bounds, 6, 6);
        _spriteBatch.Draw(_overlayPixel, bounds, isHovered ? HudConstants.TitleButtonHoverColor : HudConstants.TitleButtonColor);
        DrawPanelBorder(bounds, 2, HudConstants.TitleAccentColor);
        DrawCenteredScaledText(label, new Vector2(bounds.Center.X, bounds.Center.Y), HudConstants.PauseButtonScale, HudConstants.TitleButtonTextColor);
    }

    private void DrawGameOverButton(Rectangle bounds, string label, bool isHovered)
    {
        DrawShadow(bounds, 6, 6);
        _spriteBatch.Draw(_overlayPixel, bounds, isHovered ? HudConstants.TitleButtonHoverColor : HudConstants.TitleButtonColor);
        DrawPanelBorder(bounds, 2, HudConstants.TitleAccentColor);
        DrawCenteredScaledText(label, new Vector2(bounds.Center.X, bounds.Center.Y), HudConstants.GameOverButtonScale, HudConstants.TitleButtonTextColor);
    }

    private void DrawCenteredScaledText(string text, Vector2 center, float scale, Color color)
    {
        Vector2 size = _hudFont.MeasureString(text) * scale;
        Vector2 position = new(center.X - (size.X / 2f), center.Y - (size.Y / 2f));
        _spriteBatch.DrawString(_hudFont, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    private void DrawFramedPanel(Rectangle bounds, Color fillColor, Color borderColor, int borderThickness)
    {
        DrawShadow(bounds, 8, 8);
        _spriteBatch.Draw(_overlayPixel, bounds, fillColor);
        DrawPanelBorder(bounds, borderThickness, borderColor);

        Rectangle insetBounds = new(bounds.X + borderThickness + 2, bounds.Y + borderThickness + 2, Math.Max(1, bounds.Width - ((borderThickness + 2) * 2)), Math.Max(1, bounds.Height - ((borderThickness + 2) * 2)));
        if (insetBounds.Width > 0 && insetBounds.Height > 0)
        {
            _spriteBatch.Draw(_overlayPixel, insetBounds, HudConstants.InventoryPanelInsetHighlightColor);
        }
    }

    private void DrawShadow(Rectangle bounds, int offsetX, int offsetY)
    {
        Rectangle shadowBounds = new(bounds.X + offsetX, bounds.Y + offsetY, bounds.Width, bounds.Height);
        _spriteBatch.Draw(_overlayPixel, shadowBounds, HudConstants.InventoryShadowColor);
    }

    private void DrawPanelBorder(Rectangle bounds, int thickness, Color color)
    {
        _spriteBatch.Draw(_overlayPixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
        _spriteBatch.Draw(_overlayPixel, new Rectangle(bounds.X, bounds.Bottom - thickness, bounds.Width, thickness), color);
        _spriteBatch.Draw(_overlayPixel, new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
        _spriteBatch.Draw(_overlayPixel, new Rectangle(bounds.Right - thickness, bounds.Y, thickness, bounds.Height), color);
    }
}
