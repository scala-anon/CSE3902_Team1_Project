using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Abilities;
using HollowKnight.Storage;
using System;

namespace HollowKnight;

public partial class Game1
{
    internal void DrawPausedStateOverlay() => DrawCenteredOverlay(PauseTitle, PausePrompt, ScreenTint, Color.White);
    internal void DrawInventoryStateOverlay() => DrawInventoryOverlay();
    internal void DrawWinStateOverlay() => DrawCenteredOverlay(WinTitle, string.Empty, ScreenTint, Color.Yellow);

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

    private void DrawInventoryOverlay()
    {
        Rectangle overlayBounds = GetOverlayBounds();
        _spriteBatch.Draw(_overlayPixel, overlayBounds, InventoryTint);
        DrawInventoryBackdrop(overlayBounds);
        DrawReferenceInventoryLayout(overlayBounds);
    }

    internal bool TrySelectInventoryItem(Point mousePosition)
    {
        if (_gameState is not InventoryState)
        {
            return false;
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
            overlayBounds.Center.X - (titleSize.X * InventoryTitleScale) / 2f,
            InventoryTitleY);
        _spriteBatch.DrawString(_hudFont, InventoryTitle, titlePosition, Color.White, 0f, Vector2.Zero, InventoryTitleScale, SpriteEffects.None, 0f);

        Rectangle centerBounds = GetReferenceInventoryBounds(overlayBounds);
        DrawFramedPanel(centerBounds, InventoryPanelFillColor, InventoryPanelBorderColor, InventoryPanelBorderThickness);

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
        int visibleRows = Math.Min(InventoryGridMaxRows, Math.Max(1, (int)Math.Ceiling(ItemManager.Items.Count / (double)InventoryGridColumns)));
        int availableWidth = centerBounds.Width - (InventoryGridOuterPadding * 2) - ((InventoryGridColumns - 1) * InventoryGridCellSpacing);
        int availableHeight = centerBounds.Height - (InventoryGridOuterPadding * 2) - ((visibleRows - 1) * InventoryGridCellSpacing);
        int cellSize = Math.Min(availableWidth / InventoryGridColumns, availableHeight / visibleRows);
        int totalWidth = (cellSize * InventoryGridColumns) + ((InventoryGridColumns - 1) * InventoryGridCellSpacing);
        int totalHeight = (cellSize * visibleRows) + ((visibleRows - 1) * InventoryGridCellSpacing);
        int startX = centerBounds.Center.X - (totalWidth / 2);
        int startY = centerBounds.Y + InventoryGridOuterPadding;
        int row = index / InventoryGridColumns;
        int column = index % InventoryGridColumns;

        return new Rectangle(
            startX + (column * (cellSize + InventoryGridCellSpacing)),
            startY + (row * (cellSize + InventoryGridCellSpacing)),
            cellSize,
            cellSize);
    }

    private void DrawInventoryItemBox(Rectangle bounds, string itemName, bool isSelected, int itemNumber)
    {
        Color fillColor = InventoryBoxFillColor;
        Color borderColor = isSelected ? InventorySelectedBoxBorderColor : InventoryBoxBorderColor;
        _spriteBatch.Draw(_overlayPixel, bounds, fillColor);
        DrawPanelBorder(bounds, InventoryBoxBorderThickness, borderColor);

        Vector2 nameSize = _hudFont.MeasureString(itemName);
        float nameScale = itemName.Length > InventoryBoxLongNameThreshold ? InventoryBoxNameScaleLong : InventoryBoxNameScaleShort;
        Vector2 namePosition = new(
            bounds.Center.X - (nameSize.X * nameScale) / 2f,
            bounds.Center.Y - (nameSize.Y * nameScale) / 2f);
        _spriteBatch.DrawString(_hudFont, itemName, namePosition, Color.Black, 0f, Vector2.Zero, nameScale, SpriteEffects.None, 0f);
    }

    private void DrawInventoryBackdrop(Rectangle overlayBounds)
    {
        Rectangle vignetteTop = new(0, 0, overlayBounds.Width, InventoryBackdropTopHeight);
        Rectangle vignetteBottom = new(0, overlayBounds.Bottom - InventoryBackdropBottomHeight, overlayBounds.Width, InventoryBackdropBottomHeight);
        Rectangle leftEdge = new(0, 0, InventoryBackdropSideWidth, overlayBounds.Height);
        Rectangle rightEdge = new(overlayBounds.Right - InventoryBackdropSideWidth, 0, InventoryBackdropSideWidth, overlayBounds.Height);

        _spriteBatch.Draw(_overlayPixel, vignetteTop, InventoryBackdropTopColor);
        _spriteBatch.Draw(_overlayPixel, vignetteBottom, InventoryBackdropBottomColor);
        _spriteBatch.Draw(_overlayPixel, leftEdge, InventoryBackdropSideColor);
        _spriteBatch.Draw(_overlayPixel, rightEdge, InventoryBackdropSideColor);
    }

    private void DrawInventoryHeaderOrnaments(Rectangle overlayBounds)
    {
        Rectangle leftLine = new(overlayBounds.Center.X - InventoryHeaderLineOffsetLeft, InventoryHeaderLineY, InventoryHeaderLineWidth, InventoryHeaderLineHeight);
        Rectangle rightLine = new(overlayBounds.Center.X + InventoryHeaderLineOffsetRight, InventoryHeaderLineY, InventoryHeaderLineWidth, InventoryHeaderLineHeight);
        Rectangle leftCurl = new(overlayBounds.Center.X - InventoryHeaderCurlLeftOffset, InventoryHeaderCurlY, InventoryHeaderCurlWidth, InventoryHeaderCurlHeight);
        Rectangle rightCurl = new(overlayBounds.Center.X + InventoryHeaderCurlRightOffset, InventoryHeaderCurlY, InventoryHeaderCurlWidth, InventoryHeaderCurlHeight);
        Rectangle lowerFlourish = new(overlayBounds.Center.X - InventoryHeaderLowerFlourishOffset, InventoryHeaderLowerFlourishY, InventoryHeaderLowerFlourishWidth, InventoryHeaderLowerFlourishHeight);

        _spriteBatch.Draw(_overlayPixel, leftLine, Color.White);
        _spriteBatch.Draw(_overlayPixel, rightLine, Color.White);
        _spriteBatch.Draw(_overlayPixel, leftCurl, InventoryHeaderCurlColor);
        _spriteBatch.Draw(_overlayPixel, rightCurl, InventoryHeaderCurlColor);
        _spriteBatch.Draw(_overlayPixel, lowerFlourish, Color.White);
    }

    private void DrawInventoryBodyFrame(Rectangle overlayBounds)
    {
        Rectangle centerBounds = GetReferenceInventoryBounds(overlayBounds);
        Rectangle leftRail = new(centerBounds.X, centerBounds.Y, InventoryBodyRailWidth, centerBounds.Height);
        Rectangle rightRail = new(centerBounds.Right - InventoryBodyRailWidth, centerBounds.Y, InventoryBodyRailWidth, centerBounds.Height);
        Rectangle bottomFlourish = new(centerBounds.Center.X - InventoryBodyBottomFlourishHalfWidth, centerBounds.Bottom + InventoryBodyBottomFlourishY, InventoryBodyBottomFlourishWidth, InventoryBodyBottomFlourishHeight);
        Rectangle bottomCenter = new(centerBounds.Center.X - InventoryBodyBottomCenterHalfWidth, centerBounds.Bottom + InventoryBodyBottomCenterY, InventoryBodyBottomCenterWidth, InventoryBodyBottomCenterHeight);

        _spriteBatch.Draw(_overlayPixel, leftRail, InventoryBodyRailColor);
        _spriteBatch.Draw(_overlayPixel, rightRail, InventoryBodyRailColor);
        _spriteBatch.Draw(_overlayPixel, bottomFlourish, Color.White);
        _spriteBatch.Draw(_overlayPixel, bottomCenter, InventoryBodyBottomCenterColor);
    }

    private static Rectangle GetReferenceInventoryBounds(Rectangle overlayBounds)
    {
        return new Rectangle(
            (overlayBounds.Width / 2) - InventoryPanelHalfWidth,
            InventoryPanelTop,
            InventoryPanelWidth,
            overlayBounds.Height - InventoryPanelBottomMargin);
    }

    private void DrawFramedPanel(Rectangle bounds, Color fillColor, Color borderColor, int borderThickness)
    {
        DrawShadow(bounds, 8, 8);
        _spriteBatch.Draw(_overlayPixel, bounds, fillColor);
        DrawPanelBorder(bounds, borderThickness, borderColor);

        Rectangle insetBounds = new(bounds.X + borderThickness + 2, bounds.Y + borderThickness + 2, Math.Max(1, bounds.Width - ((borderThickness + 2) * 2)), Math.Max(1, bounds.Height - ((borderThickness + 2) * 2)));
        if (insetBounds.Width > 0 && insetBounds.Height > 0)
        {
            _spriteBatch.Draw(_overlayPixel, insetBounds, InventoryPanelInsetHighlightColor);
        }
    }

    private void DrawShadow(Rectangle bounds, int offsetX, int offsetY)
    {
        Rectangle shadowBounds = new(bounds.X + offsetX, bounds.Y + offsetY, bounds.Width, bounds.Height);
        _spriteBatch.Draw(_overlayPixel, shadowBounds, InventoryShadowColor);
    }

    private void DrawPanelBorder(Rectangle bounds, int thickness, Color color)
    {
        _spriteBatch.Draw(_overlayPixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
        _spriteBatch.Draw(_overlayPixel, new Rectangle(bounds.X, bounds.Bottom - thickness, bounds.Width, thickness), color);
        _spriteBatch.Draw(_overlayPixel, new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
        _spriteBatch.Draw(_overlayPixel, new Rectangle(bounds.Right - thickness, bounds.Y, thickness, bounds.Height), color);
    }
}
