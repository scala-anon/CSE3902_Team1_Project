using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerMoveRightCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerMoveRightCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.MoveRight();
        }
    }
}