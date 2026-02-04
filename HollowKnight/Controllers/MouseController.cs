using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using HollowKnight.Interfaces;

namespace HollowKnight.Controllers
{
    /// <summary>
    /// Mouse input controller.
    /// Maps mouse clicks to ICommand objects and handles mouse-based movement.
    /// TODO: Customize this for your game's mouse controls.
    /// </summary>
    public class MouseController : IController
    {
        private ICommand _topLeftCommand;
        private ICommand _topRightCommand;
        private ICommand _rightClickCommand;
        private MouseState _previousState;
        private int _screenWidth;
        private int _screenHeight;
        private Game1 _game;
        private float _moveSpeed = 150f;

        public MouseController(int screenWidth, int screenHeight, Game1 game)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _game = game;
            _previousState = Mouse.GetState();
        }

        /// <summary>
        /// Register commands for left-click in top screen quadrants.
        /// </summary>
        public void RegisterLeftClickCommands(ICommand topLeft, ICommand topRight)
        {
            _topLeftCommand = topLeft;
            _topRightCommand = topRight;
        }

        /// <summary>
        /// Register a command for right-click.
        /// </summary>
        public void RegisterRightClickCommand(ICommand command)
        {
            _rightClickCommand = command;
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentState = Mouse.GetState();
            int x = currentState.X;
            int y = currentState.Y;
            int halfWidth = _screenWidth / 2;
            int halfHeight = _screenHeight / 2;

            // Left click
            if (currentState.LeftButton == ButtonState.Pressed)
            {
                // Top quadrants - tap to execute command (only on press)
                if (y < halfHeight && _previousState.LeftButton == ButtonState.Released)
                {
                    if (x < halfWidth)
                    {
                        _topLeftCommand?.Execute();
                    }
                    else
                    {
                        _topRightCommand?.Execute();
                    }
                }
                // Bottom quadrants - hold to move sprite
                else if (y >= halfHeight)
                {
                    float delta = _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Vector2 currentPos = _game.GetCurrentSpritePosition();

                    if (x < halfWidth)
                    {
                        currentPos.Y += delta;
                        if (currentPos.Y > _screenHeight)
                        {
                            currentPos.Y = -50;
                        }
                    }
                    else
                    {
                        currentPos.X += delta;
                        if (currentPos.X > _screenWidth)
                        {
                            currentPos.X = -50;
                        }
                    }
                    _game.SetCurrentSpritePosition(currentPos);
                }
            }

            // Right click
            if (currentState.RightButton == ButtonState.Pressed && _previousState.RightButton == ButtonState.Released)
            {
                _rightClickCommand?.Execute();
            }

            _previousState = currentState;
        }
    }
}
