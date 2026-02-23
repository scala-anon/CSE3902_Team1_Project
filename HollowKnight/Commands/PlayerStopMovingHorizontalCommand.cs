using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerStopMovingHorizontalCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerStopMovingHorizontalCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.StopMovingHorizontal();
        }
    }
}