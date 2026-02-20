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

        private class ComboPressedBinding
        {
            public Keys BaseKey;
            public Keys? ModifierKey;
            public ICommand Command; 
        } 
        private List<ComboPressedBinding> _comboPressedMappings;

        public KeyboardController()
        {
            _pressedMappings = new Dictionary<Keys, ICommand>();
            _heldMappings = new Dictionary<Keys, ICommand>();
            _releasedMappings = new Dictionary<Keys, ICommand>();
            _comboPressedMappings = new List<ComboPressedBinding>();
            _previousState = Keyboard.GetState();
        }

        /// <summary>
        /// Register a command to execute when the specified key is pressed.
        /// </summary>
        /// 
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
        public void RegisterComboPressedCommand(Keys baseKey, Keys modifierKey, ICommand command)
        {
            _comboPressedMappings.Add(new ComboPressedBinding
            {
                BaseKey = baseKey,
                ModifierKey = modifierKey,
                Command = command
            });
        }
        public void RegisterComboPressedCommand(Keys baseKey, ICommand command)
        {
            _comboPressedMappings.Add(new ComboPressedBinding
            {
                BaseKey = baseKey,
                ModifierKey = null,
                Command = command
            });
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            foreach (var binding in _comboPressedMappings)
            {
                bool basePressed = currentState.IsKeyDown(binding.BaseKey) && _previousState.IsKeyUp(binding.BaseKey);

                if (!basePressed)
                    continue;

                if (binding.ModifierKey != null)
                {
                    if (currentState.IsKeyDown(binding.ModifierKey.Value))
                    {
                        binding.Command.Execute();
                        _previousState = currentState;
                        return;
                    }
                }
                else
                {
                    binding.Command.Execute();
                    _previousState = currentState;
                    return;
                }
            }

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
