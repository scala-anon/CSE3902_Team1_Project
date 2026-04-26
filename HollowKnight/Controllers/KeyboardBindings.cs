using Microsoft.Xna.Framework.Input;
using HollowKnight.Commands;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;

namespace HollowKnight.Controllers
{
    public static class KeyboardBindings
    {
        private static ICommand Gameplay(Game1 game, ICommand command) => new GameplayOnlyCommand(game, command);

        public static void BindGameplay(KeyboardController keyboard, TheKnight knight, Game1 game)
        {
            keyboard.RegisterPressedCommand(Keys.Enter, new StartGameCommand(game));
            keyboard.RegisterPressedCommand(Keys.Escape, new QuitCommand(game));

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

            // SpellCasting - F (Vengeful Spirit)
            keyboard.RegisterPressedCommand(Keys.F, Gameplay(game, new PlayerSpellCastCommand(knight)));

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
            keyboard.RegisterPressedCommand(Keys.D1, new PlayerUseItemCommand(game, knight, 1));
            keyboard.RegisterPressedCommand(Keys.D2, new PlayerUseItemCommand(game, knight, 2));
            keyboard.RegisterPressedCommand(Keys.D3, new PlayerUseItemCommand(game, knight, 3));

            // Cycle Items
            keyboard.RegisterPressedCommand(Keys.U, new CycleItemPreviousCommand());
            keyboard.RegisterPressedCommand(Keys.I, new CycleItemNextCommand());

            // Give Soul (debug - automatically get max soul) - Y 
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

            // Debug room switching
            keyboard.RegisterComboPressedCommand(Keys.Right, Keys.LeftControl, Gameplay(game, new SwitchRoomCommand(game, 1)));
            keyboard.RegisterComboPressedCommand(Keys.Left, Keys.LeftControl, Gameplay(game, new SwitchRoomCommand(game, -1)));
            keyboard.RegisterComboPressedCommand(Keys.Right, Keys.RightControl, Gameplay(game, new SwitchRoomCommand(game, 1)));
            keyboard.RegisterComboPressedCommand(Keys.Left, Keys.RightControl, Gameplay(game, new SwitchRoomCommand(game, -1)));
            keyboard.RegisterPressedCommand(Keys.F1, Gameplay(game, new DebugJumpToRoomCommand(game, 1)));
            keyboard.RegisterPressedCommand(Keys.F2, Gameplay(game, new DebugJumpToRoomCommand(game, 2)));
            keyboard.RegisterPressedCommand(Keys.F3, Gameplay(game, new DebugJumpToRoomCommand(game, 3)));
            keyboard.RegisterPressedCommand(Keys.F4, Gameplay(game, new DebugJumpToRoomCommand(game, 4)));

            // Debug toggles
            keyboard.RegisterPressedCommand(Keys.H, new ToggleHitboxesCommand(game));
            keyboard.RegisterPressedCommand(Keys.G, new ToggleGridCommand(game));

            // Debug hotkeys (compile-time gated; eliminated when DEBUG_LOG_ENABLED != 1)
            if (GameConstants.DEBUG_LOG_ENABLED == 1)
            {
                // Ctrl+D1..D4 — direct room jump via Game1.TransitionToRoom
                keyboard.RegisterComboPressedCommand(Keys.D1, Keys.LeftControl,  Gameplay(game, new DebugJumpToRoomCommand(game, 1)));
                keyboard.RegisterComboPressedCommand(Keys.D1, Keys.RightControl, Gameplay(game, new DebugJumpToRoomCommand(game, 1)));
                keyboard.RegisterComboPressedCommand(Keys.D2, Keys.LeftControl,  Gameplay(game, new DebugJumpToRoomCommand(game, 2)));
                keyboard.RegisterComboPressedCommand(Keys.D2, Keys.RightControl, Gameplay(game, new DebugJumpToRoomCommand(game, 2)));
                keyboard.RegisterComboPressedCommand(Keys.D3, Keys.LeftControl,  Gameplay(game, new DebugJumpToRoomCommand(game, 3)));
                keyboard.RegisterComboPressedCommand(Keys.D3, Keys.RightControl, Gameplay(game, new DebugJumpToRoomCommand(game, 3)));
                keyboard.RegisterComboPressedCommand(Keys.D4, Keys.LeftControl,  Gameplay(game, new DebugJumpToRoomCommand(game, 4)));
                keyboard.RegisterComboPressedCommand(Keys.D4, Keys.RightControl, Gameplay(game, new DebugJumpToRoomCommand(game, 4)));

                // K — godmode toggle (NOT gameplay-gated, matches H/G pattern)
                keyboard.RegisterPressedCommand(Keys.K, new ToggleGodmodeCommand());
            }

            // Audio
            keyboard.RegisterPressedCommand(Keys.M, new ToggleMuteCommand());

            //Interactions
            // Bench / interactive object button
            keyboard.RegisterPressedCommand(Keys.Up, Gameplay(game, new PlayerInteractCommand(game)));
            keyboard.RegisterPressedCommand(Keys.W, Gameplay(game, new PlayerInteractCommand(game)));

            // Start Mantis Lord boss fight (debug / trigger key)
            keyboard.RegisterPressedCommand(Keys.B, new StartMantisFightCommand(game));

            // Quit
            keyboard.RegisterPressedCommand(Keys.Q, new QuitCommand(game));
        }
    }
}
