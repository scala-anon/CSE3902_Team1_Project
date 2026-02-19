using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;

namespace HollowKnight.Commands
{
    public class ChangePreviousEnemyCommand : ICommand
    {
        private Game1 _game1;

        public ChangePreviousEnemyCommand(Game1 game1)
        {
            _game1 = game1;
        }

        public void Execute()
        {
            _game1.enemy_index--;
            if (_game1.enemy_index == -1)
            {
                _game1.enemy_index = 1;
            }
        }
    }
}