using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerHealHoldCommand : ICommand
    {
        private TheKnight knight;

        public PlayerHealHoldCommand(TheKnight knight)
        {
            this.knight = knight;
        }

        public void Execute()
        {
            knight.StartHeal();
        }
    }
}