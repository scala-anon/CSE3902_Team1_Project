using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;
using HollowKnight.Levels;
using HollowKnight.Audio;
using System.IO;

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

    public class ToggleMuteCommand : ICommand
    {
        private bool muted = false;
        public ToggleMuteCommand()
        {
            
        }
    
        public void Execute()
        {
            if (muted == false)
            {
                AudioManager.Instance.MuteAudio();
                muted = true;
            }
            else
            {
                AudioManager.Instance.UnmuteAudio();
                muted = false;
            }
        } 
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
