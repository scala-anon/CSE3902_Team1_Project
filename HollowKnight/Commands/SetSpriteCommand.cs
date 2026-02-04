using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    /// <summary>
    /// Command to switch the currently displayed sprite.
    /// Example usage: Cycle through player animations, switch weapons, etc.
    /// </summary>
    public class SetSpriteCommand : ICommand
    {
        private Game1 _game;
        private ISprite _sprite;

        public SetSpriteCommand(Game1 game, ISprite sprite)
        {
            _game = game;
            _sprite = sprite;
        }

        public void Execute()
        {
            _game.SetSprite(_sprite);
        }
    }
}
