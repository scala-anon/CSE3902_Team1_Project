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

        private readonly Dictionary<string, Rectangle> crawlidSingleFrames;
        private readonly Dictionary<string, Rectangle[]> crawlidAnimations;
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

            crawlidSingleFrames = new Dictionary<string, Rectangle>();
            crawlidAnimations = new Dictionary<string, Rectangle[]>();

            vengeflySingleFrames = new Dictionary<string, Rectangle>();
            vengeflyAnimations = new Dictionary<string, Rectangle[]>();
        }

        public void LoadAllTextures(ContentManager content)
        {
            // Load knight atlas from XML
            TextureAtlas knightAtlas = TextureAtlas.FromFile(content, "sprites/knight-atlas.xml");
            knightSpriteSheet = knightAtlas.Texture;

            knightSingleFrames.Add("Idle", knightAtlas.GetRegion("Idle").SourceRectangle);
            knightAnimations.Add("Walking", knightAtlas.GetAnimationFrames("Walking"));
            knightAnimations.Add("Jumping", knightAtlas.GetAnimationFrames("Jumping"));

            // Load enemy atlas from XML
            TextureAtlas enemyAtlas = TextureAtlas.FromFile(content, "sprites/enemy-atlas.xml");
            enemySpriteSheet = enemyAtlas.Texture;


            //so I'm dumb i think we need to change naming convention or something....
            crawlidAnimations.Add("Idle", enemyAtlas.GetAnimationFrames("Crawlid_Idle"));
            crawlidAnimations.Add("Turning", enemyAtlas.GetAnimationFrames("Crawlid_Turning"));
            crawlidAnimations.Add("DeathAir", enemyAtlas.GetAnimationFrames("Crawlid_Death_Air"));
            crawlidAnimations.Add("DeathLand", enemyAtlas.GetAnimationFrames("Crawlid_Death_Land"));

            vengeflyAnimations.Add("Idle", enemyAtlas.GetAnimationFrames("Vengefly_Idle"));
            vengeflyAnimations.Add("Turning", enemyAtlas.GetAnimationFrames("Vengefly_Turning"));
            vengeflyAnimations.Add("Startle", enemyAtlas.GetAnimationFrames("Vengefly_Startle"));
            vengeflyAnimations.Add("Chase", enemyAtlas.GetAnimationFrames("Vengefly_Chase"));
            vengeflyAnimations.Add("Death", enemyAtlas.GetAnimationFrames("Vengefly_Death"));

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
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Idle"], position, 0.1, 2.0f);
        }

        public ISprite CreateVengeflyTurningSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Turning"], position, 0.1, 2.0f);
        }

        public ISprite CreateVengeflyStartleSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Startle"], position, 0.1, 2.0f);
        }

        public ISprite CreateVengeflyChaseSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Chase"], position, 0.1, 2.0f);
        }

        public ISprite CreateVengeflyDeathSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Death"], position, 0.1, 2.0f);
        }

        public ISprite CreateCrawlidIdleSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Idle"], position, 0.1, 2.0f);
        }

        public ISprite CreateCrawlidTurnSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Turn"], position, 0.1, 2.0f);
        }

        public ISprite CreateCrawlidDeathAirSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Death_Air"], position, 0.1, 2.0f);
        }

        public ISprite CreateCrawlidDeathLandSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Death_Land"], position, 0.1, 2.0f);
        }

        public ISprite CreateTextSprite(string text, Vector2 position, Color color)
        {
            return new TextSprite(defaultFont, text, position, color);
        }
    }
}
