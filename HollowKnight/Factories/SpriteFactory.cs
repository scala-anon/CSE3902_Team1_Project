using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Sprites;
using HollowKnight.Graphics;
using HollowKnight.Environment;
using System.Collections.Generic;
using HollowKnight.Shared;

namespace HollowKnight.Factories
{
    public class SpriteFactory
    {
        private Texture2D knightVarietySheet;
        private Texture2D enemySpriteSheet;
        private Texture2D platformSpriteSheet;
        private Texture2D tutorialPlatformSpriteSheet;
        private Texture2D spellsSpriteSheet;
        private Texture2D backgroundSpriteSheet;
        private Texture2D mantisLordSpriteSheet;
        private Texture2D mantisVillageSpriteSheet;
        private Texture2D layersSpriteSheet;

        private SpriteFont defaultFont;

        private readonly Dictionary<string, Rectangle> knightSingleFrames;
        private readonly Dictionary<string, Rectangle[]> knightAnimations;
        private readonly Dictionary<string, Rectangle[]> crawlidAnimations;
        private readonly Dictionary<string, Rectangle[]> vengeflyAnimations;
        private readonly Dictionary<string, Rectangle> spiritSingleFrames;
        private readonly Dictionary<string, Rectangle[]> spiritAnimations;
        private readonly Dictionary<string, Rectangle> platformFrames;

        private readonly Dictionary<string, Rectangle> plantSingleFrames;
        private readonly Dictionary<string, Rectangle[]> plantAnimations;

        private readonly Dictionary<string, Rectangle> backgroundFrames;

        private readonly Dictionary<string, Rectangle> layerFrames;

        private readonly Dictionary<string, Rectangle> mantisLordFrames;
        private readonly Dictionary<string, Rectangle[]> mantisLordAnimations;
        private readonly Dictionary<string,Rectangle[]> bossSpikeAnimations;
        private readonly Dictionary<string, Rectangle> mantisVillageFrames;

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
            plantSingleFrames = new Dictionary<string, Rectangle>();
            plantAnimations = new Dictionary<string, Rectangle[]>();
            backgroundFrames = new Dictionary<string, Rectangle>();
            mantisLordFrames = new Dictionary<string, Rectangle>();
            mantisLordAnimations = new Dictionary<string, Rectangle[]>();
            mantisVillageFrames = new Dictionary<string, Rectangle>();
            layerFrames = new Dictionary<string, Rectangle>();
            bossSpikeAnimations = new Dictionary<string, Rectangle[]>();
        }

        public void LoadAllTextures(ContentManager content)
        {
            knightVarietySheet = content.Load<Texture2D>("sprites/KnightVarietySpriteSheet");

            TextureAtlas knightAtlas = TextureAtlas.FromFile(content, "sprites/knight_movement-atlas.xml");
            TextureAtlas knightAttacksAtlas = TextureAtlas.FromFile(content, "sprites/knight_abilities-atlas.xml");
            TextureAtlas spiritAttacksAtlas = TextureAtlas.FromFile(content, "sprites/spirit-atlas.xml");
            TextureAtlas enemyAtlas = TextureAtlas.FromFile(content, "sprites/enemy-atlas.xml");
            TextureAtlas platformAtlas = TextureAtlas.FromFile(content, "sprites/platform-atlas.xml");
            TextureAtlas tutorialPlatformAtlas = TextureAtlas.FromFile(content, "sprites/tutorial-platform-atlas.xml");
            TextureAtlas backgroundAtlas = TextureAtlas.FromFile(content, "sprites/background-atlas.xml");
            TextureAtlas mantisLordAtlas = TextureAtlas.FromFile(content, "sprites/mantisLords-atlas.xml");
            TextureAtlas mantisVillageAtlas = TextureAtlas.FromFile(content,"sprites/village-atlas.xml");
            TextureAtlas layersAtlas = TextureAtlas.FromFile(content, "sprites/layers-atlas.xml");
            enemySpriteSheet = enemyAtlas.Texture;
            platformSpriteSheet = platformAtlas.Texture;
            tutorialPlatformSpriteSheet = tutorialPlatformAtlas.Texture;
            spellsSpriteSheet = spiritAttacksAtlas.Texture;
            backgroundSpriteSheet = backgroundAtlas.Texture;
            mantisLordSpriteSheet = mantisLordAtlas.Texture;
            mantisVillageSpriteSheet = mantisVillageAtlas.Texture;
            layersSpriteSheet = layersAtlas.Texture;
            // Knight movement frames
            knightSingleFrames.Add("Damaged", knightAtlas.GetRegion("Damaged").SourceRectangle);
            
            knightAnimations.Add("Idle", knightAtlas.GetAnimationFrames("Idle"));
            knightAnimations.Add("Walking", knightAtlas.GetAnimationFrames("Walking"));
            knightAnimations.Add("Jumping", knightAtlas.GetAnimationFrames("Jumping"));

            // Knight abilities frames
            knightAnimations.Add("UpSword", knightAttacksAtlas.GetAnimationFrames("UpSword"));
            knightAnimations.Add("SideSword", knightAttacksAtlas.GetAnimationFrames("SideSword"));
            knightAnimations.Add("DownSword", knightAttacksAtlas.GetAnimationFrames("DownSword"));
            knightAnimations.Add("HealPrep", knightAttacksAtlas.GetAnimationFrames("HealPrep"));
            knightAnimations.Add("HealPost", knightAttacksAtlas.GetAnimationFrames("HealPost"));
            knightAnimations.Add("SpiritCast", knightAttacksAtlas.GetAnimationFrames("SpiritCast"));
            knightAnimations.Add("SideSlash", knightAttacksAtlas.GetAnimationFrames("SideSlash"));
            knightAnimations.Add("UpSlash", knightAttacksAtlas.GetAnimationFrames("UpSlash"));
            knightAnimations.Add("DownSlash", knightAttacksAtlas.GetAnimationFrames("DownSlash"));

            // Spirit frames
            spiritSingleFrames.Add("SpiritInitial", spiritAttacksAtlas.GetRegion("SpiritInitial").SourceRectangle);
            spiritAnimations.Add("MovingSpirit", spiritAttacksAtlas.GetAnimationFrames("MovingSpirit"));
            spiritAnimations.Add("Pulse", spiritAttacksAtlas.GetAnimationFrames("Pulse"));
            spiritAnimations.Add("Collision", spiritAttacksAtlas.GetAnimationFrames("Collision"));

            // Platform frames
            platformFrames.Add("Spike_Floor_1", platformAtlas.GetRegion("Spike_Floor_1").SourceRectangle);
            platformFrames.Add("Spike_Floor_2", platformAtlas.GetRegion("Spike_Floor_2").SourceRectangle);
            platformFrames.Add("Spike_Ceiling", platformAtlas.GetRegion("Spike_Ceiling").SourceRectangle);
            platformFrames.Add("Path_1", platformAtlas.GetRegion("Path_1").SourceRectangle);
            platformFrames.Add("Path_2", platformAtlas.GetRegion("Path_2").SourceRectangle);
            platformFrames.Add("Path_Stone_3", platformAtlas.GetRegion("Path_Stone_3").SourceRectangle);
            platformFrames.Add("Path_ledge", platformAtlas.GetRegion("Path_ledge").SourceRectangle);
            platformFrames.Add("Bench", backgroundAtlas.GetRegion("Bench").SourceRectangle);
            backgroundFrames.Add("Background_1", backgroundAtlas.GetRegion("Background_1").SourceRectangle);
            backgroundFrames.Add("Background_2", backgroundAtlas.GetRegion("Background_2").SourceRectangle);
            backgroundFrames.Add("Wall_0", backgroundAtlas.GetRegion("Wall_0").SourceRectangle);
            backgroundFrames.Add("Wall_1", backgroundAtlas.GetRegion("Wall_1").SourceRectangle);
            backgroundFrames.Add("Wall_2", backgroundAtlas.GetRegion("Wall_2").SourceRectangle);
            backgroundFrames.Add("Wall_3",backgroundAtlas.GetRegion("Wall_3").SourceRectangle);
            backgroundFrames.Add("Wall_4",backgroundAtlas.GetRegion("Wall_4").SourceRectangle);
            backgroundFrames.Add("Wall_5",backgroundAtlas.GetRegion("Wall_5").SourceRectangle);
            backgroundFrames.Add("Door_0", backgroundAtlas.GetRegion("Door_0").SourceRectangle);
            backgroundFrames.Add("Door_1", backgroundAtlas.GetRegion("Door_1").SourceRectangle);
            for (int i = 1; i <= 10; i++)
            {
                string key = $"Tutorial_Platform_{i}";
                platformFrames.Add(key, tutorialPlatformAtlas.GetRegion(key).SourceRectangle);
            }

            // Enemy frames
            crawlidAnimations.Add("Crawlid_Idle", enemyAtlas.GetAnimationFrames("Crawlid_Idle"));
            crawlidAnimations.Add("Crawlid_Turn", enemyAtlas.GetAnimationFrames("Crawlid_Turn"));
            crawlidAnimations.Add("Crawlid_Death_Air", enemyAtlas.GetAnimationFrames("Crawlid_Death_Air"));
            crawlidAnimations.Add("Crawlid_Death_Land", enemyAtlas.GetAnimationFrames("Crawlid_Death_Land"));

            vengeflyAnimations.Add("Vengefly_Idle", enemyAtlas.GetAnimationFrames("Vengefly_Idle"));
            vengeflyAnimations.Add("Vengefly_Turning", enemyAtlas.GetAnimationFrames("Vengefly_Turning"));
            vengeflyAnimations.Add("Vengefly_Startle", enemyAtlas.GetAnimationFrames("Vengefly_Startle"));
            vengeflyAnimations.Add("Vengefly_Chase", enemyAtlas.GetAnimationFrames("Vengefly_Chase"));
            vengeflyAnimations.Add("Vengefly_Death", enemyAtlas.GetAnimationFrames("Vengefly_Death"));

            // plant frames
            // plant frames
            plantAnimations.Add("Plant1_Idle", backgroundAtlas.GetAnimationFrames("Plant1_Idle"));
            plantAnimations.Add("Plant2_Idle", backgroundAtlas.GetAnimationFrames("Plant2_Idle"));
            plantSingleFrames.Add("Plant1_Frame0", backgroundAtlas.GetRegion("Plant1_Frame0").SourceRectangle);
            plantSingleFrames.Add("Plant2_Frame0", backgroundAtlas.GetRegion("Plant2_Frame0").SourceRectangle);

            bossSpikeAnimations.Add("Floor_Spike", mantisVillageAtlas.GetAnimationFrames("Floor_Spike"));

            // Mantis Lord frames
            mantisLordFrames.Add("Throne_Idle", mantisLordAtlas.GetRegion("Throne_Idle").SourceRectangle);
            mantisLordAnimations.Add("Throne_Gesture", mantisLordAtlas.GetAnimationFrames("Throne_Gesture"));
            mantisLordAnimations.Add("Throne_Stand", mantisLordAtlas.GetAnimationFrames("Throne_Stand"));
            mantisLordAnimations.Add("Look", mantisLordAtlas.GetAnimationFrames("Look"));
            mantisLordAnimations.Add("Throne_Leave", mantisLordAtlas.GetAnimationFrames("Throne_Leave"));
            mantisLordAnimations.Add("Throne_Wounded", mantisLordAtlas.GetAnimationFrames("Throne_Wounded"));
            mantisLordAnimations.Add("Throne_Bow", mantisLordAtlas.GetAnimationFrames("Throne_Bow"));
            mantisLordAnimations.Add("Wall_Arrive", mantisLordAtlas.GetAnimationFrames("Wall_Arrive"));
            mantisLordAnimations.Add("Wall_Ready", mantisLordAtlas.GetAnimationFrames("Wall_Ready"));
            mantisLordAnimations.Add("Throw", mantisLordAtlas.GetAnimationFrames("Throw"));
            mantisLordAnimations.Add("Wall_Leave", mantisLordAtlas.GetAnimationFrames("Wall_Leave"));
            mantisLordAnimations.Add("Dash_Arrive", mantisLordAtlas.GetAnimationFrames("Dash_Arrive"));
            mantisLordAnimations.Add("Dash_Anticipate", mantisLordAtlas.GetAnimationFrames("Dash_Anticipate"));
            mantisLordAnimations.Add("Dash", mantisLordAtlas.GetAnimationFrames("Dash"));
            mantisLordAnimations.Add("Dash_Recover", mantisLordAtlas.GetAnimationFrames("Dash_Recover"));
            mantisLordAnimations.Add("Dash_Leave", mantisLordAtlas.GetAnimationFrames("Dash_Leave"));
            mantisLordAnimations.Add("DStab_Arrive", mantisLordAtlas.GetAnimationFrames("DStab_Arrive"));
            mantisLordAnimations.Add("DStab", mantisLordAtlas.GetAnimationFrames("DStab"));
            mantisLordAnimations.Add("DStab_Land", mantisLordAtlas.GetAnimationFrames("DStab_Land"));
            mantisLordAnimations.Add("DStab_Leave", mantisLordAtlas.GetAnimationFrames("DStab_Leave"));
            mantisLordAnimations.Add("Death", mantisLordAtlas.GetAnimationFrames("Death"));
            mantisLordAnimations.Add("Death_Leave_One", mantisLordAtlas.GetAnimationFrames("Death_Leave_One"));
            mantisLordAnimations.Add("Death_Leave_Two", mantisLordAtlas.GetAnimationFrames("Death_Leave_Two"));

            for (int i = 1; i <= 8; i++)
            {
                string key = $"Brick_{i}";
                mantisVillageFrames.Add(key, mantisVillageAtlas.GetRegion(key).SourceRectangle);
            }
            for (int i = 1; i <= 2; i++)
            {
                string key = $"MantisThrone_{i}";
                mantisVillageFrames.Add(key, mantisVillageAtlas.GetRegion(key).SourceRectangle);
            }
            for (int i = 1; i <= 3; i++)
            {
                string key = $"Floor_{i}";
                mantisVillageFrames.Add(key, mantisVillageAtlas.GetRegion(key).SourceRectangle);
            }
            for (int i = 1; i <= 4; i++)
            {
                string key = $"Flag_{i}";
                mantisVillageFrames.Add(key, mantisVillageAtlas.GetRegion(key).SourceRectangle);
            }
            for (int i = 1; i <= 3; i++)
            {
                string key = $"Village_{i}";
                mantisVillageFrames.Add(key, mantisVillageAtlas.GetRegion(key).SourceRectangle);
            }

            for (int i = 1; i <= 7; i++)
            {
                string key = $"Right_Rock_{i}";
                layerFrames.Add(key, layersAtlas.GetRegion(key).SourceRectangle);
            }

            for (int i = 1; i <= 10; i++)
            {
                string key = $"Left_Rock_{i}";
                layerFrames.Add(key, layersAtlas.GetRegion(key).SourceRectangle);
            }

            for (int i = 1; i <= 2; i++)
            {
                string key = $"Cage_{i}";
                mantisVillageFrames.Add(key, mantisVillageAtlas.GetRegion(key).SourceRectangle);
            }

            for (int i = 1; i <= 2; i++)
            {
                string key = $"Pole_{i}";
                mantisVillageFrames.Add(key, mantisVillageAtlas.GetRegion(key).SourceRectangle);
            }
        }

        // Consolidated platform factory methods
        public ISprite CreateTutorialPlatformSprite(int id, Vector2 position)
        {
            return new StaticSprite(tutorialPlatformSpriteSheet, platformFrames[$"Tutorial_Platform_{id}"], position, 1.0f);
        }

        public ISprite CreatePathSprite(int variant, Vector2 position)
        {
            string key = variant switch
            {
                1 => "Path_1",
                2 => "Path_2",
                3 => "Path_Stone_3",
                _ => "Path_1"
            };
            return new StaticSprite(platformSpriteSheet, platformFrames[key], position, 1.0f);
        }

        public ISprite CreatePathLedgeSprite(Vector2 position)
        {
            return new StaticSprite(platformSpriteSheet, platformFrames["Path_ledge"], position, 1.0f);
        }

        public ISprite CreateSpikeSprite(SpikeVariant variant, Vector2 position)
        {
            string key = variant switch
            {
                SpikeVariant.Floor1 => "Spike_Floor_1",
                SpikeVariant.Floor2 => "Spike_Floor_2",
                SpikeVariant.Ceiling => "Spike_Ceiling",
                _ => "Spike_Floor_1"
            };
            return new StaticSprite(platformSpriteSheet, platformFrames[key], position, 1.0f);
        }

        // Knight factory methods
        public ISprite CreateKnightIdleSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["Idle"], position, 0.1, 1.0f);
        }
        public ISprite CreateKnightDamagedSprite(Vector2 position)
        {
            return new StaticSprite(knightVarietySheet, knightSingleFrames["Damaged"], position, 1.0f);
        }
        public ISprite CreateKnightWalkSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["Walking"], position, 0.05, 1.0f);
        }
        public ISprite CreateKnightJumpSprite(Vector2 position)
        {
            return new AnimatedSprite(knightVarietySheet, knightAnimations["Jumping"], position, 0.1, 1.0f);
        }
        public ISprite CreateKnightDashSprite(Vector2 position)
        {
            //TODO: Updated to dash animation once we have them
            return new AnimatedSprite(knightVarietySheet, knightAnimations["Walking"], position, 0.05, 1.0f);
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

        // Vengefly factory methods
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
        public ISprite CreateVengeflyDeathAirSprite(Vector2 position)
        {
            return new AnimatedSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Death"], position, 0.1, 1.0f);
        }
        public ISprite CreateVengeflyDeathLandSprite(Vector2 position)
        {
            return new StaticSprite(enemySpriteSheet, vengeflyAnimations["Vengefly_Death"][0], position, 1.0f);
        }

        // Crawlid factory methods
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
            return new StaticSprite(enemySpriteSheet, crawlidAnimations["Crawlid_Death_Land"][0], position, 1.0f);
        }

        // Spirit factory methods
        public ISprite CreateSpiritInitialSprite(Vector2 position)
        {
            return new StaticSprite(spellsSpriteSheet, spiritSingleFrames["SpiritInitial"], position, 1.0f);
        }
        public ISprite CreateSpiritMovingSprite(Vector2 position)
        {
            return new AnimatedSprite(spellsSpriteSheet, spiritAnimations["MovingSpirit"], position, 0.1, 1.0f);
        }
        public ISprite CreateSpiritPulseSprite(Vector2 position)
        {
            return new AnimatedSprite(spellsSpriteSheet, spiritAnimations["Pulse"], position, 0.1, 1.0f);
        }
        public ISprite CreateSpiritCollisionSprite(Vector2 position)
        {
            return new AnimatedSprite(spellsSpriteSheet, spiritAnimations["Collision"], position, 0.1, 1.0f);
        }

        public ISprite CreateTextSprite(string text, Vector2 position, Color color)
        {
            return new TextSprite(defaultFont, text, position, color);
        }

        public ISprite CreatePlant1IdleSprite(Vector2 position)
        {
            return new AnimatedSprite(backgroundSpriteSheet, plantAnimations["Plant1_Idle"], position, 0.25, 1.0f);
        }
        

        public ISprite CreatePlant1ChoppedSprite(Vector2 position)
        {
            return new StaticSprite(backgroundSpriteSheet, plantSingleFrames["Plant1_Frame0"], position, 1.0f);
        }


        public ISprite CreatePlant2IdleSprite(Vector2 position)
        {
            return new AnimatedSprite(backgroundSpriteSheet, plantAnimations["Plant2_Idle"], position, 0.25, 1.0f);  
        }
    

        public ISprite CreatePlant2ChoppedSprite(Vector2 position)
        {
            return new StaticSprite(backgroundSpriteSheet, plantSingleFrames["Plant2_Frame0"], position, 1.0f);   
        }

        public ISprite CreateBenchSprite(Vector2 position)
        {
            return new StaticSprite(backgroundSpriteSheet, platformFrames["Bench"], position, 2.0f);
        }
        public ISprite CreateBackgroundSprite(int variant, Vector2 position)
        {
            // TODO: Fix constants
            string key = variant switch
            {
                1 => "Background_1",
                2 => "Background_2",
                _ => "Background_0"
            };
            return new StaticSprite(backgroundSpriteSheet, backgroundFrames[key], position, 1.0f);
        }

        public ISprite CreateWallSprite(int variant, Vector2 position)
        {
            float scale = 1.25f;
            if(variant==4) scale = .8f;
            string key = variant switch
            {
                0 => "Wall_0",
                1 => "Wall_1",
                2 => "Wall_2",
                3 => "Wall_3",
                4 => "Wall_4",
                5 => "Wall_5",
                _ => "Wall_0"
            };
            return new StaticSprite(backgroundSpriteSheet, backgroundFrames[key], position, scale);
        }

        public ISprite CreateDoorSprite(Vector2 position)
        {
            return new StaticSprite(backgroundSpriteSheet, backgroundFrames["Door_0"], position, 1.25f);
        }

        public ISprite CreateDoorHitSprite(Vector2 position)
        {
            return new StaticSprite(backgroundSpriteSheet, backgroundFrames["Door_1"], position, 1.25f);
        }

        // Mantis Lord factory methods
        public ISprite CreateMantisThroneIdle(Vector2 position)
        {
            return new StaticSprite(mantisLordSpriteSheet, mantisLordFrames["Throne_Idle"], position, 1.00f);
        }
        public ISprite CreateThroneGesture(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Throne_Gesture"], position, 0.1, 1.0f);
        }
        public ISprite CreateThroneArrive(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Throne_Gesture"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateThroneStand(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Throne_Stand"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisLook(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Look"], position, 0.1, 1.0f);
        }
        public ISprite CreateThroneLeave(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Throne_Leave"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateThroneWounded(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Throne_Wounded"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateThroneBow(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Throne_Bow"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateWallArrive(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Wall_Arrive"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateWallReady(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Wall_Ready"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisThrow(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Throw"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateWallLeave(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Wall_Leave"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDashArrive(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Dash_Arrive"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDashAnticipate(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Dash_Anticipate"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDash(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Dash"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDashRecover(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Dash_Recover"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDashLeave(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Dash_Leave"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDStabArrive(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["DStab_Arrive"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDStab(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["DStab"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDStabLand(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["DStab_Land"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDStabLeave(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["DStab_Leave"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDeath(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Death"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDeathLeaveOne(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Death_Leave_One"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateMantisDeathLeaveTwo(Vector2 position)
        {
            return new AnimatedSprite(mantisLordSpriteSheet, mantisLordAnimations["Death_Leave_Two"], position, 0.1, 1.0f, loop: false);
        }
        public ISprite CreateBrickSprite(int variant, Vector2 position)
        {
            string key = variant switch
            {
                1 => "Brick_1",
                2 => "Brick_2",
                3 => "Brick_3",
                4 => "Brick_4",
                5 => "Brick_5",
                6 => "Brick_6",
                7 => "Brick_7",
                8 => "Brick_8",
                _ => "Brick_1"
            };
            return new StaticSprite(mantisVillageSpriteSheet, mantisVillageFrames[key], position, 1.0f);
        }
        public ISprite CreateMantisThroneSprite(int variant, Vector2 position)
        {
            string key = variant switch
            {
                1 => "MantisThrone_1",
                2 => "MantisThrone_2",
                _ => "MantisThrone_1"
            };
            return new StaticSprite(mantisVillageSpriteSheet, mantisVillageFrames[key], position, 1.0f);
        }
        public ISprite CreateFloorSprite(int variant, Vector2 position)
        {
            string key = variant switch
            {
                1 => "Floor_1",
                2 => "Floor_2",
                3 => "Floor_3",
                _ => "Floor_1"
            };
            return new StaticSprite(mantisVillageSpriteSheet, mantisVillageFrames[key], position, 1.0f);
        }
        public ISprite CreateFlagSprite(int variant, Vector2 position)
        {
            string key = variant switch
            {
                1 => "Flag_1",
                2 => "Flag_2",
                3 => "Flag_3",
                4 => "Flag_4",
                _ => "Flag_1"
            };
            return new StaticSprite(mantisVillageSpriteSheet, mantisVillageFrames[key], position, 1.0f);
        }
        public ISprite CreateVillageSprite(int variant, Vector2 position)
        {
            string key = variant switch
            {
                1 => "Village_1",
                2 => "Village_2",
                3 => "Village_3",
                _ => "Village_1"
            };
            return new StaticSprite(mantisVillageSpriteSheet, mantisVillageFrames[key], position, 1.0f);
        }

        public ISprite CreateLayerSprite(int variant, Vector2 position)
        {
            // TODO: Fix constants
            string key = variant switch
            {
                1 => "Right_Rock_1",
                2 => "Right_Rock_2",
                3 => "Right_Rock_3",
                4 => "Right_Rock_4",
                5 => "Right_Rock_5",
                6 => "Right_Rock_6",
                7 => "Right_Rock_7",
                8 => "Left_Rock_1",
                9 => "Left_Rock_2",
                10 => "Left_Rock_3",
                11 => "Left_Rock_4",
                12 => "Left_Rock_5",
                13 => "Left_Rock_6",
                14 => "Left_Rock_7",
                15 => "Left_Rock_8",
                16 => "Left_Rock_9",
                17 => "Left_Rock_10",
                _ => "Right_Rock_1" 

            };
            return new StaticSprite(layersSpriteSheet, layerFrames[key], position, 2.0f, .75f,Color.White);
        }

        public ISprite CreateBossMiddlegroundSprite(int variant, Vector2 position)
        {
            string key = variant switch
            {
                1 => "Cage_1",
                2 => "Cage_2",
                _ => "Cage_1"
            };
            return new StaticSprite(mantisVillageSpriteSheet, mantisVillageFrames[key], position, 1.0f);
        }
        public ISprite CreatePoleSprite(int variant, Vector2 position)
        {
            string key = variant switch
            {
                1 => "Pole_1",
                2 => "Pole_2",
                _ => "Pole_1",
            };
            return new StaticSprite(mantisVillageSpriteSheet,mantisVillageFrames[key],position,1.0f);
        }
        public ISprite CreateBossSpikeIdle(Vector2 position)
        {
            return new AnimatedSprite(mantisVillageSpriteSheet, bossSpikeAnimations["Floor_Spike"], position, 0.5, 1.5f);
        }
    }
}
