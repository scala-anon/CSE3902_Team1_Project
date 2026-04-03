using Microsoft.Xna.Framework;

namespace HollowKnight.Shared
{
    public static class CollisionConstants
    {
        // Debug Visualization
        public const int DebugBorderThickness = 2;
        public const int DebugCircleSegments = 32;
        public const int DebugTextOffsetY = 16;
        public const int DebugPointDefaultSize = 6;

        // Debug Colors
        public static readonly Color DebugColorKnight = Color.DodgerBlue;
        public static readonly Color DebugColorEnemy = Color.OrangeRed;
        public static readonly Color DebugColorEnvironment = Color.LimeGreen;
        public static readonly Color DebugColorTrigger = Color.Yellow;
        public static readonly Color DebugColorMidpoint = Color.Magenta;
        public static readonly Color DebugColorSword = Color.Cyan;

        // Knight Hurtbox
        public const int KnightHurtboxShrink = 6;

        // Sword Hitbox Dimensions
        public const int SideSlashWidth = 160;
        public const int SideSlashHeight = 110;
        public const int UpSlashWidth = 90;
        public const int UpSlashHeight = 120;
        public const int DownSlashWidth = 90;
        public const int DownSlashHeight = 120;

        // Spike Hitbox Dimensions — Floor1
        public const int SpikeFloor1PrimaryY = 50;
        public const int SpikeFloor1PrimaryW = 165;
        public const int SpikeFloor1PrimaryH = 45;
        public const int SpikeFloor1SecondaryX = 60;
        public const int SpikeFloor1SecondaryW = 75;
        public const int SpikeFloor1SecondaryH = 50;

        // Spike Hitbox Dimensions — Floor2
        public const int SpikeFloor2PrimaryY = 40;
        public const int SpikeFloor2PrimaryW = 140;
        public const int SpikeFloor2PrimaryH = 60;
        public const int SpikeFloor2SecondaryX = 40;
        public const int SpikeFloor2SecondaryW = 100;
        public const int SpikeFloor2SecondaryH = 40;

        // Spike Hitbox Dimensions — Ceiling
        public const int SpikeCeilingPrimaryW = 230;
        public const int SpikeCeilingPrimaryH = 70;
        public const int SpikeCeilingSecondaryX = 50;
        public const int SpikeCeilingSecondaryY = 70;
        public const int SpikeCeilingSecondaryW = 115;
        public const int SpikeCeilingSecondaryH = 60;

        // Physics/Collision Thresholds
        public const float GroundProbeExtension = 1;
    }
}
