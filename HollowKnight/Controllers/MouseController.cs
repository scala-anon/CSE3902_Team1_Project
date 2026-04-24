using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HollowKnight.Controllers
{
    public class MouseController : IController
    {
        private readonly Game1 _game;
        private readonly int _screenWidth;
        private MouseState _previousState;

        public MouseController(Game1 game, int screenWidth)
        {
            _game = game;
            _screenWidth = screenWidth;
            _previousState = Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentState = Mouse.GetState();
            bool leftClickPressed = currentState.LeftButton == ButtonState.Pressed &&
                                    _previousState.LeftButton == ButtonState.Released;

            if (leftClickPressed)
            {
                if (_game.TrySelectInventoryItem(new Point(currentState.X, currentState.Y)))
                {
                    _previousState = currentState;
                    return;
                }

                if (_game.AllowsGameplayInput())
                {
                    int direction = currentState.X < _screenWidth / 2 ? -1 : 1;
                    _game.QuickNavigateHorizontally(direction);
                }
            }

            _previousState = currentState;
        }
    }
}
