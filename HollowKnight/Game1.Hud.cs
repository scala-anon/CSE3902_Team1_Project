using HollowKnight.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight;

public partial class Game1
{
    private const float OverlayTextLineSpacing = 18f;

    private const string GameOverTitle = "GAME OVER";
    private const string GameOverPrompt = "Press R to Restart";
    private const string PauseTitle = "PAUSED";
    private const string PausePrompt = "Press P to Resume";
    private const string InventoryTitle = "INVENTORY";
    private const string InventoryPrompt = "Press Tab to Close";
    private const string WinTitle = "YOU WIN";

    private SpriteFont _hudFont;
    private HealthHud _healthHud;
    private SoulHud _soulHud;
    private Texture2D _overlayPixel;
    private bool _restartRequested;

    private static readonly Color ScreenTint = new(0, 0, 0, 190);
    private static readonly Color InventoryTint = new(8, 12, 24, 210);
}
