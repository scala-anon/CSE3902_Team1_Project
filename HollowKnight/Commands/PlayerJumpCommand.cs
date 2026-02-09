using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class PlayerJumpCommand : ICommand
    {
        private IHollowKnight theKnight;
        public PlayerJumpCommand(IHollowKnight theKnight)
        {
            this.theKnight = theKnight;
        }

        public void Execute()
        {
            theKnight.Jump();
        }
    }
}