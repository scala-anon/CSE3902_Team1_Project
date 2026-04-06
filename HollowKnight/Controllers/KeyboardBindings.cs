using Microsoft.Xna.Framework.Input;
using HollowKnight.Commands;
using HollowKnight.Levels;
using HollowKnight.Player;

namespace HollowKnight.Controllers
{
    public static class KeyboardBindings
    {
        public static void BindGameplay(KeyboardController keyboard, TheKnight knight, Game1 game, RoomManager roomManager)
        {
            // Move Left — A and Left Arrow
            keyboard.RegisterHeldCommand(Keys.A, new PlayerMoveLeftCommand(knight));
            keyboard.RegisterHeldCommand(Keys.Left, new PlayerMoveLeftCommand(knight));

            // Move Right — D and Right Arrow
            keyboard.RegisterHeldCommand(Keys.D, new PlayerMoveRightCommand(knight));
            keyboard.RegisterHeldCommand(Keys.Right, new PlayerMoveRightCommand(knight));

            // Look Up — W and Up Arrow
            keyboard.RegisterHeldCommand(Keys.W, new PlayerMoveUpCommand(knight));
            keyboard.RegisterHeldCommand(Keys.Up, new PlayerMoveUpCommand(knight));

            // Stop Horizontal on release
            keyboard.RegisterReleasedCommand(Keys.A, new PlayerStopMovingHorizontalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.Left, new PlayerStopMovingHorizontalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.D, new PlayerStopMovingHorizontalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.Right, new PlayerStopMovingHorizontalCommand(knight));

            // Stop Vertical on release
            keyboard.RegisterReleasedCommand(Keys.W, new PlayerStopMovingVerticalCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.Up, new PlayerStopMovingVerticalCommand(knight));

            // Jump — Space
            keyboard.RegisterPressedCommand(Keys.Space, new PlayerJumpCommand(knight));

            // Attack — Z + direction for slash variants
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.W, new PlayerUpSlashCommand(knight));
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.Up, new PlayerUpSlashCommand(knight));
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.S, new PlayerDownSlashCommand(knight));
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.Down, new PlayerDownSlashCommand(knight));
            keyboard.RegisterComboPressedCommand(Keys.Z, new PlayerSideSlashCommand(knight));

            // Healing — hold X to heal
            keyboard.RegisterHeldCommand(Keys.X, new PlayerHealHoldCommand(knight));
            keyboard.RegisterReleasedCommand(Keys.X, new PlayerHealCancelCommand(knight));

            // Items
            keyboard.RegisterPressedCommand(Keys.D1, new PlayerUseItemCommand(knight, 1));
            keyboard.RegisterPressedCommand(Keys.D2, new PlayerUseItemCommand(knight, 2));
            keyboard.RegisterPressedCommand(Keys.D3, new PlayerUseItemCommand(knight, 3));

            // Cycle Items
            keyboard.RegisterPressedCommand(Keys.U, new CycleItemPreviousCommand());
            keyboard.RegisterPressedCommand(Keys.I, new CycleItemNextCommand());

            // Damage (debug)
            keyboard.RegisterPressedCommand(Keys.E, new PlayerTakeDamageCommand(knight));

            // Debug room switching
            keyboard.RegisterComboPressedCommand(Keys.Right, Keys.LeftControl, new SwitchRoomCommand(roomManager, 1));
            keyboard.RegisterComboPressedCommand(Keys.Left, Keys.LeftControl, new SwitchRoomCommand(roomManager, -1));
            keyboard.RegisterComboPressedCommand(Keys.Right, Keys.RightControl, new SwitchRoomCommand(roomManager, 1));
            keyboard.RegisterComboPressedCommand(Keys.Left, Keys.RightControl, new SwitchRoomCommand(roomManager, -1));
            keyboard.RegisterPressedCommand(Keys.F1, new JumpToRoomCommand(roomManager, 0));
            keyboard.RegisterPressedCommand(Keys.F2, new JumpToRoomCommand(roomManager, 1));
            keyboard.RegisterPressedCommand(Keys.F3, new JumpToRoomCommand(roomManager, 2));

            // Debug toggles
            keyboard.RegisterPressedCommand(Keys.H, new ToggleHitboxesCommand(game));
            keyboard.RegisterPressedCommand(Keys.G, new ToggleGridCommand(game));

            //Toggle Mute
            keyboard.RegisterComboPressedCommand(Keys.M, new ToggleMuteCommand());

            // Quit
            keyboard.RegisterPressedCommand(Keys.Q, new QuitCommand(game));
        }
    }
}
