using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerJumpCommand : ICommand
    {
        private TheKnight _knight;
        public PlayerJumpCommand(TheKnight knight)
        {
            _knight = knight;
        }

        public void Execute()
        {
            _knight.Jump();
        }
    }
}