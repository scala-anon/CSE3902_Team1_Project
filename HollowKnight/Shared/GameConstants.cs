namespace HollowKnight.Shared
{
    public static class GameConstants
    {
        // Screen
        public const int ScreenWidth = 1920;
        public const int ScreenHeight =1080;

        // Level
        public const int DefaultLevelWidth = 9500;
        public const int DefaultLevelHeight = 720;

        // Knight — Physics
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

        //Knight - Dash
        public const float KnightDashSpeed = 600f;
        public const float KnightDashDuration = 0.5f;
        public const float KnightDashCooldown = 0.65f;

        // Knight — Soul Storage/Usage
        public const int KnightStartSoul = 0;
        public const int KnightMaxSoul = 99;
        public const int KnightSoulPerHeal = 33;
        public const int KnightSoulPerHit = 11;

        //Knight - Health
        public const int KnightStartHealth = 5;
        public const int KnightMaxHealth = 5;
        public const double KnightInvincibilityDuration = 1.3;
        public const double KnightHealPrepDuration = 0.6; //actual game is 1.14
        public const double KnightHealPostDuration = 0.2;
        public const double KnightHealCooldown = 2.0;
        public const double KnightHealStartUp = 0.25;

        // Enemy — Shared
        public const float EnemyKnockbackSpeed = 950f;
        public const float EnemyKnockbackDecay = 8f;
        public const float EnemyDeathGravity = 600f;
        public const double EnemyDamagedDuration = 0.4;
        public const int EnemyHurtboxGrow = 6;
        public const int EnemyDefaultHealth = 3;

        // Crawlid
        public const float CrawlidPatrolSpeed = 175f; //prev 120
        public const float CrawlidTurnDuration = 0.08f;

        // Vengefly
        public const float VengeflyDetectionRadius = 500f;
        public const float VengeflyPatrolSpeed = 95f; //prev 75
        public const float VengeflyChaseSpeed = 130f; //prev 100
        public const double VengeflyStartleDuration = 0.5;
        public const float VengeflyPathUpdateInterval = 0.3f;
        public const float VengeflyKnockbackUpComponent = -150f;
        public const float VengeflyVerticalKnockbackSpeed = 250f; //used for death

        // Navigation
        public const int NavGridCellSize = 12;
        public const float PathReachedThreshold = 10f;

        //Damage
        public const int KnightDamage = 1;
        public const int CrawlidDamage = 1;
        public const int VengeflyDamage = 1;
        public const int SpikeDamage = 1;

        // Slash Effect Positioning (divisors for fractional offset)
        public const float SlashEffectRightDivisor = 7f;
        public const float SlashEffectLeftDivisor = 5f;
        public const float SlashEffectYDivisor = 10f;
        public const float UpSlashEffectYDivisor = 4f;
        public const float DownSlashEffectXDivisor = 10f;
        public const float DownSlashEffectYDivisor = 3f;

        // Cast Pulse Effect
        public const double KnightCastPulseDuration = 0.4;
        public const float CastPulseYDivisor = 2f;
        public const float KnightCastKnockbackSpeed = 90f;

        // Physics Thresholds
        public const float JumpMomentumCut = 0.5f;
        public const float KnockbackVelocityThreshold = 1f;
        public const int GroundProbeExtension = 1;

        // Audio
        public const float NoVolume = 0.0f;
        public const float MaxVolume = 1.0f;
        public const float Pan = 0.0f;
        public const float Pitch = 0.0f;
    }
}
