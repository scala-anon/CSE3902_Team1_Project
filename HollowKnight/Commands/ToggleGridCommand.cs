using HollowKnight.Interfaces;
using HollowKnight.Pathfinding;

namespace HollowKnight.Commands
{
    public class ToggleGridCommand : ICommand
    {
        public ToggleGridCommand(Game1 game) { }

        public void Execute()
        {
            NavigationGrid.GridEnabled = !NavigationGrid.GridEnabled;
        }
    }
}
