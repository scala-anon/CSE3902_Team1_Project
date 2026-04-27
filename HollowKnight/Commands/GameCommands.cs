using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;
using HollowKnight.Audio;
using HollowKnight.Player;
using HollowKnight.Shared;

namespace HollowKnight.Commands
{
    public class StartGameCommand : ICommand
    {
        private readonly Game1 _game;

        public StartGameCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute() => _game.StartGame();
    }

    public class QuitCommand : ICommand
    {
        private readonly Game _game;
        public QuitCommand(Game game) { _game = game; }
        public void Execute() => _game.Exit();
    }

    public class ToggleHitboxesCommand : ICommand
    {
        public ToggleHitboxesCommand(Game1 game) { }
        public void Execute() => DebugRenderer.hitboxEnabled = !DebugRenderer.hitboxEnabled;
    }

    public class ToggleGridCommand : ICommand
    {
        public ToggleGridCommand(Game1 game) { }
        public void Execute() => NavigationGrid.GridEnabled = !NavigationGrid.GridEnabled;
    }

    public class ToggleGodmodeCommand : ICommand
    {
        public ToggleGodmodeCommand() { }
        public void Execute()
        {
            TheKnight.GodmodeEnabled = !TheKnight.GodmodeEnabled;
            DebugLogger.LogGeneral($"Godmode toggled: {(TheKnight.GodmodeEnabled ? "ON" : "OFF")}");
        }
    }

    public class TogglePauseCommand : ICommand
    {
        private Game1 _game;

        public TogglePauseCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute()
        {
            _game.TogglePause();
        }
    }

    public class ToggleInventoryCommand : ICommand
    {
        private Game1 _game;

        public ToggleInventoryCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute()
        {
            _game.ToggleInventory();
        }
    }

    public class RestartGameCommand : ICommand
    {
        private readonly Game1 _game;

        public RestartGameCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute()
        {
            _game.ResetGame();
        }
    }

    public class GameplayOnlyCommand : ICommand
    {
        private readonly Game1 _game;
        private readonly ICommand _innerCommand;

        public GameplayOnlyCommand(Game1 game, ICommand innerCommand)
        {
            _game = game;
            _innerCommand = innerCommand;
        }

        public void Execute()
        {
            if (_game.AllowsGameplayInput())
            {
                _innerCommand.Execute();
            }
        }
    }

    public class SwitchRoomCommand : ICommand
    {
        private readonly Game1 _game;
        private readonly int _offset;

        public SwitchRoomCommand(Game1 game, int offset)
        {
            _game = game;
            _offset = offset;
        }

        public void Execute()
        {
            if (_offset > 0) _game.SwitchToNextRoom();
            else if (_offset < 0) _game.SwitchToPreviousRoom();
        }
    }

    public class DebugJumpToRoomCommand : ICommand
    {
        private readonly Game1 _game;
        private readonly int _targetRoom;

        public DebugJumpToRoomCommand(Game1 game, int targetRoom)
        {
            _game = game;
            _targetRoom = targetRoom;
        }

        public void Execute()
        {
            if (_game.CurrentRoom == _targetRoom)
            {
                DebugLogger.LogGeneral($"DebugJumpToRoom: already in room {_targetRoom}, no-op");
                return;
            }
            _game.TransitionToRoom(_targetRoom);
        }
    }

    public class ToggleMuteCommand : ICommand
    {
        public void Execute() => AudioManager.Instance.ToggleMute();
    }
}
