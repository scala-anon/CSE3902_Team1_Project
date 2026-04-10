namespace HollowKnight.Shared
{
    public static class EnemyConstants
    {
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
        public const float VengeflyChaseRadius = 1200f;
        public const float VengeflyPatrolSpeed = 95f; //prev 75
        public const float VengeflyChaseSpeed = 130f; //prev 100
        public const double VengeflyStartleDuration = 0.5;
        public const float VengeflyPathUpdateInterval = 0.3f;
        public const float VengeflyKnockbackUpComponent = -150f;
        public const float VengeflyVerticalKnockbackSpeed = 250f; //used for death
        public const float VengeflyWaypointReachDivisor = 1.5f;

        // Damage
        public const int CrawlidDamage = 1;
        public const int VengeflyDamage = 1;
    }
}