using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class PlayerAttackCommand : ICommand
    {
        private IHollowKnight theKnight;
        public PlayerAttackCommand(IHollowKnight theKnight)
        {
            this.theKnight = theKnight;
        }
        public void Execute()
        {
            theKnight.Attack();
        }
    }
}