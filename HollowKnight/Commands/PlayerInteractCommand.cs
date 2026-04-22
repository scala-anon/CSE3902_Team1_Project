using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerInteractCommand : ICommand
    {
        private readonly Game1 _game;

        public PlayerInteractCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute() => _game.TryInteract();
    }
}