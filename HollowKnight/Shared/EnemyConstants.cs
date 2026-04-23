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
        public const float VengeflyDetectionRadius = 400f;
        public const float VengeflyChaseRadius = 1200f;
        public const float VengeflyPatrolSpeed = 95f;
        public const float VengeflyChaseSpeed = 130f;
        public const double VengeflyStartleDuration = 0.5;
        public const float VengeflyPathUpdateInterval = 0.3f;
        public const float VengeflyWaypointReachDivisor = 1.5f;
        public const float VengeflyPathFailBackoffInitial = 0.6f;      // first backoff delay, seconds
        public const float VengeflyPathFailBackoffMax = 2.0f;          // backoff cap, seconds
        public const float VengeflyPathFailBackoffMultiplier = 2.0f;   // growth per failure
        public const int VengeflyPathFailuresBeforeBackoff = 2;        // consecutive failures needed to trigger backoff
        public const float VengeflyKnockbackUpComponent = -150f;
        public const float VengeflyVerticalKnockbackSpeed = 250f;
        public const int VengeflyDamage = 1;

        // Projectile
        public const float EnemyProjectileInterval = 3f;
        public const float EnemyProjectileSpeed = 250f;

        // Mantis Lord
        public const int MantisLordMiddleHealth          = 36;
        public const int MantisLordSideHealth            = 18;

        public const float MantisAttackCooldown          = 1.2f;
        public const float MantisSiblingStaggerDelay     = 0.6f;
        public const float MantisWallAttackInterval      = 6.0f;
        public const float MantisPostDeathToWoundedDelay = 0.2f;
        public const float MantisFrameInterval           = 0.1f;
        public const float MantisWallReadyDuration       = 1.0f;

        public const float MantisThroneLeftOffsetX       = -245f;
        public const float MantisThroneMiddleOffsetX     =2f;
        public const float MantisThroneRightOffsetX      = 255f;
        public const float MantisPrimaryThroneOffsetY    = -35f;
        public const float MantisSecondaryThroneOffsetY = 70f;
        public const float MantisStabSpeed = 5f;
        public const float MantisDashSpeed = 5f;
    }
}
