using HollowKnight.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight;

public partial class Game1
{
    private const float OverlayTextLineSpacing = 18f;
    private const float MantisChallengePromptScale = 1.25f;
    private const float MantisChallengePromptLineSpacing = 16f;
    private const float MantisChallengeTriggerHalfWidth = 150f;
    private const float InventoryTitleScale = 0.9f;
    private const int InventoryTitleY = 26;

    private const int InventoryPanelHalfWidth = 210;
    private const int InventoryPanelTop = 118;
    private const int InventoryPanelWidth = 420;
    private const int InventoryPanelBottomMargin = 240;
    private const int InventoryPanelBorderThickness = 1;

    private const int InventoryGridColumns = 3;
    private const int InventoryGridMaxRows = 5;
    private const int InventoryGridCellSpacing = 12;
    private const int InventoryGridOuterPadding = 32;

    private const int InventoryBoxBorderThickness = 2;
    private const float InventoryBoxNameScaleShort = 0.93f;
    private const float InventoryBoxNameScaleLong = 0.75f;
    private const int InventoryBoxLongNameThreshold = 9;

    private const int InventoryBackdropTopHeight = 86;
    private const int InventoryBackdropBottomHeight = 100;
    private const int InventoryBackdropSideWidth = 24;

    private const int InventoryHeaderLineOffsetLeft = 190;
    private const int InventoryHeaderLineOffsetRight = 58;
    private const int InventoryHeaderLineWidth = 132;
    private const int InventoryHeaderLineY = 48;
    private const int InventoryHeaderLineHeight = 2;
    private const int InventoryHeaderCurlLeftOffset = 232;
    private const int InventoryHeaderCurlRightOffset = 196;
    private const int InventoryHeaderCurlY = 42;
    private const int InventoryHeaderCurlWidth = 36;
    private const int InventoryHeaderCurlHeight = 14;
    private const int InventoryHeaderLowerFlourishOffset = 8;
    private const int InventoryHeaderLowerFlourishY = 58;
    private const int InventoryHeaderLowerFlourishWidth = 16;
    private const int InventoryHeaderLowerFlourishHeight = 4;

    private const int InventoryBodyRailWidth = 2;
    private const int InventoryBodyBottomFlourishHalfWidth = 116;
    private const int InventoryBodyBottomFlourishY = 18;
    private const int InventoryBodyBottomFlourishWidth = 232;
    private const int InventoryBodyBottomFlourishHeight = 2;
    private const int InventoryBodyBottomCenterHalfWidth = 12;
    private const int InventoryBodyBottomCenterY = 12;
    private const int InventoryBodyBottomCenterWidth = 24;
    private const int InventoryBodyBottomCenterHeight = 14;

    private const string GameOverTitle = "GAME OVER";
    private const string GameOverPrompt = "Press R to Restart";
    private const string PauseTitle = "PAUSED";
    private const string PausePrompt = "Press P to Resume";
    private const string InventoryTitle = "INVENTORY";
    private const string InventoryPrompt = "Tab close  |  U / I items  |  Click to select";
    private const string WinTitle = "YOU WIN";
    private const string MantisChallengeTitle = "CHALLENGE";
    private const string MantisChallengePrompt = "Press L to Challenge";

    private SpriteFont _hudFont;
    private HealthHud _healthHud;
    private SoulHud _soulHud;
    private Texture2D _overlayPixel;
    private bool _restartRequested;
    private bool _showMantisChallengePrompt;

    private static readonly Color ScreenTint = new(0, 0, 0, 190);
    private static readonly Color InventoryTint = new(6, 8, 18, 218);
    private static readonly Color InventoryPanelColor = new(30, 22, 28, 236);
    private static readonly Color InventoryHighlightColor = new(202, 168, 111, 255);
    private static readonly Color InventoryTextColor = new(240, 232, 220, 255);
    private static readonly Color InventoryMutedTextColor = new(184, 170, 156, 228);
    private static readonly Color InventoryAccentColor = new(120, 77, 49, 255);
    private static readonly Color InventoryShadowColor = new(0, 0, 0, 110);
    private static readonly Color InventorySurfaceColor = new(58, 42, 40, 225);
    private static readonly Color InventoryInsetColor = new(88, 63, 54, 210);
    private static readonly Color InventoryPanelFillColor = new(18, 14, 20, 120);
    private static readonly Color InventoryPanelBorderColor = new(130, 130, 142, 96);
    private static readonly Color InventoryBoxFillColor = new(214, 214, 214, 235);
    private static readonly Color InventoryBoxBorderColor = new(120, 120, 120, 255);
    private static readonly Color InventorySelectedBoxBorderColor = new(212, 175, 55, 255);
    private static readonly Color InventoryBackdropTopColor = new(18, 10, 12, 120);
    private static readonly Color InventoryBackdropBottomColor = new(18, 10, 12, 135);
    private static readonly Color InventoryBackdropSideColor = new(10, 6, 10, 115);
    private static readonly Color InventoryBodyRailColor = new(226, 226, 234, 160);
    private static readonly Color InventoryBodyBottomCenterColor = new(255, 255, 255, 170);
    private static readonly Color InventoryHeaderCurlColor = new(255, 255, 255, 160);
    private static readonly Color InventoryPanelInsetHighlightColor = new(255, 255, 255, 8);
}
