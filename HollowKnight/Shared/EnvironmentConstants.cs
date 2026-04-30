using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HollowKnight.Shared
{
    public static class EnvironmentConstants
    {
        // Breakable objects (Door, BreakableWall)
        public const float BreakableBreakDriftSpeed = 400f;
        public const float BreakableBreakAnimDuration = 0.3f;

        // Foreground pillar darkness (0 = black, 255 = full brightness)
        public const int PillarForegroundTint = 64;
        public static readonly Dictionary<int, (string Key, float Scale)> BrickVariantKeys = new Dictionary<int, (string, float)>()
        {
            { 1,  ("Brick_1",  GameConstants.DefaultSpriteScale) },
            { 2,  ("Brick_2",  GameConstants.DefaultSpriteScale) },
            { 3,  ("Brick_3",  GameConstants.DefaultSpriteScale) },
            { 4,  ("Brick_4",  GameConstants.DefaultSpriteScale) },
            { 5,  ("Brick_5",  GameConstants.DefaultSpriteScale) },
            { 6,  ("Brick_6",  GameConstants.DefaultSpriteScale) },
            { 7,  ("Brick_7",  GameConstants.DefaultSpriteScale) },
            { 8,  ("Brick_8",  GameConstants.DefaultSpriteScale) },
            { 9,  ("Brick_9",  GameConstants.HalfDefaultSpriteScale) },
            { 10, ("Brick_10", GameConstants.HalfDefaultSpriteScale) },
        };

        public static readonly IReadOnlyDictionary<int, (string Key, float Scale)> FlagVariantKeys = new Dictionary<int, (string, float)>()
        {
            { 1, ("Flag_1", 1.0f)  },
            { 2, ("Flag_2", 0.9f)  },
            { 3, ("Flag_3", 0.75f) },
            { 4, ("Flag_4", 1.0f)  },
        };

        public static readonly IReadOnlyDictionary<int, (string Key, float Scale, Color? Tint)> VillageVariantKeys = new Dictionary<int, (string, float, Color?)>()
        {
            { 1, ("Village_1", 1.0f,  null)              },
            { 2, ("Village_2", 1.0f,  null)              },
            { 3, ("Village_3", 1.0f,  null)              },
            { 4, ("Village_4", 1.5f,  Color.White * 0.5f) },
            { 5, ("Village_5", 1.25f, null)              },
        };

        public static readonly IReadOnlyDictionary<int, (string Key, float Scale)> LayerVariantKeys = new Dictionary<int, (string, float)>()
        {
            { 1,  ("Right_Rock_1",  2.0f) },
            { 2,  ("Right_Rock_2",  2.0f) },
            { 3,  ("Right_Rock_3",  2.0f) },
            { 4,  ("Right_Rock_4",  2.0f) },
            { 5,  ("Right_Rock_5",  2.0f) },
            { 6,  ("Right_Rock_6",  2.0f) },
            { 7,  ("Right_Rock_7",  2.0f) },
            { 8,  ("Left_Rock_1",   2.0f) },
            { 9,  ("Left_Rock_2",   2.0f) },
            { 10, ("Left_Rock_3",   2.0f) },
            { 11, ("Left_Rock_4",   2.0f) },
            { 12, ("Left_Rock_5",   2.0f) },
            { 13, ("Left_Rock_6",   2.0f) },
            { 14, ("Left_Rock_7",   2.0f) },
            { 15, ("Left_Rock_8",   2.0f) },
            { 16, ("Left_Rock_9",   2.0f) },
            { 17, ("Left_Rock_10",  2.0f) },
        };

        public static readonly IReadOnlyDictionary<int, string> WallVariantKeys = new Dictionary<int, string>()
        {
            { 0, "Wall_0" },
            { 1, "Wall_1" },
            { 2, "Wall_2" },
            { 3, "Wall_3" },
            { 4, "Wall_4" },
            { 5, "Wall_5" },
        };

        public static readonly IReadOnlyDictionary<int, string> FloorVariantKeys = new Dictionary<int, string>()
        {
            { 1, "Floor_1" },
            { 2, "Floor_2" },
            { 3, "Floor_3" },
        };

        public static readonly IReadOnlyDictionary<int, string> MantisThroneVariantKeys = new Dictionary<int, string>()
        {
            { 1, "MantisThrone_1" },
            { 2, "MantisThrone_2" },
        };

        public static readonly IReadOnlyDictionary<int, string> CageVariantKeys = new Dictionary<int, string>()
        {
            { 1, "Cage_1" },
            { 2, "Cage_2" },
        };

        public static readonly IReadOnlyDictionary<int, string> PoleVariantKeys = new Dictionary<int, string>()
        {
            { 1, "Pole_1" },
            { 2, "Pole_2" },
        };

        public static readonly IReadOnlyDictionary<int, string> PathVariantKeys = new Dictionary<int, string>()
        {
            { 1, "Path_1"       },
            { 2, "Path_2"       },
            { 3, "Path_Stone_3" },
        };
    }
}
