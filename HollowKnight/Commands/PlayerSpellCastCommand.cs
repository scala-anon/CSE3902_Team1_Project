using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerSpellCastCommand : ICommand
    {
        private readonly TheKnight _knight;

        public PlayerSpellCastCommand(TheKnight knight)
        {
            _knight = knight;
        }

        public void Execute()
        {
            _knight.CastSpell();
        }
    }
}
