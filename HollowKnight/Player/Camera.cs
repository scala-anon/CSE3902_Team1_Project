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

        public void Follow(Vector2 target)
        {
            // puts target at center of screen
            float x = target.X - _screenWidth /2f;
            float y = target.Y - _screenHeight /2f - CameraConstants.yAxisCameraRaise;

            // makes sure that the range for the camera is valid
            x = MathHelper.Clamp(x,0,_levelWidth - _screenWidth);
            y = MathHelper.Clamp(y,0, _levelHeight - _screenHeight);

            position = new Vector2(x,y);
        }

        public void SetBounds(int levelWidth, int levelHeight)
        {
            _levelWidth = levelWidth;
            _levelHeight = levelHeight;
        }

        public void SnapTo(Vector2 target)
        {
            Follow(target);
        }

        /// <summary>
        /// Make the screen "clamp" temporarily    
        /// </summary>s
        public void SetTempBounds(Vector2 _position)
        {
            float y = _position.Y - _screenHeight /2f - CameraConstants.yAxisCameraRaise;
            float x = position.X;

            y = MathHelper.Clamp(y, 0, _levelHeight - _screenHeight);
            
            position = new Vector2(x,y);
        }

    }
}
