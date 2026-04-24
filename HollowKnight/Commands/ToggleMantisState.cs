// using HollowKnight.Enemies;
// using HollowKnight.Interfaces;

// namespace HollowKnight.Commands
// {
//     public class ToggleMantisStateCommand : ICommand
//     {
//         private readonly MantisLord _mantisLord;

//         private static readonly MantisLordState[] _states = new[]
//         {
//             MantisLordState.IdleOnThrone,
//             MantisLordState.ThroneStand,
//             MantisLordState.ThroneLeave,
//             MantisLordState.WallArrive,
//             MantisLordState.WallReady,
//             MantisLordState.WallLeave,
//             MantisLordState.Throw,
//             MantisLordState.DashArrive,
//             MantisLordState.DashAnticipate,
//             MantisLordState.Dash,
//             MantisLordState.DashRecover,
//             MantisLordState.DashLeave,
//             MantisLordState.DStabArrive,
//             MantisLordState.DStab,
//             MantisLordState.DStabLand,
//             MantisLordState.DStabLeave,
//             MantisLordState.ThroneArrive,
//             MantisLordState.ThroneWounded,
//             MantisLordState.ThroneBow,
//             MantisLordState.Death,
//             MantisLordState.DeathLeaveOne,
//             MantisLordState.DeathLeaveTwo,
//             MantisLordState.Dormant,
//         };

//         private int _currentIndex = 0;

//         public ToggleMantisStateCommand(MantisLord mantisLord)
//         {
//             _mantisLord = mantisLord;
//         }

//         public void Execute()
//         {
//             _currentIndex = (_currentIndex + 1) % _states.Length;
//             _mantisLord.SetState(_states[_currentIndex]);
//         }
//     }
// }

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
            MantisLordState.ThroneStand,
            MantisLordState.ThroneLeave,
            MantisLordState.WallArrive,
            MantisLordState.WallReady,
            MantisLordState.WallLeave,
            MantisLordState.Throw,
            MantisLordState.DashArrive,
            MantisLordState.DashAnticipate,
            MantisLordState.Dash,
            MantisLordState.DashRecover,
            MantisLordState.DashLeave,
            MantisLordState.DStabArrive,
            MantisLordState.DStab,
            MantisLordState.DStabLand,
            MantisLordState.DStabLeave,
            MantisLordState.ThroneArrive,
            MantisLordState.ThroneWounded,
            MantisLordState.ThroneBow,
            MantisLordState.Death,
            MantisLordState.DeathLeaveOne,
            MantisLordState.DeathLeaveTwo,
            MantisLordState.Dormant
        };

        private int _currentIndex = 0;

        public ToggleMantisStateCommand(MantisLord mantisLord)
        {
            _mantisLord = mantisLord;
        }

        public void Execute()
        {
            if (!_mantisLord.StateMachine.IsFrozen) return; // only cycle when frozen
            _currentIndex = (_currentIndex + 1) % _states.Length;
            MantisLordState next = _states[_currentIndex];
            _mantisLord.StateMachine.SnapPositionForState(next);
            _mantisLord.SetState(next);
        }
    }
}