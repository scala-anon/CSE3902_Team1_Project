using Microsoft.Xna.Framework;

namespace HollowKnight
{
    public class Camera
    {
        private Vector2 position;
        public Vector2 Position => position;
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private int _levelWidth;
        private int _levelHeight;

        //private static Camera instance = new Camera();

        public Camera(int screenWidth, int screenHeight, int levelWidth, int levelHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _levelWidth = levelWidth;
            _levelHeight = levelHeight;
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
            float y = target.Y - _screenHeight /2f;

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

    }
}
