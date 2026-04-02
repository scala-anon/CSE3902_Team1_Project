using HollowKnight.Interfaces;
using HollowKnight.Player;

namespace HollowKnight.Commands
{
    public class PlayerMoveLeftCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerMoveLeftCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.MoveLeft();
    }

    public class PlayerMoveRightCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerMoveRightCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.MoveRight();
    }

    public class PlayerMoveUpCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerMoveUpCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.MoveUp();
    }

    public class PlayerMoveDownCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerMoveDownCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.MoveDown();
    }

    public class PlayerJumpCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerJumpCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.Jump();
    }

    public class PlayerStopMovingHorizontalCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerStopMovingHorizontalCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.StopMovingHorizontal();
    }

    public class PlayerStopMovingVerticalCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerStopMovingVerticalCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.StopMovingVertical();
    }
}
