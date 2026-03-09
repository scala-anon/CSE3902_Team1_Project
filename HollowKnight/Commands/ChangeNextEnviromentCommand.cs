
using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class ChangeNextEnviromentCommand : ICommand
    {
           private Game1 _game1;

           public ChangeNextEnviromentCommand(Game1 game1)
        {
            _game1 = game1;
        }

        public void Execute()
        {
            _game1.enviroment_index++;
            if(_game1.enviroment_index == 17)
            {
                _game1.enviroment_index = 0;
            }
        }
    }

}