using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerTakeDamageCommand : ICommand
    {
        private TheKnight _knight;

        public PlayerTakeDamageCommand(TheKnight knight)
        {
            _knight = knight;
        }

        public void Execute()
        {
            _knight.TakeDamage();
        }
    }
}