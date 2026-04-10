namespace HollowKnight.Shared
{
    public static class EnemyConstants
    {
        // Shared
        public const float EnemyKnockbackSpeed = 950f;
        public const float EnemyKnockbackDecay = 8f;
        public const float EnemyKnockbackStopThreshold = 1f;
        public const float EnemyDeathGravity = 600f;
        public const double EnemyDamagedDuration = 0.4;
        public const int EnemyHurtboxGrow = 6;
        public const int EnemyDefaultHealth = 3;

        // Crawlid
        public const float CrawlidPatrolSpeed = 175f;
        public const float CrawlidTurnDuration = 0.08f;
        public const int CrawlidDamage = 1;

        // Vengefly
        public const float VengeflyDetectionRadius = 500f;
        public const float VengeflyChaseRadius = 1200f;
        public const float VengeflyPatrolSpeed = 95f;
        public const float VengeflyChaseSpeed = 130f;
        public const double VengeflyStartleDuration = 0.5;
        public const float VengeflyPathUpdateInterval = 0.3f;
        public const float VengeflyKnockbackUpComponent = -150f;
        public const float VengeflyVerticalKnockbackSpeed = 250f;
        public const float VengeflyWaypointReachDivisor = 1.5f;
        public const int VengeflyDamage = 1;

        // Projectile
        public const float EnemyProjectileInterval = 3f;
        public const float EnemyProjectileSpeed = 250f;
    }
}
