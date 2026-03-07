using Microsoft.Xna.Framework.Input;
using HollowKnight.Commands;
using HollowKnight.Player;

namespace HollowKnight.Controllers
{
    public static class KeyboardBindings
    {
        public static void BindGameplay(KeyboardController keyboard, TheKnight knight, Game1 game)
        {
            // Move Left
            keyboard.RegisterHeldCommand(Keys.A, new PlayerMoveLeftCommand(knight));
            keyboard.RegisterHeldCommand(Keys.Left, new PlayerMoveLeftCommand(knight));

            // Move Right
            keyboard.RegisterHeldCommand(Keys.D, new PlayerMoveRightCommand(knight));
            keyboard.RegisterHeldCommand(Keys.Right, new PlayerMoveRightCommand(knight));

            // Look Up
            keyboard.RegisterHeldCommand(Keys.W, new PlayerMoveUpCommand(knight));
            keyboard.RegisterHeldCommand(Keys.Up, new PlayerMoveUpCommand(knight));

            // Look Down
            keyboard.RegisterHeldCommand(Keys.S, new PlayerMoveDownCommand(knight));
            keyboard.RegisterHeldCommand(Keys.Down, new PlayerMoveDownCommand(knight));

            // Stop Horizontal
            keyboard.RegisterReleasedCommand(Keys.A, new PlayerStopMovingHorizontalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.Left, new PlayerStopMovingHorizontalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.D, new PlayerStopMovingHorizontalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.Right, new PlayerStopMovingHorizontalCommand(knight));

            // Stop Vertical
            keyboard.RegisterReleasedCommand(Keys.W, new PlayerStopMovingVerticalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.Up, new PlayerStopMovingVerticalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.S, new PlayerStopMovingVerticalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.Down, new PlayerStopMovingVerticalCommand(knight));

            // Jump
            keyboard.RegisterPressedCommand(Keys.Space, new PlayerJumpCommand(knight));

            // Attack (SideSlash, UpSlash, DownSlash)
            // Up Slash
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.W, new PlayerUpSlashCommand(knight));
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.Up, new PlayerUpSlashCommand(knight));

            // Down Slash
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.S, new PlayerDownSlashCommand(knight));
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.Down, new PlayerDownSlashCommand(knight));

            // Side
            keyboard.RegisterComboPressedCommand(Keys.Z, new PlayerSideSlashCommand(knight));

            // Healing
            keyboard.RegisterHeldCommand(Keys.A, new PlayerHealHoldCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.A, new PlayerHealCancelCommand(knight));

            // Items
            keyboard.RegisterPressedCommand(Keys.D1, new PlayerUseItemCommand(knight, 1));
            keyboard.RegisterPressedCommand(Keys.D2, new PlayerUseItemCommand(knight, 2));
            keyboard.RegisterPressedCommand(Keys.D3, new PlayerUseItemCommand(knight, 3));

            // Cycle Items
            keyboard.RegisterPressedCommand(Keys.U, new CycleItemPreviousCommand());
            keyboard.RegisterPressedCommand(Keys.I, new CycleItemNextCommand());

            // Damage
            keyboard.RegisterPressedCommand(Keys.E, new PlayerTakeDamageCommand(knight));


            //Commands for switching enemy sprites
            keyboard.RegisterPressedCommand(Keys.P, new ChangeNextEnemyCommand(game));
            keyboard.RegisterPressedCommand(Keys.O, new ChangePreviousEnemyCommand(game));

            //Commands for switching Enviroment Sprites
            keyboard.RegisterPressedCommand(Keys.Y, new ChangeNextEnviromentCommand(game));
            keyboard.RegisterPressedCommand(Keys.T, new ChangePreviousEnviromentCommand(game));

            //Command for hitbox toggling
            keyboard.RegisterPressedCommand(Keys.H, new ToggleHitboxesCommand(game));

            //Command for grid toggling
            keyboard.RegisterPressedCommand(Keys.G, new ToggleGridCommand(game));

            // Quit Game
            keyboard.RegisterPressedCommand(Keys.Q, new QuitCommand(game));
        }
    }
}