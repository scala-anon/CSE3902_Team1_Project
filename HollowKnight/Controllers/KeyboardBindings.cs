using Microsoft.Xna.Framework.Input;
using HollowKnight.Commands;
using HollowKnight.Player;

namespace HollowKnight.Controllers
{
    public static class KeyboardBindings
    {
        public static void BindGameplay(KeyboardController keyboard, TheKnight knight, Game1 game, RoomManager roomManager)
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


            // Debug room switching
            keyboard.RegisterComboPressedCommand(Keys.Right, Keys.LeftControl, new SwitchRoomCommand(roomManager, 1));
            keyboard.RegisterComboPressedCommand(Keys.Left, Keys.LeftControl, new SwitchRoomCommand(roomManager, -1));
            keyboard.RegisterComboPressedCommand(Keys.Right, Keys.RightControl, new SwitchRoomCommand(roomManager, 1));
            keyboard.RegisterComboPressedCommand(Keys.Left, Keys.RightControl, new SwitchRoomCommand(roomManager, -1));
            keyboard.RegisterPressedCommand(Keys.F1, new JumpToRoomCommand(roomManager, 0));
            keyboard.RegisterPressedCommand(Keys.F2, new JumpToRoomCommand(roomManager, 1));
            keyboard.RegisterPressedCommand(Keys.F3, new JumpToRoomCommand(roomManager, 2));

            // Quit Game
            keyboard.RegisterPressedCommand(Keys.Q, new QuitCommand(game));
        }
    }
}
