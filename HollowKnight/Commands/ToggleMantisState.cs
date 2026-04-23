using HollowKnight.Enemies;
using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class ToggleMantisStateCommand : ICommand
    {
        private readonly MantisLord _mantisLord;

        private static readonly MantisLordState[] _states = new[]
        {
            MantisLordState.IdleOnThrone,
            MantisLordState.WallArrive,
            MantisLordState.DashArrive,
            MantisLordState.DStabArrive,
            MantisLordState.ThroneWounded,
            MantisLordState.ThroneBow,
            MantisLordState.Death,
            MantisLordState.Dormant
        };

        private int _currentIndex = 0;

        public ToggleMantisStateCommand(MantisLord mantisLord)
        {
            _mantisLord = mantisLord;
        }

        public void Execute()
        {
            _currentIndex = (_currentIndex + 1) % _states.Length;
            _mantisLord.SetState(_states[_currentIndex]);
        }
    }
}