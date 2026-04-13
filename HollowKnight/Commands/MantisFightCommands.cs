using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class StartMantisFightCommand : ICommand
    {
        private readonly Game1 _game;

        public StartMantisFightCommand(Game1 game)
        {
            _game = game;
        }

        public void Execute() => _game.StartMantisFight();
    }
}
