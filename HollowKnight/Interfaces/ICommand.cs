namespace HollowKnight.Interfaces
{
    /// <summary>
    /// Command pattern interface.
    /// Encapsulates actions as objects for flexible input binding.
    /// Use this for player actions, menu navigation, debug commands, etc.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute the command action.
        /// </summary>
        void Execute();
    }
}
