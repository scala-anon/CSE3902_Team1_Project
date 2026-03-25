using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Storage;

namespace HollowKnight.Commands
{
    public class PlayerUseItemCommand : ICommand
    {
        private readonly TheKnight _knight;
        private readonly int _itemNumber;

        public PlayerUseItemCommand(TheKnight knight, int itemNumber)
        {
            _knight = knight;
            _itemNumber = itemNumber;
        }

        public void Execute() => _knight.UseItem(_itemNumber);
    }

    public class CycleItemNextCommand : ICommand
    {
        public void Execute() => ItemManager.NextItem();
    }

    public class CycleItemPreviousCommand : ICommand
    {
        public void Execute() => ItemManager.PreviousItem();
    }
}
