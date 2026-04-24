using HollowKnight.Enemies;
using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class KillMantisCommand : ICommand
    {
        private readonly MantisLord _mantisLord;

        public KillMantisCommand(MantisLord mantisLord)
        {
            _mantisLord = mantisLord;
        }

        public void Execute()
        {
            _mantisLord.StateMachine.OnHealthDepleted();
        }
    }
}