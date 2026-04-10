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
        public const float EnemyKnockbackStopThreshold = 1f;
        public const float EnemyDeathGravity = 600f;
        public const double EnemyDamagedDuration = 0.4;
        public const int EnemyHurtboxGrow = 6;
        public const int EnemyDefaultHealth = 3;

        // Crawlid
        public const float CrawlidPatrolSpeed = 175f; //prev 120
        public const float CrawlidTurnDuration = 0.08f;

        // Vengefly
        public const float VengeflyDetectionRadius = 500f;
        public const float VengeflyChaseRadius = 1200f; //prev 800, 1200 causes lag
        public const float VengeflyPatrolSpeed = 75f;
        public const float VengeflyChaseSpeed = 120f;
        public const double VengeflyStartleDuration = 0.5;
        public const float VengeflyPathUpdateInterval = 0.3f;
        public const float VengeflyKnockbackUpComponent = -150f;
        public const float VengeflyVerticalKnockbackSpeed = 250f; //used for death
        public const float VengeflyWaypointReachDivisor = 1.5f;

        // Navigation
        public const int NavGridCellSize = 12;
        public const int NavGridDefaultCellSize = 32;
        public const float NavGridOverlayOpacity = 0.2f;
        public const float PathReachedThreshold = 10f;

        //Damage
        public const int KnightDamage = 1;
        public const int CrawlidDamage = 1;
        public const int VengeflyDamage = 1;
        public const int SpikeDamage = 1;

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

        // Spike Hitboxes — Floor1
        public const int SpikeFloor1PrimaryY = 50;
        public const int SpikeFloor1PrimaryW = 165;
        public const int SpikeFloor1PrimaryH = 45;
        public const int SpikeFloor1SecondaryX = 60;
        public const int SpikeFloor1SecondaryW = 75;
        public const int SpikeFloor1SecondaryH = 50;

        // Spike Hitboxes — Floor2
        public const int SpikeFloor2PrimaryY = 40;
        public const int SpikeFloor2PrimaryW = 140;
        public const int SpikeFloor2PrimaryH = 60;
        public const int SpikeFloor2SecondaryX = 40;
        public const int SpikeFloor2SecondaryW = 100;
        public const int SpikeFloor2SecondaryH = 40;

        // Spike Hitboxes — Ceiling
        public const int SpikeCeilingPrimaryW = 230;
        public const int SpikeCeilingPrimaryH = 70;
        public const int SpikeCeilingSecondaryX = 50;
        public const int SpikeCeilingSecondaryY = 70;
        public const int SpikeCeilingSecondaryW = 115;
        public const int SpikeCeilingSecondaryH = 60;

        // Physics Thresholds
        public const float JumpMomentumCut = 0.5f;
        public const float KnockbackVelocityThreshold = 1f;
        public const int GroundProbeExtension = 1;

        // Audio
        public const float NoVolume = 0.0f;
        public const float MaxVolume = 1.0f;
        public const float Pan = 0.0f;
        public const float Pitch = 0.0f;

        // A* Pathfinding
        public const int AStarMaxIterations = 1000;
        public const int AStarStraightMoveCost = 10;
        public const int AStarDiagonalMoveCost = 14;
    }
}
