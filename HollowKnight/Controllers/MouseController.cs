using HollowKnight.Interfaces;
using HollowKnight.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HollowKnight.Controllers
{
    public class MouseController : IController
    {
        private readonly int _screenWidth;
        private readonly RoomManager _roomManager;
        private MouseState _previousState;

        public MouseController(int screenWidth, RoomManager roomManager)
        {
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
                if (currentState.X < _screenWidth / 2)
                {
                    _roomManager.SwitchRoomByOffset(-1);
                }
                else
                {
                    _roomManager.SwitchRoomByOffset(1);
                }
            }

            _previousState = currentState;
        }
    }
}
