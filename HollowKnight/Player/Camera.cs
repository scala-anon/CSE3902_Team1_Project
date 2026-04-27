using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight
{
    public class Camera
    {
        private Vector2 position;
        public Vector2 Position => position;
        private int _screenWidth;
        private int _screenHeight;
        private int _levelWidth;
        private int _levelHeight;

        private static Camera instance = new Camera();

        public Camera()
        {
            _levelWidth = GameConstants.DefaultLevelWidth;
            _levelHeight = GameConstants.DefaultLevelHeight;
        }

        public void Initialize(int width, int heigth)
        {
            _screenHeight = heigth;
            _screenWidth = width;
        }

        public static Camera Instance
        {
            get {return instance; }
        }

        public Matrix GetTransform()
        {
            // so everything on screen goes to the left when moving right
            return Matrix.CreateTranslation(-position.X, -position.Y,0);
        }

        public Matrix GetParallaxTransform(float factor)
        {
            return Matrix.CreateTranslation(-position.X * factor, -position.Y * factor, 0);
        }

        private Vector2 ComputeClampedTarget(Vector2 target)
        {
            float x = target.X - _screenWidth / 2f;
            float y = target.Y - _screenHeight / 2f - CameraConstants.yAxisCameraRaise;
            x = MathHelper.Clamp(x, 0, _levelWidth - _screenWidth);
            y = MathHelper.Clamp(y, 0, _levelHeight - _screenHeight);
            return new Vector2(x, y);
        }

        public void Follow(Vector2 target)
        {
            Vector2 desired = ComputeClampedTarget(target);
            position = Vector2.Lerp(position, desired, CameraConstants.cameraLerpFactor);
        }

        public void SetBounds(int levelWidth, int levelHeight)
        {
            _levelWidth = levelWidth;
            _levelHeight = levelHeight;
        }

        public void SnapTo(Vector2 target)
        {
            position = ComputeClampedTarget(target);
        }

        public bool Contains(Vector2 worldPos)
        {
            return worldPos.X >= position.X
                && worldPos.X <= position.X + _screenWidth
                && worldPos.Y >= position.Y
                && worldPos.Y <= position.Y + _screenHeight;
        }

        /// <summary>
        /// Make the screen "clamp" temporarily
        /// </summary>s
        public void SetTempBounds(Vector2 _position)
        {
            float desiredY = _position.Y - _screenHeight / 2f - CameraConstants.yAxisCameraRaise;
            desiredY = MathHelper.Clamp(desiredY, 0, _levelHeight - _screenHeight);
            float newY = MathHelper.Lerp(position.Y, desiredY, CameraConstants.cameraLerpFactor);
            position = new Vector2(position.X, newY);
        }
    }
}
