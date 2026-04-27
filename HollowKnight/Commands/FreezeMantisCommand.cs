using HollowKnight.Enemies;
using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class FreezeMantisCommand : ICommand
    {
        private readonly MantisLord _mantisLord;

        public FreezeMantisCommand(MantisLord mantisLord)
        {
            _mantisLord = mantisLord;
        }

        public void Execute()
        {
            _mantisLord.StateMachine.ForceStartFight();
            _mantisLord.StateMachine.ToggleFreeze();
        }
    }
}
