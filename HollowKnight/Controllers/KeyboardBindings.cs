using Microsoft.Xna.Framework.Input;
using HollowKnight.Commands;
using HollowKnight.Interfaces;
using HollowKnight.Levels;
using HollowKnight.Player;

namespace HollowKnight.Controllers
{
    public static class KeyboardBindings
    {
        private static ICommand Gameplay(Game1 game, ICommand command) => new GameplayOnlyCommand(game, command);

        public static void BindGameplay(KeyboardController keyboard, TheKnight knight, Game1 game, RoomManager roomManager)
        {
            //TODO: remove developer keybinding and change ability and movement binds if needed

            // Move Left — A and Left Arrow
            keyboard.RegisterHeldCommand(Keys.A, Gameplay(game, new PlayerMoveLeftCommand(knight)));
            keyboard.RegisterHeldCommand(Keys.Left, Gameplay(game, new PlayerMoveLeftCommand(knight)));

            // Move Right — D and Right Arrow
            keyboard.RegisterHeldCommand(Keys.D, Gameplay(game, new PlayerMoveRightCommand(knight)));
            keyboard.RegisterHeldCommand(Keys.Right, Gameplay(game, new PlayerMoveRightCommand(knight)));

            // Look Up — W and Up Arrow
            keyboard.RegisterHeldCommand(Keys.W, Gameplay(game, new PlayerMoveUpCommand(knight)));
            keyboard.RegisterHeldCommand(Keys.Up, Gameplay(game, new PlayerMoveUpCommand(knight)));

            // Stop Horizontal on release
            keyboard.RegisterReleasedCommand(Keys.A, Gameplay(game, new PlayerStopMovingHorizontalCommand(knight)));
            keyboard.RegisterReleasedCommand(Keys.Left, Gameplay(game, new PlayerStopMovingHorizontalCommand(knight)));
            keyboard.RegisterReleasedCommand(Keys.D, Gameplay(game, new PlayerStopMovingHorizontalCommand(knight)));
            keyboard.RegisterReleasedCommand(Keys.Right, Gameplay(game, new PlayerStopMovingHorizontalCommand(knight)));

            // Stop Vertical on release
            keyboard.RegisterReleasedCommand(Keys.W, Gameplay(game, new PlayerStopMovingVerticalCommand(knight)));
            keyboard.RegisterReleasedCommand(Keys.Up, Gameplay(game, new PlayerStopMovingVerticalCommand(knight)));

            // Jump — Space
            keyboard.RegisterPressedCommand(Keys.Space, Gameplay(game, new PlayerJumpCommand(knight)));
            keyboard.RegisterReleasedCommand(Keys.Space, Gameplay(game, new PlayerStopJumpCommand(knight)));

            //Dash - C
            keyboard.RegisterPressedCommand(Keys.C, new PlayerDashCommand(knight));

            //SpellCasting - T 
            keyboard.RegisterPressedCommand(Keys.T, new PlayerSpellCastCommand(knight));

            // Attack — Z + direction for slash variants
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.W, Gameplay(game, new PlayerUpSlashCommand(knight)));
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.Up, Gameplay(game, new PlayerUpSlashCommand(knight)));
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.S, Gameplay(game, new PlayerDownSlashCommand(knight)));
            keyboard.RegisterComboPressedCommand(Keys.Z, Keys.Down, Gameplay(game, new PlayerDownSlashCommand(knight)));
            keyboard.RegisterComboPressedCommand(Keys.Z, Gameplay(game, new PlayerSideSlashCommand(knight)));

            // Healing — hold X to heal
            keyboard.RegisterHeldCommand(Keys.X, Gameplay(game, new PlayerHealHoldCommand(knight)));
            keyboard.RegisterReleasedCommand(Keys.X, Gameplay(game, new PlayerHealCancelCommand(knight)));

            // Items
            keyboard.RegisterPressedCommand(Keys.D1, Gameplay(game, new PlayerUseItemCommand(knight, 1)));
            keyboard.RegisterPressedCommand(Keys.D2, Gameplay(game, new PlayerUseItemCommand(knight, 2)));
            keyboard.RegisterPressedCommand(Keys.D3, Gameplay(game, new PlayerUseItemCommand(knight, 3)));

            // Cycle Items
            keyboard.RegisterPressedCommand(Keys.U, Gameplay(game, new CycleItemPreviousCommand()));
            keyboard.RegisterPressedCommand(Keys.I, Gameplay(game, new CycleItemNextCommand()));

            // Give Soul (debug) - Using Y because U is taken
            keyboard.RegisterPressedCommand(Keys.Y, new PlayerGiveSoulCommand(knight));

            // GameState
            keyboard.RegisterPressedCommand(Keys.P, new TogglePauseCommand(game));
            keyboard.RegisterPressedCommand(Keys.Tab, new ToggleInventoryCommand(game));
            keyboard.RegisterPressedCommand(Keys.R, new RestartGameCommand(game));

            // GameState (debug)
            keyboard.RegisterPressedCommand(Keys.F5, new SetGameOverCommand(game));
            keyboard.RegisterPressedCommand(Keys.F6, new SetWinCommand(game));

            // Damage (debug)
            keyboard.RegisterPressedCommand(Keys.E, Gameplay(game, new PlayerTakeDamageCommand(knight)));
            keyboard.RegisterPressedCommand(Keys.T, Gameplay(game, new PlayerGainSoulCommand(knight, 11)));

            // Debug room switching
            keyboard.RegisterComboPressedCommand(Keys.Right, Keys.LeftControl, Gameplay(game, new SwitchRoomCommand(roomManager, 1)));
            keyboard.RegisterComboPressedCommand(Keys.Left, Keys.LeftControl, Gameplay(game, new SwitchRoomCommand(roomManager, -1)));
            keyboard.RegisterComboPressedCommand(Keys.Right, Keys.RightControl, Gameplay(game, new SwitchRoomCommand(roomManager, 1)));
            keyboard.RegisterComboPressedCommand(Keys.Left, Keys.RightControl, Gameplay(game, new SwitchRoomCommand(roomManager, -1)));
            keyboard.RegisterPressedCommand(Keys.F1, Gameplay(game, new JumpToRoomCommand(roomManager, 0)));
            keyboard.RegisterPressedCommand(Keys.F2, Gameplay(game, new JumpToRoomCommand(roomManager, 1)));
            keyboard.RegisterPressedCommand(Keys.F3, Gameplay(game, new JumpToRoomCommand(roomManager, 2)));

            // Debug toggles
            keyboard.RegisterPressedCommand(Keys.H, new ToggleHitboxesCommand(game));
            keyboard.RegisterPressedCommand(Keys.G, new ToggleGridCommand(game));

            // Audio
            keyboard.RegisterPressedCommand(Keys.M, new ToggleMuteCommand());

            // Quit
            keyboard.RegisterPressedCommand(Keys.Q, new QuitCommand(game));
        }
    }
}
