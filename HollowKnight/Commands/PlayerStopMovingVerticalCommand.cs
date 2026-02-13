using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerStopMovingVerticalCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerStopMovingVerticalCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.StopMovingVertical();
        }
    }
}