using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class PlayerStopMovingHorizontalCommand : ICommand
    {
        private IHollowKnight theKnight;
        public PlayerStopMovingHorizontalCommand(IHollowKnight theKnight)
        {
            this.theKnight = theKnight;
        }
        public void Execute()
        {
            theKnight.StopMovingHorizontal();
        }
    }
}