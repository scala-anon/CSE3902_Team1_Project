using HollowKnight.Interfaces;
using HollowKnight.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HollowKnight.Controllers
{
    public class MouseController : IController
    {
        private readonly Game1 _game;
        private readonly int _screenWidth;
        private readonly RoomManager _roomManager;
        private MouseState _previousState;

        public MouseController(Game1 game, int screenWidth, RoomManager roomManager)
        {
            _game = game;
            _screenWidth = screenWidth;
            _roomManager = roomManager;
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
                    if (currentState.X < _screenWidth / 2)
                    {
                        _roomManager.SwitchRoomByOffset(-1);
                    }
                    else
                    {
                        _roomManager.SwitchRoomByOffset(1);
                    }
                }
            }

            _previousState = currentState;
        }
    }
}
