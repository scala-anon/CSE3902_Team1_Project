using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using System;

namespace HollowKnight.Levels
{
    public class RoomManager
    {
        private const int PlayerWidth = 48;

        private readonly TheKnight _knight;
        private readonly Camera _camera;
        private readonly int _levelWidth;

        public RoomManager(
            TheKnight knight,
            int screenWidth,
            int screenHeight,
            int levelWidth,
            int levelHeight)
        {
            _knight = knight;
            _camera = Camera.Instance;
            _levelWidth = Math.Max(screenWidth, levelWidth);

            _camera.SetBounds(_levelWidth, levelHeight);
            _camera.SnapTo(new Vector2(screenWidth / 2f, screenHeight / 2f));
        }

        public void Update(GameTime gameTime)
        {
            ClampKnightToLevelBounds();

            Vector2 knightPosition = _knight.GetPosition();
            Vector2 cameraPosition = Camera.Instance.Position;
            float xDifference = knightPosition.X - (cameraPosition.X + CameraConstants.cameraCenterOffset);

            if (_knight.Facing == Direction.Right && xDifference >= CameraConstants.cameraFollowKnightMax)
            {
                cameraPosition.X = knightPosition.X - CameraConstants.cameraFollowKnightMax;
                cameraPosition.Y = knightPosition.Y;
                _camera.Follow(cameraPosition);
            }
            else if (_knight.Facing == Direction.Left && xDifference <= CameraConstants.cameraFollowKnightMin)
            {
                cameraPosition.X = knightPosition.X + CameraConstants.cameraKnightOffset;
                cameraPosition.Y = knightPosition.Y;
                _camera.Follow(cameraPosition);
            }
            else if (xDifference >= CameraConstants.cameraFollowKnightMin && xDifference <= -CameraConstants.cameraFollowKnightMin)
            {
                // Dead zone: knight near screen center — hold X, only lerp Y.
                _camera.SetTempBounds(knightPosition);
            }
            else
            {
                _camera.Follow(knightPosition);
            }
        }

        private void ClampKnightToLevelBounds()
        {
            Vector2 position = _knight.GetPosition();
            if (position.X < 0)
            {
                _knight.SetPosition(new Vector2(0, position.Y));
            }
            else if (position.X > _levelWidth - PlayerWidth)
            {
                _knight.SetPosition(new Vector2(_levelWidth - PlayerWidth, position.Y));
            }
        }
    }
}
