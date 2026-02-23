using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerHealCancelCommand : ICommand
    {
        private TheKnight knight;

        public PlayerHealCancelCommand(TheKnight knight)
        {
            this.knight = knight;
        }

        public void Execute()
        {
            knight.CancelHeal();
        }
    }
}