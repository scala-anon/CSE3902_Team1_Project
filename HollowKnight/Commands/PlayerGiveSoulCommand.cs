using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerGiveSoulCommand : ICommand
    {
        private readonly TheKnight _knight;

        public PlayerGiveSoulCommand(TheKnight knight)
        {
            _knight = knight;
        }

        public void Execute()
        {
            _knight.GiveFullSoul();
        }
    }
}
