namespace HollowKnight.Shared
{
    public static class KnightConstants
    {
        public const float KnightMoveSpeed = 250f; //prev 200
        public const float KnightJumpSpeed = -700f;
        public const float KnightGravity = 900f;
        public const float KnightKnockbackSpeed = 250f;
        public const float KnightKnockbackUpwards = -300f;
        public const float KnightKnockbackDuration = 0.3f;
        public const float KnightKnockbackTimer = 0f; // Start at max so not in knockback

        // Knight — Combat
        public const double KnightAttackDuration = 0.25;
        public const double KnightAttackCooldown = 0.41;

        // Knight — Health
        public const int KnightStartHealth = 5;
        public const int KnightMaxHealth = 9;
        public const double KnightInvincibilityDuration = 1.3;
        public const double KnightHealPrepDuration = 0.6;
        public const double KnightHealPostDuration = 0.2;
        public const int KnightHurtboxShrink = 6;

        //Damage
        public const int KnightDamage = 1;

        // Sword Hitbox Dimensions
        public const int SideSlashWidth = 160;
        public const int SideSlashHeight = 110;
        public const int UpSlashWidth = 90;
        public const int UpSlashHeight = 120;
        public const int DownSlashWidth = 90;
        public const int DownSlashHeight = 120;

        // Slash Effect Positioning (divisors for fractional offset)
        public const float SlashEffectRightDivisor = 7f;
        public const float SlashEffectLeftDivisor = 5f;
        public const float SlashEffectYDivisor = 10f;
        public const float UpSlashEffectYDivisor = 4f;
        public const float DownSlashEffectXDivisor = 10f;
        public const float DownSlashEffectYDivisor = 3f;
    }
}