namespace HollowKnight.Shared
{
    // TODO: Extract all magic numbers from TheKnight, Crawlid, Vengefly,
    // CrawlidStateMachine, VengeflyStateMachine, and Game1 into this class.
    // Changing screen resolution or tuning physics should mean editing one file.
    public static class GameConstants
    {
        // Screen
        // TODO: Replace hardcoded 1280/720 in Game1, enemy classes, and state machines
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;

        // Level
        // TODO: Replace hardcoded 3200/720 in Game1
        public const int DefaultLevelWidth = 3200;
        public const int DefaultLevelHeight = 720;

        // Player Physics
        // TODO: Extract from TheKnight fields
        // public const float PlayerMoveSpeed = ...;
        // public const float PlayerJumpSpeed = ...;
        // public const float Gravity = ...;

        // Player Combat
        // TODO: Extract from TheKnight fields
        // public const float AttackCooldown = ...;
        // public const float AttackDuration = ...;
        // public const float InvincibilityDuration = ...;
        // public const float KnockbackSpeed = ...;

        // Enemy Defaults
        // TODO: Extract from Crawlid/Vengefly fields
        // public const float CrawlidSpeed = ...;
        // public const float VengeflySpeed = ...;
    }
}
