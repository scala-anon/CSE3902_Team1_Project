using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerSideSlashCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerSideSlashCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.SideSlash();
        }
    }
}