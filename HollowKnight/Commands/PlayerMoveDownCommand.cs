using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerMoveDownCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerMoveDownCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.MoveDown();
        }
    }
}