using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Sprites;
using HollowKnight.Storage;
using System.Collections.Generic;

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
        private Texture2D knightSpriteSheet;
        // private Texture2D enemySpriteSheet;

        private SpriteFont defaultFont;

        private readonly Dictionary<string, Rectangle> knightSingleFrames;
        private readonly Dictionary<string, Rectangle[]> knightAnimations;

        private static SpriteFactory instance = new SpriteFactory();

        public static SpriteFactory Instance
        {
            get { return instance; }
        }

        private SpriteFactory()
        {
            knightSingleFrames = new Dictionary<string, Rectangle>();

            knightAnimations = new Dictionary<string, Rectangle[]>();
        }

        /// <summary>
        /// Initialize factory with textures from storage.
        /// Call this after Texture2DStorage.LoadAllTextures().
        /// </summary>
        public void LoadAllTextures()
        {
            // TODO: Get your sprite sheets from storage
            knightSpriteSheet = Texture2DStorage.GetKnightSpriteSheet();
            // enemySpriteSheet = Texture2DStorage.GetEnemySpriteSheet();

            // defaultFont = Texture2DStorage.GetDefaultFont();
        }

        private void PopulateKnightFrames()
        {

            // IDLE
            knightSingleFrames.Add("Idle", new Rectangle(23, 3, 35, 70));

            // WALKING
            knightAnimations.Add("Walking", new Rectangle[]
            {
                new Rectangle(101, 3, 42, 70),
                new Rectangle(183, 3, 41, 70),
                new Rectangle(259, 1, 43, 70),
                new Rectangle(342, 2, 43, 71),
                new Rectangle(415, 3, 49, 71),
                new Rectangle(493, 4, 49, 71),
                new Rectangle(578, 3, 41, 70),
                new Rectangle(660, 3, 41, 70)
            });


            // JUMPING
            knightAnimations.Add("Jumping", new Rectangle[]
            {
                new Rectangle(20, 725, 41, 70),
                new Rectangle(100, 725, 40, 70),
                new Rectangle(174, 721, 46, 72),
                new Rectangle(259, 719, 43, 71),
                new Rectangle(337, 719, 44, 71),
                new Rectangle(408, 722, 57, 71),
                new Rectangle(488, 723, 58, 71),
                new Rectangle(568, 723, 58, 71),
                new Rectangle(660, 720, 44, 71),
                new Rectangle(731, 723, 58, 71),
                new Rectangle(811, 723, 58, 71),
                new Rectangle(891, 723, 57, 71)
            });
        }

        // =================================================================
        // TODO: Add factory methods for your game sprites below
        // =================================================================

        public ISprite CreateKnightIdleSprite(Vector2 position)
        {
            return new StaticSprite(knightSpriteSheet, knightSingleFrames["Idle"], position, 2.0f);
        }

        public ISprite CreateKnightWalkSprite(Vector2 position)
        {
            return new AnimatedSprite(knightSpriteSheet, knightAnimations["Walking"], position, 0.1, 2.0f);
        }

        public ISprite CreateKnightJumpSprite(Vector2 position)
        {
            return new AnimatedSprite(knightSpriteSheet, knightAnimations["Jumping"], position, 0.1, 2.0f);
        }
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
