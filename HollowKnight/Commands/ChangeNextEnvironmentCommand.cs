
using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class ChangeNextEnvironmentCommand : ICommand
    {
           private Game1 _game1;

           public ChangeNextEnvironmentCommand(Game1 game1)
        {
            _game1 = game1;
        }

        public void Execute()
        {
            _game1.SwitchToNextRoom();
        }
    }

}
