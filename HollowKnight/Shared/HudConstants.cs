using Microsoft.Xna.Framework;

namespace HollowKnight.Shared
{
    public static class HudConstants
    {
        // Shared overlay spacing
        public const float OverlayTextLineSpacing = 18f;

        // Health HUD
        public const int HealthPipWidth = 36;
        public const int HealthPipHeight = 52;
        public const int HealthPipStartX = 118;
        public const int HealthPipStartY = 28;
        public const int HealthPipSpacing = 0;

        // Soul HUD
        public const int SoulGaugeWidth = 102;
        public const int SoulGaugeHeight = 98;
        public const int SoulGaugeX = 14;
        public const int SoulGaugeY = 22;

        // Inventory overlay layout
        public const float InventoryTitleScale = 0.9f;
        public const int InventoryTitleY = 26;
        public const int InventoryPanelHalfWidth = 210;
        public const int InventoryPanelTop = 118;
        public const int InventoryPanelWidth = 420;
        public const int InventoryPanelBottomMargin = 240;
        public const int InventoryPanelBorderThickness = 1;
        public const int InventoryGridColumns = 3;
        public const int InventoryGridMaxRows = 5;
        public const int InventoryGridCellSpacing = 12;
        public const int InventoryGridOuterPadding = 32;
        public const int InventoryBoxBorderThickness = 2;
        public const float InventoryBoxNameScaleShort = 0.93f;
        public const float InventoryBoxNameScaleLong = 0.75f;
        public const int InventoryBoxLongNameThreshold = 9;
        public const int InventoryBackButtonWidth = 168;
        public const int InventoryBackButtonHeight = 48;
        public const int InventoryBackButtonRightMargin = 36;
        public const int InventoryBackButtonBottomMargin = 36;
        public const float InventoryBackButtonScale = 0.78f;
        public const int InventoryBackdropTopHeight = 86;
        public const int InventoryBackdropBottomHeight = 100;
        public const int InventoryBackdropSideWidth = 24;
        public const int InventoryHeaderLineOffsetLeft = 190;
        public const int InventoryHeaderLineOffsetRight = 58;
        public const int InventoryHeaderLineWidth = 132;
        public const int InventoryHeaderLineY = 48;
        public const int InventoryHeaderLineHeight = 2;
        public const int InventoryHeaderCurlLeftOffset = 232;
        public const int InventoryHeaderCurlRightOffset = 196;
        public const int InventoryHeaderCurlY = 42;
        public const int InventoryHeaderCurlWidth = 36;
        public const int InventoryHeaderCurlHeight = 14;
        public const int InventoryHeaderLowerFlourishOffset = 8;
        public const int InventoryHeaderLowerFlourishY = 58;
        public const int InventoryHeaderLowerFlourishWidth = 16;
        public const int InventoryHeaderLowerFlourishHeight = 4;
        public const int InventoryBodyRailWidth = 2;
        public const int InventoryBodyBottomFlourishHalfWidth = 116;
        public const int InventoryBodyBottomFlourishY = 18;
        public const int InventoryBodyBottomFlourishWidth = 232;
        public const int InventoryBodyBottomFlourishHeight = 2;
        public const int InventoryBodyBottomCenterHalfWidth = 12;
        public const int InventoryBodyBottomCenterY = 12;
        public const int InventoryBodyBottomCenterWidth = 24;
        public const int InventoryBodyBottomCenterHeight = 14;

        // Title / pause / game-over menu layout
        public const int TitleButtonWidth = 250;
        public const int TitleButtonHeight = 52;
        public const int TitleButtonSpacing = 18;
        public const float TitleScale = 1.15f;
        public const float TitleSubtitleScale = 0.7f;
        public const float TitleButtonScale = 0.66f;
        public const int PauseButtonWidth = 250;
        public const int PauseButtonHeight = 52;
        public const int PauseButtonSpacing = 18;
        public const float PauseTitleScale = 1.05f;
        public const float PauseButtonScale = 0.66f;

        // HUD / menu colors
        public static readonly Color ScreenTint = new(0, 0, 0, 190);
        public static readonly Color InventoryTint = new(6, 8, 18, 218);
        public static readonly Color InventoryPanelColor = new(30, 22, 28, 236);
        public static readonly Color InventoryHighlightColor = new(202, 168, 111, 255);
        public static readonly Color InventoryTextColor = new(240, 232, 220, 255);
        public static readonly Color InventoryMutedTextColor = new(184, 170, 156, 228);
        public static readonly Color InventoryAccentColor = new(120, 77, 49, 255);
        public static readonly Color InventoryShadowColor = new(0, 0, 0, 110);
        public static readonly Color InventorySurfaceColor = new(58, 42, 40, 225);
        public static readonly Color InventoryInsetColor = new(88, 63, 54, 210);
        public static readonly Color InventoryPanelFillColor = new(18, 14, 20, 120);
        public static readonly Color InventoryPanelBorderColor = new(130, 130, 142, 96);
        public static readonly Color InventoryBoxFillColor = new(214, 214, 214, 235);
        public static readonly Color InventoryBoxBorderColor = new(120, 120, 120, 255);
        public static readonly Color InventorySelectedBoxBorderColor = new(212, 175, 55, 255);
        public static readonly Color InventoryBackdropTopColor = new(18, 10, 12, 120);
        public static readonly Color InventoryBackdropBottomColor = new(18, 10, 12, 135);
        public static readonly Color InventoryBackdropSideColor = new(10, 6, 10, 115);
        public static readonly Color InventoryBodyRailColor = new(226, 226, 234, 160);
        public static readonly Color InventoryBodyBottomCenterColor = new(255, 255, 255, 170);
        public static readonly Color InventoryHeaderCurlColor = new(255, 255, 255, 160);
        public static readonly Color InventoryPanelInsetHighlightColor = new(255, 255, 255, 8);
        public static readonly Color TitleBackdropTint = new(3, 5, 12, 240);
        public static readonly Color TitleAccentColor = new(219, 231, 255, 255);
        public static readonly Color TitleButtonColor = new(231, 236, 246, 255);
        public static readonly Color TitleButtonHoverColor = new(255, 244, 214, 255);
        public static readonly Color TitleButtonTextColor = new(18, 25, 42, 255);
        public static readonly Color InactiveHealthPipColor = new(28, 32, 44, 255);
        public static readonly Color SoulGaugeMinColor = new(85, 85, 85, 255);
    }
}
