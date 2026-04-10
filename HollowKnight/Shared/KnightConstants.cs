namespace HollowKnight.Shared
{
    public static class KnightConstants
    {
        // Physics
        public const float KnightMoveSpeed = 250f;
        public const float KnightJumpSpeed = -700f;
        public const float KnightGravity = 900f;
        public const float KnightKnockbackSpeed = 250f;
        public const float KnightKnockbackUpwards = -300f;
        public const float KnightKnockbackDuration = 0.3f;
        public const float KnightKnockbackTimer = 0f;

        // Combat
        public const double KnightAttackDuration = 0.25;
        public const double KnightAttackCooldown = 0.41;
        public const int KnightDamage = 1;

        // Dash
        public const float KnightDashSpeed = 600f;
        public const float KnightDashDuration = 0.5f;
        public const float KnightDashCooldown = 0.65f;

        // Soul
        public const int KnightStartSoul = 0;
        public const int KnightMaxSoul = 99;
        public const int KnightSoulPerHeal = 33;
        public const int KnightSoulPerHit = 11;
        public const int KnightSpellCastSoulCost = 36;

        // Health
        public const int KnightStartHealth = 5;
        public const int KnightMaxHealth = 5;
        public const double KnightInvincibilityDuration = 1.3;
        public const double KnightHealPrepDuration = 0.6;
        public const double KnightHealPostDuration = 0.2;
        public const double KnightHealCooldown = 2.0;
        public const double KnightHealStartUp = 0.25;

        // Slash Effect Positioning (visual offsets, not hitbox — hitbox dims are in CollisionConstants)
        public const float SlashEffectRightDivisor = 7f;
        public const float SlashEffectLeftDivisor = 5f;
        public const float SlashEffectYDivisor = 10f;
        public const float UpSlashEffectYDivisor = 4f;
        public const float DownSlashEffectXDivisor = 10f;
        public const float DownSlashEffectYDivisor = 3f;

        // Cast Pulse
        public const double KnightCastPulseDuration = 0.4;
        public const float CastPulseYDivisor = 2f;
        public const float KnightCastKnockbackSpeed = 90f;

        // Projectile (Vengeful Spirit)
        public const float KnightProjectileInterval = 2f;
        public const float KnightProjectileSpeed = 800f;
        public const float KnightProjectileSpawnOffsetRight = 20f;
        public const float KnightProjectileSpawnOffsetLeft = 180f;
        public const double VengefulSpiritImpactDuration = 0.5;
    }
}
