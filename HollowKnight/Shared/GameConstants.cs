namespace HollowKnight.Shared
{
    public static class GameConstants
    {
        // Screen
        public const int ScreenWidth = 1920;
        public const int ScreenHeight = 1080;

        // Level
        public const int DefaultLevelWidth = 9500;
        public const int DefaultLevelHeight = 720;

        // Knight — Physics
        public const float KnightMoveSpeed = 200f;
        public const float KnightJumpSpeed = -700f;
        public const float KnightGravity = 900f;
        public const float KnightKnockbackSpeed = 250f;
        public const float KnightKnockbackUpwards = -300f;

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

        // Enemy — Shared
        public const float EnemyKnockbackSpeed = 950f;
        public const float EnemyKnockbackDecay = 8f;
        public const float EnemyDeathGravity = 600f;
        public const double EnemyDamagedDuration = 0.4;
        public const int EnemyHurtboxGrow = 6;
        public const int EnemyDefaultHealth = 3;

        // Crawlid
        public const float CrawlidPatrolSpeed = 120f;
        public const float CrawlidTurnDuration = 0.08f;

        // Vengefly
        public const float VengeflyDetectionRadius = 500f;
        public const float VengeflyPatrolSpeed = 75f;
        public const float VengeflyChaseSpeed = 100f;
        public const double VengeflyStartleDuration = 0.5;
        public const float VengeflyPathUpdateInterval = 0.3f;
        public const float VengeflyKnockbackUpComponent = -150f;

        // Navigation
        public const int NavGridCellSize = 12;
        public const float PathReachedThreshold = 10f;



        // Audio 
        public const float NoVolume = 0.0f;
        public const float MaxVolume = 1.0f;
        public const float HalfVolume = .5f;
        public const float Pan = 0.0f;
        public const float Pitch = 0.0f;
        public const float SongVolume = 0.1f;
    }
}
