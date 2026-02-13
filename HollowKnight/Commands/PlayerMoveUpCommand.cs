using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerMoveUpCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerMoveUpCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.MoveUp();
        }
    }
}