using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class PlayerMoveRightCommand : ICommand
    {
        private IHollowKnight theKnight;
        public PlayerMoveRightCommand(IHollowKnight theKnight) {
            this.theKnight = theKnight;
        }
        public void Execute()
        {
            theKnight.MoveRight();
        }
    }
}