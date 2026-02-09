using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class PlayerMoveLeftCommand : ICommand
    {
        private IHollowKnight theKnight;
        public PlayerMoveLeftCommand(IHollowKnight theKnight)
        {
            this.theKnight = theKnight;
        }
        public void Execute()
        {
            theKnight.MoveLeft();
        }
    }
}