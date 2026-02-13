using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerAttackCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerAttackCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.Attack();
        }
    }
}