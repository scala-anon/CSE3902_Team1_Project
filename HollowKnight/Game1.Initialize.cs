using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Controllers;
using HollowKnight.Factories;
using HollowKnight.Levels;
using HollowKnight.Player;
using HollowKnight.Projectiles;
using HollowKnight.Collision;
using HollowKnight.Interfaces;
using HollowKnight.Pathfinding;
using HollowKnight.Shared;
using HollowKnight.Graphics;
using HollowKnight.Abilities;
using HollowKnight.Audio;
using Microsoft.Xna.Framework.Media;

namespace HollowKnight;

public partial class Game1
{
    private void InitializeRendering()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    private void InitializeSharedResources()
    {
        SpriteFactory.Instance.LoadAllTextures(Content);
    }

    private void InitializeAudio()
    {
        AudioLoader.Instance.loadAudio(Content);
        Song music = AudioLoader.Instance.Get_Mantis_Lords_Music();
        AudioManager.Instance.PlaySong(music);
    }

    private void InitializeNavigationGrid()
    {
        _navigationGrid ??= new NavigationGrid(
            GameConstants.DefaultLevelWidth,
            GameConstants.DefaultLevelHeight,
            GraphicsDevice,
            cellSize: GameConstants.NavGridCellSize);
    }

    private void InitializeItems()
    {
        _items.Clear();
        _items.Add(new Spirit(new Vector2(100, 100)));
        _items.Add(new Spirit(new Vector2(-100, -100)));
    }

    private const string CurrentLevel = "Content/levels/levelOne.xml";

    private void InitializeLevel()
    {
        _level = new LevelLoader();
        _level.Load(CurrentLevel);
        LoadObstacles();
    }

    private void InitializeDebug()
    {
        DebugRenderer.Initialize(GraphicsDevice);
        DebugRenderer.LoadFont(Content.Load<SpriteFont>("fonts/Credits"));
        _debugOverlay = new DebugOverlay(GraphicsDevice);
    }

    private void InitializeGameplaySystems()
    {
        _collisionSystem = new CollisionSystem();
        _projectileSpawner = new ProjectileSpawner(_projectileManager);
    }

    private void InitializePlayerAndProjectiles()
    {
        var sprites = KnightSpriteBuilder.BuildKnightSprites(_level.KnightSpawn);
        _knight = new TheKnight(sprites, _level.KnightSpawn);
        _knightProjectile = new KnightProjectile(_knight, _projectileSpawner);
        _knight.Projectiles = _knightProjectile;
    }

    private void InitializeCameraAndRooms()
    {
        int screenWidth = _graphics.PreferredBackBufferWidth;
        int screenHeight = _graphics.PreferredBackBufferHeight;

        _camera = new Camera(
            screenWidth,
            screenHeight,
            GameConstants.DefaultLevelWidth,
            GameConstants.DefaultLevelHeight);

        _roomManager = new RoomManager(
            _knight,
            _camera,
            screenWidth,
            screenHeight,
            GameConstants.DefaultLevelWidth,
            GameConstants.DefaultLevelHeight);
    }

    private void InitializeControllers()
    {
        int screenWidth = _graphics.PreferredBackBufferWidth;

        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this, _roomManager);

        _controllerList.Add(keyboard);
        _controllerList.Add(new MouseController(screenWidth, _roomManager));
    }

    private void LoadObstacles()
    {
        foreach (IObject obj in _level.Platforms)
        {
            if (obj == null)
            {
                continue;
            }

            foreach (Rectangle rect in obj.GetBounds())
            {
                _navigationGrid.AddObstacle(rect);
            }
        }
    }
}