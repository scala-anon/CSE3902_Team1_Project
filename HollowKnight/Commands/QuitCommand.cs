using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    /// <summary>
    /// Command to exit the game.
    /// Example usage: Bind to Escape key or quit button.
    /// </summary>
    public class QuitCommand : ICommand
    {
        private Game _game;

        public QuitCommand(Game game)
        {
            _game = game;
        }

        public void Execute()
        {
            _game.Exit();
        }
    }
}
