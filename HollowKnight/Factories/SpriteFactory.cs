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

        private Dictionary<string, Rectangle> knightFrames;
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
            knightFrames = new Dictionary<string, Rectangle>();
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
            knightFrames.Clear();

            // IDLE
            knightFrames.Add("Knight_Idle", new Rectangle(23, 3, 35, 70));

            // WALKING
            knightFrames.Add("Walking_1", new Rectangle(101, 3, 42, 70));
            knightFrames.Add("Walking_2", new Rectangle(183, 3, 41, 70));
            knightFrames.Add("Walking_3", new Rectangle(259, 1, 43, 70));
            knightFrames.Add("Walking_4", new Rectangle(342, 2, 43, 71));
            knightFrames.Add("Walking_5", new Rectangle(415, 3, 49, 71));
            knightFrames.Add("Walking_6", new Rectangle(493, 4, 49, 71));
            knightFrames.Add("Walking_7", new Rectangle(578, 3, 41, 70));
            knightFrames.Add("Walking_8", new Rectangle(660, 3, 41, 70));

            // JUMPING
            knightFrames.Add("Jumping_1", new Rectangle(20, 725, 41, 70));
            knightFrames.Add("Jumping_2", new Rectangle(100, 725, 40, 70));
            knightFrames.Add("Jumping_3", new Rectangle(174, 721, 46, 72));
            knightFrames.Add("Jumping_4", new Rectangle(259, 719, 43, 71));
            knightFrames.Add("Jumping_5", new Rectangle(337, 719, 44, 71));
            knightFrames.Add("Jumping_6", new Rectangle(408, 722, 57, 71));
            knightFrames.Add("Jumping_7", new Rectangle(488, 723, 58, 71));
            knightFrames.Add("Jumping_8", new Rectangle(568, 723, 58, 71));
            knightFrames.Add("Jumping_9", new Rectangle(660, 720, 44, 71));
            knightFrames.Add("Jumping_10", new Rectangle(731, 723, 58, 71));
            knightFrames.Add("Jumping_11", new Rectangle(811, 723, 58, 71));
            knightFrames.Add("Jumping_12", new Rectangle(891, 723, 57, 71));
        }

        // =================================================================
        // TODO: Add factory methods for your game sprites below
        // =================================================================

        public ISprite CreateKnightIdleSprite(Vector2 position)
        {
            Rectangle sourceRect = knightFrames["Knight_Idle"];
            return new StaticSprite(knightSpriteSheet, sourceRect, position, 2.0f);
        }

        public ISprite CreateKnightWalkSprite(Vector2 position)
        {
            Rectangle[] walkFrames = new Rectangle[]
            {
                knightFrames["Walking_1"],
                knightFrames["Walking_2"],
                knightFrames["Walking_3"],
                knightFrames["Walking_4"],
                knightFrames["Walking_5"],
                knightFrames["Walking_6"],
                knightFrames["Walking_7"],
                knightFrames["Walking_8"],
            };
            return new AnimatedSprite(knightSpriteSheet, walkFrames, position, 0.1, 2.0f);
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
