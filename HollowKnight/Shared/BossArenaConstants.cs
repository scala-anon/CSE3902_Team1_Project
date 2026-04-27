using Microsoft.Xna.Framework;

namespace HollowKnight.Shared
{
    public static class BossArenaConstants
    {
        public const float LeftBarrierX  = 3600f;
        public const float RightBarrierX = 5440f;
        public const float BarrierWidth  = 69f;
        public const float BarrierHeight = 1400f;
        public const float BarrierTopY   = 4200f;

        public const float CameraClampCenterX = 3591f;
        public const float CameraClampCenterY = 4723f;
        public static readonly Vector2 CameraClampCenter = new Vector2(CameraClampCenterX, CameraClampCenterY);
    }
}
