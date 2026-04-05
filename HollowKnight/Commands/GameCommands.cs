using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;
using HollowKnight.Levels;

namespace HollowKnight.Commands
{
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

    public class SetGameOverCommand : ICommand
    {
        private Game1 _game;

        public SetGameOverCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute()
        {
            _game.SetGameOver();
        }
    }

    public class SetWinCommand : ICommand
    {
        private Game1 _game;

        public SetWinCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute()
        {
            _game.SetWin();
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
        private readonly RoomManager _roomManager;
        private readonly int _offset;

        public SwitchRoomCommand(RoomManager roomManager, int offset)
        {
            _roomManager = roomManager;
            _offset = offset;
        }

        public void Execute() => _roomManager.SwitchRoomByOffset(_offset);
    }

    public class JumpToRoomCommand : ICommand
    {
        private readonly RoomManager _roomManager;
        private readonly int _roomIndex;

        public JumpToRoomCommand(RoomManager roomManager, int roomIndex)
        {
            _roomManager = roomManager;
            _roomIndex = roomIndex;
        }

        public void Execute() => _roomManager.JumpToRoomIndex(_roomIndex);
    }
}
