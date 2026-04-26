using Microsoft.Xna.Framework;

namespace HollowKnight.Shared
{
    public static class HudConstants
    {
        // Health HUD
        public const int HealthPipWidth = 36;
        public const int HealthPipHeight = 52;
        public const int HealthPipStartX = 118;
        public const int HealthPipStartY = 28;
        public const int HealthPipSpacing = 0;

        // Soul HUD
        public const int SoulGaugeWidth = 102;
        public const int SoulGaugeHeight = 98;
        public const int SoulGaugeX = 14;
        public const int SoulGaugeY = 22;

        // HUD Colors
        public static readonly Color InactiveHealthPipColor = new(28, 32, 44, 255);
        public static readonly Color SoulGaugeMinColor = new(85, 85, 85, 255);
    }
}
