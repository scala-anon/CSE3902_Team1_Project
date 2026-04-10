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
        public const float SongVolume = 0.5f;

        // A* Pathfinding
        public const int AStarMaxIterations = 1000;
        public const int AStarStraightMoveCost = 10;
        public const int AStarDiagonalMoveCost = 14;
    }
}
