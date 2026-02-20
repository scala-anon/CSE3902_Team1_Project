using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerUseItemCommand : ICommand
    {
        private TheKnight _knight;
        private int _itemNumber;

        public PlayerUseItemCommand(TheKnight knight, int itemNumber)
        {
            _knight = knight;
            _itemNumber = itemNumber;
        }

        public void Execute()
        {
            _knight.UseItem(_itemNumber);
        }
    }
}