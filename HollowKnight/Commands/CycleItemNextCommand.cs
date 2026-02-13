using HollowKnight.Interfaces;
using HollowKnight.Storage;

namespace HollowKnight.Commands
{
    public class CycleItemNextCommand : ICommand
    {
        public CycleItemNextCommand() { }

        public void Execute()
        {
            ItemManager.NextItem();
        }
    }
}