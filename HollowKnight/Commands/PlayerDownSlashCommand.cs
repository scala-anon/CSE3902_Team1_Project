using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerDownSlashCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerDownSlashCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.DownSlash();
        }
    }
}