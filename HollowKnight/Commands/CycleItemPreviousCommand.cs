using HollowKnight.Interfaces;
using HollowKnight.Storage;

namespace HollowKnight.Commands
{
    public class CycleItemPreviousCommand : ICommand
    {
        public CycleItemPreviousCommand() { }

        public void Execute()
        {
            ItemManager.PreviousItem();
        }
    }
}