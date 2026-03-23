using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class JumpToRoomCommand : ICommand
    {
        private readonly RoomManager _roomManager;
        private readonly int _roomIndex;

        public JumpToRoomCommand(RoomManager roomManager, int roomIndex)
        {
            _roomManager = roomManager;
            _roomIndex = roomIndex;
        }

        public void Execute()
        {
            _roomManager.JumpToRoomIndex(_roomIndex);
        }
    }
}
