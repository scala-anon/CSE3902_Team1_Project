using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Storage;

namespace HollowKnight.Commands
{
    public class PlayerUseItemCommand : ICommand
    {
        private readonly Game1 _game;
        private readonly TheKnight _knight;
        private readonly int _itemNumber;

        public PlayerUseItemCommand(Game1 game, TheKnight knight, int itemNumber)
        {
            _game = game;
            _knight = knight;
            _itemNumber = itemNumber;
        }

        public void Execute()
        {
            ItemManager.SelectItem(_itemNumber - 1);

            if (_game.AllowsGameplayInput())
            {
                _knight.UseItem(_itemNumber);
            }
        }
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
