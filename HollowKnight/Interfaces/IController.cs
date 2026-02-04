using Microsoft.Xna.Framework;

namespace HollowKnight.Interfaces
{
    /// <summary>
    /// Interface for input controllers.
    /// Implement this for keyboard, mouse, gamepad, etc.
    /// Controllers map input to ICommand objects.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Poll input and execute registered commands.
        /// Called once per game tick.
        /// </summary>
        void Update(GameTime gameTime);
    }
}
