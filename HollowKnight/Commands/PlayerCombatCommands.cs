using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Collision;

namespace HollowKnight.Commands
{
    public class PlayerSideSlashCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerSideSlashCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.SideSlash();
    }

    public class PlayerUpSlashCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerUpSlashCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.UpSlash();
    }

    public class PlayerDownSlashCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerDownSlashCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.DownSlash();
    }

    public class PlayerTakeDamageCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerTakeDamageCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.TakeDamage(CollisionSide.None);
    }

    public class PlayerHealHoldCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerHealHoldCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.StartHeal();
    }

    public class PlayerHealCancelCommand : ICommand
    {
        private readonly TheKnight _knight;
        public PlayerHealCancelCommand(TheKnight knight) { _knight = knight; }
        public void Execute() => _knight.CancelHeal();
    }

    public class PlayerGainSoulCommand : ICommand
    {
        private readonly TheKnight _knight;
        private readonly int _amount;

        public PlayerGainSoulCommand(TheKnight knight, int amount)
        {
            _knight = knight;
            _amount = amount;
        }

        public void Execute() => _knight.GainSoul(_amount);
    }
}
