using HollowKnight.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight;

public partial class Game1
{
    private const string PauseTitle = "PAUSED";
    private const string InventoryTitle = "INVENTORY";
    private const string InventoryPrompt = "Tab close  |  U / I items  |  Click to select";
    private const string WinTitle = "YOU WIN";
    private const string LandingTitle = "HOLLOW KNIGHT";
    private const string StartGameLabel = "START GAME";
    private const string QuitGameLabel = "QUIT GAME";
    private const string ContinueGameLabel = "CONTINUE";
    private const string RestartGameLabel = "RESTART";
    private const string InventoryBackLabel = "GO BACK";

    private SpriteFont _hudFont;
    private HealthHud _healthHud;
    private SoulHud _soulHud;
    private Texture2D _overlayPixel;
    private Texture2D _titleBackgroundTexture;
    private Texture2D _titleLogoTexture;
    private bool _restartRequested;
}
