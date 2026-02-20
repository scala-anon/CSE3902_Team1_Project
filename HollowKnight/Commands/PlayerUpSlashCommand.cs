using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerUpSlashCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerUpSlashCommand(TheKnight knight)
        {
            _knight = knight;
        }
        public void Execute()
        {
            _knight.UpSlash();
        }
    }
}