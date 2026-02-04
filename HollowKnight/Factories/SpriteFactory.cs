using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Sprites;
using HollowKnight.Storage;

namespace HollowKnight.Factories
{
    /// <summary>
    /// Singleton factory for creating game sprites.
    /// Use this to create all sprites instead of calling constructors directly.
    /// Add factory methods for each sprite type (player, enemies, items, etc.)
    /// </summary>
    public class SpriteFactory
    {
        // TODO: Add texture references for each sprite sheet
        // private Texture2D playerSpriteSheet;
        // private Texture2D enemySpriteSheet;

        private SpriteFont defaultFont;

        private static SpriteFactory instance = new SpriteFactory();

        public static SpriteFactory Instance
        {
            get
            {
                return instance;
            }
        }

        private SpriteFactory()
        {
        }

        /// <summary>
        /// Initialize factory with textures from storage.
        /// Call this after Texture2DStorage.LoadAllTextures().
        /// </summary>
        public void LoadAllTextures()
        {
            // TODO: Get your sprite sheets from storage
            // playerSpriteSheet = Texture2DStorage.GetPlayerSpriteSheet();
            // enemySpriteSheet = Texture2DStorage.GetEnemySpriteSheet();

            defaultFont = Texture2DStorage.GetDefaultFont();
        }

        // =================================================================
        // TODO: Add factory methods for your game sprites below
        // =================================================================

        // Example: Player sprite factory methods
        // public ISprite CreatePlayerIdleSprite(Vector2 position)
        // {
        //     Rectangle sourceRect = new Rectangle(0, 0, 32, 32);
        //     return new StaticSprite(playerSpriteSheet, sourceRect, position, 2.0f);
        // }

        // public ISprite CreatePlayerWalkSprite(Vector2 position)
        // {
        //     Rectangle[] walkFrames = new Rectangle[]
        //     {
        //         new Rectangle(0, 0, 32, 32),
        //         new Rectangle(32, 0, 32, 32),
        //         new Rectangle(64, 0, 32, 32),
        //     };
        //     return new AnimatedSprite(playerSpriteSheet, walkFrames, position, 0.1, 2.0f);
        // }

        // Example: Enemy sprite factory methods
        // public ISprite CreateEnemySprite(Vector2 position, int screenWidth)
        // {
        //     Rectangle[] frames = new Rectangle[]
        //     {
        //         new Rectangle(0, 0, 24, 24),
        //         new Rectangle(24, 0, 24, 24),
        //     };
        //     return new MovingAnimatedSprite(enemySpriteSheet, frames, position, screenWidth);
        // }

        /// <summary>
        /// Create a text sprite using the default font.
        /// </summary>
        public ISprite CreateTextSprite(string text, Vector2 position, Color color)
        {
            return new TextSprite(defaultFont, text, position, color);
        }
    }
}
