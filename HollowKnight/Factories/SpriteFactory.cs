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
        private Texture2D platformSpriteSheet;

        private Texture2D spellsSpriteSheet;

        private Texture2D knightAbilitiesSheet;


        private SpriteFont defaultFont;

        private readonly Dictionary<string, Rectangle> knightSingleFrames;
        private readonly Dictionary<string, Rectangle[]> knightAnimations;

        // private readonly Dictionary<string, Rectangle> crawlidSingleFrames;
        private readonly Dictionary<string, Rectangle[]> crawlidAnimations;

        private readonly Dictionary<string, Rectangle> platformFrames;

        private readonly Dictionary<string, Rectangle[]> vengeflyAnimations;
        // private readonly Dictionary<string, Rectangle> vengeflySingleFrames;


        private readonly Dictionary<string, Rectangle> spiritSingleFrames;
        private readonly Dictionary<string, Rectangle[]> spiritAnimations;

        private static SpriteFactory instance = new SpriteFactory();

        public static SpriteFactory Instance
        {
            get { return instance; }
        }

        private SpriteFactory()
        {
            knightSingleFrames = new Dictionary<string, Rectangle>();
            knightAnimations = new Dictionary<string, Rectangle[]>();

            // crawlidSingleFrames = new Dictionary<string, Rectangle>();
            crawlidAnimations = new Dictionary<string, Rectangle[]>();

            // vengeflySingleFrames = new Dictionary<string, Rectangle>();
            vengeflyAnimations = new Dictionary<string, Rectangle[]>();

            platformFrames = new Dictionary<string, Rectangle>();
            spiritSingleFrames = new Dictionary<string, Rectangle>();
            spiritAnimations = new Dictionary<string, Rectangle[]>();
        }

        public void LoadAllTextures(ContentManager content)
        {
            // Load knight atlas from XML
            TextureAtlas knightAtlas = TextureAtlas.FromFile(content, "sprites/knight_movement-atlas.xml");
            TextureAtlas knightAttacksAtlas = TextureAtlas.FromFile(content, "sprites/knight_abilities-atlas.xml");
            TextureAtlas knightSpiritAttacksAtlas = TextureAtlas.FromFile(content, "sprites/spirit-atlas.xml");
            TextureAtlas enemyAtlas = TextureAtlas.FromFile(content, "sprites/enemy-atlas.xml");
            TextureAtlas platformAtlas = TextureAtlas.FromFile(content, "sprites/platform-atlas.xml");


            knightSpriteSheet = knightAtlas.Texture;
            enemySpriteSheet = enemyAtlas.Texture;
            spellsSpriteSheet = knightSpiritAttacksAtlas.Texture;
            knightAbilitiesSheet = knightAttacksAtlas.Texture;
            platformSpriteSheet = platformAtlas.Texture;


            knightSingleFrames.Add("Idle", knightAtlas.GetRegion("Idle").SourceRectangle);
            knightSingleFrames.Add("Damaged", knightAtlas.GetRegion("Damaged").SourceRectangle);

            knightAnimations.Add("Walking", knightAtlas.GetAnimationFrames("Walking"));
            knightAnimations.Add("Jumping", knightAtlas.GetAnimationFrames("Jumping"));

            knightAnimations.Add("UpSlash", knightAttacksAtlas.GetAnimationFrames("UpSlash"));
            knightAnimations.Add("SideSlash", knightAttacksAtlas.GetAnimationFrames("SideSlash"));
            knightAnimations.Add("DownSlash", knightAttacksAtlas.GetAnimationFrames("DownSlash"));
            knightAnimations.Add("HealPrep", knightAttacksAtlas.GetAnimationFrames("HealPrep"));
            knightAnimations.Add("HealPost", knightAttacksAtlas.GetAnimationFrames("HealPost"));

            spiritSingleFrames.Add("SpellCast", knightSpiritAttacksAtlas.GetRegion("SoulSpirit_Initial").SourceRectangle); //Inital spirit frame

            spiritAnimations.Add("MovingSpirit", knightSpiritAttacksAtlas.GetAnimationFrames("MovingSpirit"));
            spiritAnimations.Add("Pulse", knightSpiritAttacksAtlas.GetAnimationFrames("Pulse"));
            spiritAnimations.Add("Collision", knightSpiritAttacksAtlas.GetAnimationFrames("Collision"));


            // Load enemy atlas from XML
            enemySpriteSheet = enemyAtlas.Texture;

            platformFrames.Add("Spike_Floor_1", platformAtlas.GetRegion("Spike_Floor_1").SourceRectangle);
            platformFrames.Add("Spike_Floor_2", platformAtlas.GetRegion("Spike_Floor_2").SourceRectangle);
            platformFrames.Add("Spike_Ceiling", platformAtlas.GetRegion("Spike_Ceiling").SourceRectangle);

            platformFrames.Add("Path_1", platformAtlas.GetRegion("Path_1").SourceRectangle);
            platformFrames.Add("Path_2", platformAtlas.GetRegion("Path_2").SourceRectangle);
            platformFrames.Add("Path_Stone_3", platformAtlas.GetRegion("Path_Stone_3").SourceRectangle);
            platformFrames.Add("Path_ledge", platformAtlas.GetRegion("Path_ledge").SourceRectangle);


            crawlidAnimations.Add("Crawlid_Idle", enemyAtlas.GetAnimationFrames("Crawlid_Idle"));
            crawlidAnimations.Add("Crawlid_Turning", enemyAtlas.GetAnimationFrames("Crawlid_Turning"));
            crawlidAnimations.Add("Crawlid_Death_Air", enemyAtlas.GetAnimationFrames("Crawlid_Death_Air"));
            crawlidAnimations.Add("Crawlid_Death_Land", enemyAtlas.GetAnimationFrames("Crawlid_Death_Land"));

            vengeflyAnimations.Add("Vengefly_Idle", enemyAtlas.GetAnimationFrames("Vengefly_Idle"));
            vengeflyAnimations.Add("Vengefly_Turning", enemyAtlas.GetAnimationFrames("Vengefly_Turning"));
            vengeflyAnimations.Add("Vengefly_Startle", enemyAtlas.GetAnimationFrames("Vengefly_Startle"));
            vengeflyAnimations.Add("Vengefly_Chase", enemyAtlas.GetAnimationFrames("Vengefly_Chase"));
            vengeflyAnimations.Add("Vengefly_Death", enemyAtlas.GetAnimationFrames("Vengefly_Death"));

            // defaultFont = content.Load<SpriteFont>("fonts/Credits");
        }

        //Knight factory methods
        public ISprite CreateKnightIdleSprite(Vector2 position)
        {
            return new StaticSprite(knightSpriteSheet, knightSingleFrames["Idle"], position, 2.0f);
        }

        public ISprite CreatePath_1Sprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_1"], position, 2.0f);
        }

        public ISprite CreatePath_2Sprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_2"], position, 2.0f);
        }

        public ISprite CreatePath_Stone_3Sprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_Stone_3"], position, 2.0f);
        }

        public ISprite CreatePath_LedgeSprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_ledge"], position, 2.0f);
        }

        public ISprite CreateSpikeSprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Spike_Floor_1"], position, 2.0f);
        }
        public ISprite CreateSpikeFloor2Sprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Spike_Floor_2"], position, 2.0f);
        }
        public ISprite CreateSpikeCeilingSprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Spike_Ceiling"], position, 2.0f);
        }
        public ISprite CreateKnightDamagedSprite(Vector2 position)
        {
            return new StaticSprite(knightAbilitiesSheet, knightSingleFrames["Damaged"], position, 2.0f);
        }

        public ISprite CreateKnightWalkSprite(Vector2 position)
        {
            return new AnimatedSprite(knightSpriteSheet, knightAnimations["Walking"], position, 0.1, 2.0f);
        }

        public ISprite CreateKnightJumpSprite(Vector2 position)
        {
            return new AnimatedSprite(knightSpriteSheet, knightAnimations["Jumping"], position, 0.1, 2.0f);
        }

        public ISprite CreateKnightUpSlashSprite(Vector2 position)
        {
            return new AnimatedSprite(knightAbilitiesSheet, knightAnimations["UpSlash"], position, 0.1, 2.0f);
        }

        public ISprite CreateKnightSideSlashSprite(Vector2 position)
        {
            return new AnimatedSprite(knightAbilitiesSheet, knightAnimations["SideSlash"], position, 0.1, 2.0f);
        }

        public ISprite CreateKnightDownSlashSprite(Vector2 position)
        {
            return new AnimatedSprite(knightAbilitiesSheet, knightAnimations["DownSlash"], position, 0.1, 2.0f);
        }

        public ISprite CreateKnightHealPrepSprite(Vector2 position)
        {
            return new AnimatedSprite(knightAbilitiesSheet, knightAnimations["HealPrep"], position, 0.1, 2.0f);
        }
        public ISprite CreateKnightHealPostSprite(Vector2 position)
        {
            return new AnimatedSprite(knightAbilitiesSheet, knightAnimations["HealPost"], position, 0.1, 2.0f);
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


        //Crawlid factory methods
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


        //Spirit factory methods
        public ISprite CreateSpiritInitialSprite(Vector2 position)
        {
            return new StaticSprite(spellsSpriteSheet, spiritSingleFrames["SpellCast"], position, 2.0f);
        }

        public ISprite CreateSpiritMovingSprite(Vector2 position)
        {
            return new AnimatedSprite(spellsSpriteSheet, spiritAnimations["MovingSpirit"], position, 0.1, 2.0f);
        }

        public ISprite CreateSpiritPulseSprite(Vector2 position)
        {
            return new AnimatedSprite(spellsSpriteSheet, spiritAnimations["Pulse"], position, 0.1, 2.0f);
        }

        public ISprite CreateSpiritCollisionSprite(Vector2 position)
        {
            return new AnimatedSprite(spellsSpriteSheet, spiritAnimations["Collision"], position, 0.1, 2.0f);

        }


        public ISprite CreateTextSprite(string text, Vector2 position, Color color)
        {
            return new TextSprite(defaultFont, text, position, color);
        }

    }
}
