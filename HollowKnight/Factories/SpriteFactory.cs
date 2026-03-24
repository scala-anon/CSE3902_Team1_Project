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
        private Texture2D knightVarietySheet;
        //private Texture2D knightSpriteSheet;
        //private Texture2D knightAbilitiesSheet;

        private Texture2D enemySpriteSheet;
        private Texture2D platformSpriteSheet;
        //ideally we make this into one sprite sheet but for now we have this additional one
        private Texture2D tutorialPlatformSpriteSheet;

        private Texture2D spellsSpriteSheet;


        private SpriteFont defaultFont; //not used yet, will use later on

        private readonly Dictionary<string, Rectangle> knightSingleFrames;
        private readonly Dictionary<string, Rectangle[]> knightAnimations;

        //crawlid only has animations, no single frames
        private readonly Dictionary<string, Rectangle[]> crawlidAnimations;

        //vengefly only has animations, no single frames
        private readonly Dictionary<string, Rectangle[]> vengeflyAnimations;


        private readonly Dictionary<string, Rectangle> spiritSingleFrames;
        private readonly Dictionary<string, Rectangle[]> spiritAnimations;

        private readonly Dictionary<string, Rectangle> platformFrames;


        private static SpriteFactory instance = new SpriteFactory();

        public static SpriteFactory Instance
        {
            get { return instance; }
        }

        private SpriteFactory()
        {
            knightSingleFrames = new Dictionary<string, Rectangle>();
            knightAnimations = new Dictionary<string, Rectangle[]>();

            crawlidAnimations = new Dictionary<string, Rectangle[]>();

            vengeflyAnimations = new Dictionary<string, Rectangle[]>();

            spiritSingleFrames = new Dictionary<string, Rectangle>();
            spiritAnimations = new Dictionary<string, Rectangle[]>();

            platformFrames = new Dictionary<string, Rectangle>();

        }

        public void LoadAllTextures(ContentManager content)
        {
            //Loading Master Knight sheet
            knightVarietySheet = content.Load<Texture2D>("sprites/KnightVarietySpriteSheet");

            // Loading Atlases
            TextureAtlas knightAtlas = TextureAtlas.FromFile(content, "sprites/knight_movement-atlas.xml");
            TextureAtlas knightAttacksAtlas = TextureAtlas.FromFile(content, "sprites/knight_abilities-atlas.xml");

            TextureAtlas SpiritAttacksAtlas = TextureAtlas.FromFile(content, "sprites/spirit-atlas.xml");
            TextureAtlas enemyAtlas = TextureAtlas.FromFile(content, "sprites/enemy-atlas.xml");
            TextureAtlas platformAtlas = TextureAtlas.FromFile(content, "sprites/platform-atlas.xml");
            TextureAtlas tutorialPlatformAtlas = TextureAtlas.FromFile(content,"sprites/tutorial-platform-atlas.xml");

            enemySpriteSheet = enemyAtlas.Texture;
            platformSpriteSheet = platformAtlas.Texture;
            tutorialPlatformSpriteSheet = tutorialPlatformAtlas.Texture;
            spellsSpriteSheet = SpiritAttacksAtlas.Texture;

            //From knight_movement-atlas.xml
            knightSingleFrames.Add("Idle", knightAtlas.GetRegion("Idle").SourceRectangle);
            knightSingleFrames.Add("Damaged", knightAtlas.GetRegion("Damaged").SourceRectangle);

            knightAnimations.Add("Walking", knightAtlas.GetAnimationFrames("Walking"));
            knightAnimations.Add("Jumping", knightAtlas.GetAnimationFrames("Jumping"));

            //From knight_abilities-atlas.xml
            //Consists of attack animation and ability animations
            knightAnimations.Add("UpSword", knightAttacksAtlas.GetAnimationFrames("UpSword"));
            knightAnimations.Add("SideSword", knightAttacksAtlas.GetAnimationFrames("SideSword"));
            knightAnimations.Add("DownSword", knightAttacksAtlas.GetAnimationFrames("DownSword"));
            knightAnimations.Add("HealPrep", knightAttacksAtlas.GetAnimationFrames("HealPrep"));
            knightAnimations.Add("HealPost", knightAttacksAtlas.GetAnimationFrames("HealPost"));
            knightAnimations.Add("SpiritCast", knightAttacksAtlas.GetAnimationFrames("SpiritCast"));

            //Slash effect animations (also from knight_abilities-atlas.xml)
            knightAnimations.Add("SideSlash", knightAttacksAtlas.GetAnimationFrames("SideSlash"));
            knightAnimations.Add("UpSlash", knightAttacksAtlas.GetAnimationFrames("UpSlash"));
            knightAnimations.Add("DownSlash", knightAttacksAtlas.GetAnimationFrames("DownSlash"));

            //From spirit-atlas.xml
            spiritSingleFrames.Add("SpiritInitial", SpiritAttacksAtlas.GetRegion("SpiritInitial").SourceRectangle);
            spiritAnimations.Add("MovingSpirit", SpiritAttacksAtlas.GetAnimationFrames("MovingSpirit"));
            spiritAnimations.Add("Pulse", SpiritAttacksAtlas.GetAnimationFrames("Pulse"));
            spiritAnimations.Add("Collision", SpiritAttacksAtlas.GetAnimationFrames("Collision"));

            // Load platform atlas from XML
            platformFrames.Add("Spike_Floor_1", platformAtlas.GetRegion("Spike_Floor_1").SourceRectangle);
            platformFrames.Add("Spike_Floor_2", platformAtlas.GetRegion("Spike_Floor_2").SourceRectangle);
            platformFrames.Add("Spike_Ceiling", platformAtlas.GetRegion("Spike_Ceiling").SourceRectangle);
            platformFrames.Add("Path_1", platformAtlas.GetRegion("Path_1").SourceRectangle);
            platformFrames.Add("Path_2", platformAtlas.GetRegion("Path_2").SourceRectangle);
            platformFrames.Add("Path_Stone_3", platformAtlas.GetRegion("Path_Stone_3").SourceRectangle);
            platformFrames.Add("Path_ledge", platformAtlas.GetRegion("Path_ledge").SourceRectangle);

            platformFrames.Add("Tutorial_Platform_1", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_1").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_2", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_2").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_3", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_3").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_4", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_4").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_5", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_5").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_6", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_6").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_7", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_7").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_8", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_8").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_9", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_9").SourceRectangle);
            platformFrames.Add("Tutorial_Platform_10", tutorialPlatformAtlas.GetRegion("Tutorial_Platform_10").SourceRectangle);


            // From enemy-atlas.xml
            crawlidAnimations.Add("Crawlid_Idle", enemyAtlas.GetAnimationFrames("Crawlid_Idle"));
            crawlidAnimations.Add("Crawlid_Turn", enemyAtlas.GetAnimationFrames("Crawlid_Turn"));
            crawlidAnimations.Add("Crawlid_Death_Air", enemyAtlas.GetAnimationFrames("Crawlid_Death_Air"));
            crawlidAnimations.Add("Crawlid_Death_Land", enemyAtlas.GetAnimationFrames("Crawlid_Death_Land"));

            vengeflyAnimations.Add("Vengefly_Idle", enemyAtlas.GetAnimationFrames("Vengefly_Idle"));
            vengeflyAnimations.Add("Vengefly_Turning", enemyAtlas.GetAnimationFrames("Vengefly_Turning"));
            vengeflyAnimations.Add("Vengefly_Startle", enemyAtlas.GetAnimationFrames("Vengefly_Startle"));
            vengeflyAnimations.Add("Vengefly_Chase", enemyAtlas.GetAnimationFrames("Vengefly_Chase"));
            vengeflyAnimations.Add("Vengefly_Death", enemyAtlas.GetAnimationFrames("Vengefly_Death"));

            // defaultFont = content.Load<SpriteFont>("fonts/Credits"); //used later on
        }

        //Platform factory methods
        public ISprite CreateTutorial_Platform_1Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_1"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_2Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_2"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_3Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_3"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_4Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_4"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_5Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_5"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_6Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_6"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_7Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_7"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_8Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_8"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_9Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_9"], position, 1.0f);
        }

        public ISprite CreateTutorial_Platform_10Sprite(Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames["Tutorial_Platform_10"], position, 1.0f);
        }


        public ISprite CreatePath_1Sprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_1"], position, 1.0f);
        }

        public ISprite CreatePath_2Sprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_2"], position, 1.0f);
        }

        public ISprite CreatePath_Stone_3Sprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_Stone_3"], position, 1.0f);
        }

        public ISprite CreatePath_LedgeSprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_ledge"], position, 1.0f);
        }
        public ISprite CreateSpikeSprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Spike_Floor_1"], position, 1.0f);
        }
        public ISprite CreateSpikeFloor2Sprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Spike_Floor_2"], position, 1.0f);
        }
        public ISprite CreateSpikeCeilingSprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Spike_Ceiling"], position, 1.0f);
        }


        //Knight factory methods
        public ISprite CreateKnightIdleSprite(Vector2 position)
        {
            return new StaticSprite(knightVarietySheet, knightSingleFrames["Idle"], position, 1.0f);
        }
        public ISprite CreateKnightDamagedSprite(Vector2 position)
        {
            return new StaticSprite(knightVarietySheet, knightSingleFrames["Damaged"], position, 1.0f);
        }
        public ISprite CreateKnightWalkSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["Walking"], position, 0.05, 1.0f); //made walking animation faster
        }

        public ISprite CreateKnightJumpSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["Jumping"], position, 0.1, 1.0f);
        }

        public ISprite CreateKnightUpSwordSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["UpSword"], position, 0.1, 1.0f);
        }

        public ISprite CreateKnightSideSwordSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["SideSword"], position, 0.1, 1.0f);
        }

        public ISprite CreateKnightDownSwordSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["DownSword"], position, 0.1, 1.0f);
        }

        public ISprite CreateSideSlashEffect(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["SideSlash"], position, 0.08, 1.0f);
        }

        public ISprite CreateUpSlashEffect(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["UpSlash"], position, 0.08, 1.0f);
        }

        public ISprite CreateDownSlashEffect(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["DownSlash"], position, 0.08, 1.0f);
        }

        public ISprite CreateKnightHealPrepSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["HealPrep"], position, 0.1, 1.0f);
        }
        public ISprite CreateKnightHealPostSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["HealPost"], position, 0.1, 1.0f);
        }
        public ISprite CreateSpiritCastSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["SpiritCast"], position, 0.1, 1.0f);
        }



        //Vengefly factory methods
        public ISprite CreateVengeflyIdleSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Idle"], position, 0.1, 1.0f);
        }

        public ISprite CreateVengeflyTurningSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Turning"], position, 0.1, 1.0f);
        }

        public ISprite CreateVengeflyStartleSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Startle"], position, 0.1, 1.0f);
        }

        public ISprite CreateVengeflyChaseSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Chase"], position, 0.1, 1.0f);
        }

        public ISprite CreateVengeflyDeathSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Death"], position, 0.1, 1.0f);
        }


        //Crawlid factory methods
        public ISprite CreateCrawlidIdleSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Idle"], position, 0.1, 1.0f);
        }

        public ISprite CreateCrawlidTurnSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Turn"], position, 0.1, 1.0f);
        }

        public ISprite CreateCrawlidDeathAirSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Death_Air"], position, 0.1, 1.0f);
        }

        public ISprite CreateCrawlidDeathLandSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Death_Land"], position, 0.1, 1.0f);
        }



        //Spirit factory methods
        public ISprite CreateSpiritInitialSprite(Vector2 position)
        {
            return new StaticSprite(spellsSpriteSheet, spiritSingleFrames["SpiritInitial"], position, 1.0f);
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
