using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerMoveLeftCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerMoveLeftCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.MoveLeft();
        }
    }
}