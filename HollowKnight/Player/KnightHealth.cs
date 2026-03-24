using Microsoft.Xna.Framework;

namespace HollowKnight.Player
{
    // TODO: Extract health/damage logic from TheKnight into this class.
    // Should own: HP, damage, invincibility timer, healing timer.
    // Expose: TakeDamage(), Heal(), CancelHeal(), Health, IsDamaged, IsHealing
    // TheKnight will own an instance and delegate health calls here.
    public class KnightHealth
    {
    }
}
