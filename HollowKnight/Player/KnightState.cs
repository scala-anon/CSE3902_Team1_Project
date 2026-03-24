namespace HollowKnight.Player
{
    // TODO: Replace the boolean flag soup in TheKnight (isAttacking, isDamaged, isHealing, isGrounded)
    // with this enum. Mutually exclusive states should not be independent booleans.
    // Wire this into TheKnight.Update() to drive sprite selection and audio triggers.
    public enum KnightState
    {
        Idle,
        Running,
        Jumping,
        Falling,
        Attacking,
        Healing,
        Damaged,
        Dead
    }
}
