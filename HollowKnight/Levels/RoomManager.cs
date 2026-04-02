using HollowKnight.Player;
using Microsoft.Xna.Framework;
using System;

namespace HollowKnight.Levels
{
    public class RoomManager
    {
        private const int PlayerWidth = 48;

        private readonly TheKnight _knight;
        private readonly Camera _camera;
        private readonly int _screenWidth;
        private readonly int _levelWidth;
        private readonly int _roomCount;

        private int _currentRoomIndex;

        public RoomManager(
            TheKnight knight,
            Camera camera,
            int screenWidth,
            int screenHeight,
            int levelWidth,
            int levelHeight)
        {
            _knight = knight;
            _camera = camera;
            _screenWidth = screenWidth;
            _levelWidth = Math.Max(screenWidth, levelWidth);
            _roomCount = Math.Max(1, (int)Math.Ceiling(_levelWidth / (double)_screenWidth));

            _camera.SetBounds(_levelWidth, levelHeight);
            _camera.SnapTo(new Vector2(_screenWidth / 2f, screenHeight / 2f));
        }

        public void Update(GameTime gameTime)
        {
            HandleAutomaticRoomSwitch();
            _camera.Follow(_knight.GetPosition());
        }

        public void SwitchRoomByOffset(int offset)
        {
            int targetRoomIndex = Math.Clamp(_currentRoomIndex + offset, 0, _roomCount - 1);
            if (targetRoomIndex == _currentRoomIndex)
            {
                return;
            }

            _currentRoomIndex = targetRoomIndex;
            float roomLeft = _currentRoomIndex * _screenWidth;
            _knight.SetPosition(new Vector2(roomLeft + 100f, _knight.GetPosition().Y));
            _camera.Follow(_knight.GetPosition());
        }

        public void JumpToRoomIndex(int roomIndex)
        {
            if (roomIndex < 0 || roomIndex >= _roomCount)
            {
                return;
            }

            _currentRoomIndex = roomIndex;
            float roomLeft = _currentRoomIndex * _screenWidth;
            _knight.SetPosition(new Vector2(roomLeft + 100f, _knight.GetPosition().Y));
            _camera.Follow(_knight.GetPosition());
        }

        private void HandleAutomaticRoomSwitch()
        {
            Vector2 position = _knight.GetPosition();
            float roomLeft = _currentRoomIndex * _screenWidth;
            float roomRight = roomLeft + _screenWidth;

            if (position.X <= roomLeft && _currentRoomIndex > 0)
            {
                _currentRoomIndex--;
                return;
            }

            if (position.X + PlayerWidth >= roomRight && _currentRoomIndex < _roomCount - 1)
            {
                _currentRoomIndex++;
                return;
            }

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
