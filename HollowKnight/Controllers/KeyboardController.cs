using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using HollowKnight.Interfaces;
using System.Collections.Generic;

namespace HollowKnight.Controllers
{
    /// <summary>
    /// Keyboard input controller.
    /// Maps keyboard keys to ICommand objects.
    /// Commands execute on key press (not hold) by default.
    /// </summary>
    public class KeyboardController : IController
    {
        private Dictionary<Keys, ICommand> _keyMappings;
        private KeyboardState _previousState;

        public KeyboardController()
        {
            _keyMappings = new Dictionary<Keys, ICommand>();
            _previousState = Keyboard.GetState();
        }

        /// <summary>
        /// Register a command to execute when the specified key is pressed.
        /// </summary>
        public void RegisterCommand(Keys key, ICommand command)
        {
            _keyMappings[key] = command;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            foreach (var mapping in _keyMappings)
            {
                // Only trigger on key press (not hold)
                if (currentState.IsKeyDown(mapping.Key) && _previousState.IsKeyUp(mapping.Key))
                {
                    mapping.Value.Execute();
                }
            }

            _previousState = currentState;
        }
    }
}
