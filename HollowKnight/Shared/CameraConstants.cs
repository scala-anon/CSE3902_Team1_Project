namespace HollowKnight.Shared
{
    public static class CameraConstants
    {   
        public const float yAxisCameraRaise = 300;
        public const float cameraCenterOffset = 950;
        public const float cameraFollowKnightMax = 40;
        public const float cameraFollowKnightMin = -50;
        public const float cameraKnightOffset = 60;

        // Per-frame interpolation weight for Camera.Follow (0 = frozen, 1 = instant).
        // Tuned for 60 FPS fixed timestep; revisit if IsFixedTimeStep or TargetElapsedTime change.
        public const float cameraLerpFactor = 0.12f;

    }
}