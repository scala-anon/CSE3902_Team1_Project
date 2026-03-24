using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;

namespace HollowKnight.Commands
{
    public class ChangeNextEnemyCommand : ICommand
    {
        private Game1 _game1;

        public ChangeNextEnemyCommand(Game1 game1)
        {
            _game1 = game1;
        }

        public void Execute()
        {
            _game1.SwitchToNextRoom();
        }
    }
}
