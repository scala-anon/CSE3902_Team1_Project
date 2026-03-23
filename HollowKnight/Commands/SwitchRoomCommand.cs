using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class SwitchRoomCommand : ICommand
    {
        private readonly RoomManager _roomManager;
        private readonly int _offset;

        public SwitchRoomCommand(RoomManager roomManager, int offset)
        {
            _roomManager = roomManager;
            _offset = offset;
        }

        public void Execute()
        {
            _roomManager.SwitchRoomByOffset(_offset);
        }
    }
}
