using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Sprites;
using HollowKnight.Graphics;
using System.Collections.Generic;

namespace HollowKnight.Factories
{
    public class SpriteFactory
    {
        private Texture2D knightSpriteSheet;
        private Texture2D enemySpriteSheet;

        private SpriteFont defaultFont;

        private readonly Dictionary<string, Rectangle> knightSingleFrames;
        private readonly Dictionary<string, Rectangle[]> knightAnimations;

        private readonly Dictionary<string, Rectangle> vengeflySingleFrames;
        private readonly Dictionary<string, Rectangle[]> vengeflyAnimations;

        private static SpriteFactory instance = new SpriteFactory();

        public static SpriteFactory Instance
        {
            get { return instance; }
        }

        private SpriteFactory()
        {
            knightSingleFrames = new Dictionary<string, Rectangle>();
            knightAnimations = new Dictionary<string, Rectangle[]>();

            vengeflySingleFrames = new Dictionary<string, Rectangle>();
            vengeflyAnimations = new Dictionary<string, Rectangle[]>();
        }

        public void LoadAllTextures(ContentManager content)
        {
            // Load knight atlas from XML
            TextureAtlas knightAtlas = TextureAtlas.FromFile(content, "sprites/knight_movement-atlas.xml");
            knightSpriteSheet = knightAtlas.Texture;

            knightSingleFrames.Add("Idle", knightAtlas.GetRegion("Idle").SourceRectangle);
            knightAnimations.Add("Walking", knightAtlas.GetAnimationFrames("Walking"));
            knightAnimations.Add("Jumping", knightAtlas.GetAnimationFrames("Jumping"));

            // Load enemy atlas from XML
            TextureAtlas enemyAtlas = TextureAtlas.FromFile(content, "sprites/enemy-atlas.xml");
            enemySpriteSheet = enemyAtlas.Texture;

            vengeflyAnimations.Add("Idle", enemyAtlas.GetAnimationFrames("Idle"));
            vengeflyAnimations.Add("Turning", enemyAtlas.GetAnimationFrames("Turning"));
            vengeflyAnimations.Add("Startle", enemyAtlas.GetAnimationFrames("Startle"));
            vengeflyAnimations.Add("Chase", enemyAtlas.GetAnimationFrames("Chase"));
            vengeflyAnimations.Add("Death", enemyAtlas.GetAnimationFrames("Death"));

            // defaultFont = content.Load<SpriteFont>("fonts/Credits");
        }

        //Knight factory methods
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

        //Vengefly factory methods
        public ISprite CreateVengeflyIdleSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Idle"], position, 0.1, 2.0f);
        }

        public ISprite CreateVengeflyTurningSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Turning"], position, 0.1, 2.0f);
        }

        public ISprite CreateVengeflyStartleSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Startle"], position, 0.1, 2.0f);
        }

        public ISprite CreateVengeflyChaseSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Chase"], position, 0.1, 2.0f);
        }

        public ISprite CreateVengeflyDeathSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Death"], position, 0.1, 2.0f);
        }

        public ISprite CreateTextSprite(string text, Vector2 position, Color color)
        {
            return new TextSprite(defaultFont, text, position, color);
        }
    }
}
