namespace HollowKnight.Shared
{
    public static class GameConstants
    {
        // Screen
        public const int ScreenWidth = 1600;
        public const int ScreenHeight = 900;

        // Level
        public const int DefaultLevelWidth = 70000;
        public const int DefaultLevelHeight = 34000;
        public const int TransitionZoneHeight = 500; 
        public const int TransitionZoneWidth  = 500; 

        // Knight — Physics
        public const float KnightMoveSpeed = 250f;
        public const float KnightJumpSpeed = -700f;
        public const float KnightGravity = 900f;
        public const float KnightKnockbackSpeed = 250f;
        public const float KnightKnockbackUpwards = -300f;
        public const float KnightKnockbackDuration = 0.3f;
        public const float KnightKnockbackTimer = 0f;

        // Knight — Combat
        public const double KnightAttackDuration = 0.25;
        public const double KnightAttackCooldown = 0.41;

        // Knight — Health
        public const int KnightStartHealth = 5;
        public const int KnightMaxHealth = 5;
        public const double KnightInvincibilityDuration = 1.3;
        public const double KnightHealPrepDuration = 0.6;
        public const double KnightHealPostDuration = 0.2;
        public const int KnightHurtboxShrink = 6;

        // Knight — Soul
        public const int KnightStartSoul = 0;
        public const int KnightMaxSoul = 99;
        public const int SpiritPickupSoul = 33;

        // Enemy — Shared
        public const float EnemyKnockbackSpeed = 950f;
        public const float EnemyKnockbackDecay = 8f;
        public const float EnemyDeathGravity = 600f;
        public const double EnemyDamagedDuration = 0.4;
        public const int EnemyHurtboxGrow = 6;
        public const int EnemyDefaultHealth = 3;

        // Crawlid
        public const float CrawlidPatrolSpeed = 175f;
        public const float CrawlidTurnDuration = 0.08f;

        // Vengefly
        public const float VengeflyDetectionRadius = 500f;
        public const float VengeflyPatrolSpeed = 95f;
        public const float VengeflyChaseSpeed = 130f;
        public const double VengeflyStartleDuration = 0.5;
        public const float VengeflyPathUpdateInterval = 0.3f;
        public const float VengeflyKnockbackUpComponent = -150f;
        public const float VengeflyVerticalKnockbackSpeed = 250f;

        // Damage
        public const int KnightDamage = 1;
        public const int CrawlidDamage = 1;
        public const int VengeflyDamage = 1;

        // Sword Hitbox Dimensions
        public const int SideSlashWidth = 160;
        public const int SideSlashHeight = 110;
        public const int UpSlashWidth = 90;
        public const int UpSlashHeight = 120;
        public const int DownSlashWidth = 90;
        public const int DownSlashHeight = 120;

        // Slash Effect Positioning
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

        // Navigation
        public const int NavGridCellSize = 12;
        public const int NavGridDefaultCellSize = 32;
        public const float NavGridOverlayOpacity = 0.2f;
        public const float PathReachedThreshold = 10f;

        // Physics Thresholds
        public const float JumpMomentumCut = 0.5f;
        public const float KnockbackVelocityThreshold = 1f;
        public const int GroundProbeExtension = 1;

        // Environment Damage
        public const int SpikeDamage = 1;
        public const int InvalidPositionSentinel = -9999;

        // Projectile — Base
        public const int ProjectileDefaultSize = 12;
        public const int ProjectileDefaultDamage = 1;
        public const float ProjectileAntiTunnelingStep = 5f;

        // Audio
        public const float NoVolume = 0.0f;
        public const float MaxVolume = 1.0f;
        public const float Pan = 0.0f;
        public const float Pitch = 0.0f;
        public const float SongVolume = 0.1f;

        // A* Pathfinding
        public const int AStarMaxIterations = 1000;
        public const int AStarStraightMoveCost = 10;
        public const int AStarDiagonalMoveCost = 14;

        // Debug Logging (1 = enabled, 0 = disabled)
        public const int DEBUG_LOG_ENABLED = 1;

        // Rendering — Layer Depth (FrontToBack: 0.0 = back, 1.0 = front)
        public const float LayerDepthBackgroundFar = 0.00f;
        public const float LayerDepthBackgroundMid = 0.10f;
        public const float LayerDepthPlatform = 0.20f;
        public const float LayerDepthInteractable = 0.30f;
        public const float LayerDepthItem = 0.40f;
        public const float LayerDepthEnemy = 0.60f;
        public const float LayerDepthProjectile = 0.70f;
        public const float LayerDepthKnight = 0.80f;
        public const float LayerDepthKnightEffects = 0.85f;
        public const float LayerDepthForeground = 0.90f;

        // Foreground parallax — how many times faster foreground scrolls vs the camera
        public const float ForegroundParallaxFactor = 1.5f;
        public const float LayerDepthDebug = 1.00f;

        // Per-instance sub-depth offset used to break ties within a layer.
        // FrontToBack sort is unstable; identical depths can flicker when batch size changes
        // (e.g. a slash sprite appears mid-frame). Each object in a bucket gets baseDepth + i*epsilon.
        public const float LayerDepthEpsilon = 0.0001f;
    }
}
