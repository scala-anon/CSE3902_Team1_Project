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
        private Dictionary<Keys, ICommand> _pressedMappings;
        private Dictionary<Keys, ICommand> _heldMappings;
        private Dictionary<Keys, ICommand> _releasedMappings;

        private KeyboardState _previousState;

        public KeyboardController()
        {
            _pressedMappings = new Dictionary<Keys, ICommand>();
            _heldMappings = new Dictionary<Keys, ICommand>();
            _releasedMappings = new Dictionary<Keys, ICommand>();
            _previousState = Keyboard.GetState();
        }

        /// <summary>
        /// Register a command to execute when the specified key is pressed.
        /// </summary>
        public void RegisterPressedCommand(Keys key, ICommand command)
        {
            _pressedMappings[key] = command;
        }
        public void RegisterHeldCommand(Keys key, ICommand command)
        {
            _heldMappings[key] = command;
        }
        public void RegisterReleasedCommand(Keys key, ICommand command)
        {
            _releasedMappings[key] = command;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            foreach (var mapping in _pressedMappings)
            {
                Keys key = mapping.Key;

                if (currentState.IsKeyDown(key) && _previousState.IsKeyUp(key))
                {
                    mapping.Value.Execute();
                }
            }

            foreach (var mapping in _heldMappings)
            {
                Keys key = mapping.Key;

                if (currentState.IsKeyDown(key) && _previousState.IsKeyDown(key))
                {
                    mapping.Value.Execute();
                }
            }

            foreach (var mapping in _releasedMappings)
            {
                Keys key = mapping.Key;

                if (currentState.IsKeyUp(key) && _previousState.IsKeyDown(key))
                {
                    mapping.Value.Execute();
                }
            }

            _previousState = currentState;
        }
    }
}
