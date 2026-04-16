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
    private Camera camera;
    private void InitializeRendering()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _overlayPixel = new Texture2D(GraphicsDevice, 1, 1);
        _overlayPixel.SetData(new[] { Color.White });
    }

    private void InitializeSharedResources()
    {
        SpriteFactory.Instance.LoadAllTextures(Content);
    }

    private void InitializeAudio()
    {
        AudioLoader.Instance.loadAudio(Content);
        Song music = AudioLoader.Instance.Get_Enter_Hollownest();
        AudioManager.Instance.PlaySong(music);
    }

    private void InitializeNavigationGrid()
    {
        _navigationGrid = new NavigationGrid(
            GameConstants.DefaultLevelWidth,
            GameConstants.DefaultLevelHeight,
            GraphicsDevice,
            cellSize: GameConstants.NavGridCellSize);
    }

    private void InitializeItems()
    {
        _items.Clear();
        _items.Add(new Spirit(new Vector2(100, 100)));
        DebugLogger.LogObject($"Spirit spawned at (100,100)");
        _items.Add(new Spirit(new Vector2(-100, -100)));
        DebugLogger.LogObject($"Spirit spawned at (-100,-100)");
    }

    private const string Room1 = "Content/levels/roomOne.xml";
    private const string Room2 = "Content/levels/roomTwo.xml";

    private int _currentRoom = 1;
    public int CurrentRoom => _currentRoom;

    private void InitializeLevel()
    {
        DebugLogger.LogRoomTransition($"InitializeLevel: loading room 1 ({Room1})");
        _level = new LevelLoader();
        _level.Load(Room1);
        _currentRoom = 1;
        LoadObstacles();
        DebugLogger.LogRoomTransition($"InitializeLevel: room 1 loaded");
    }

    public void TransitionToRoom(int roomNumber)
    {
        int previousRoom = _currentRoom;
        string targetFile = roomNumber == 1 ? Room1 : Room2;
        DebugLogger.LogRoomTransition($"TransitionToRoom: room {previousRoom} -> room {roomNumber} ({targetFile})");
        _level = new LevelLoader();
        _level.Load(targetFile);
        _currentRoom = roomNumber;

        _knight.SetPosition(_level.KnightSpawn);
        LoadObstacles();
        DebugLogger.LogRoomTransition($"TransitionToRoom: room {roomNumber} loaded, knight at {_level.KnightSpawn}");
    }

    private void InitializeDebug()
    {
        DebugRenderer.Initialize(GraphicsDevice);
        _hudFont = Content.Load<SpriteFont>("fonts/Credits");
        DebugRenderer.LoadFont(_hudFont);
        TextureAtlas hudAtlas = TextureAtlas.FromFile(Content, "sprites/hud-atlas.xml");
        _healthHud = new HealthHud(hudAtlas);
        _soulHud = new SoulHud(hudAtlas);
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

            camera = _camera;
    }

    private void InitializeControllers()
    {
        _controllerList.Clear();
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

    private void RestartGame()
    {
        InitializeNavigationGrid();
        _projectileManager.Clear();
        InitializeItems();
        InitializeLevel();
        InitializeGameplaySystems();
        InitializePlayerAndProjectiles();
        InitializeCameraAndRooms();
        InitializeControllers();
        SetPlaying();
    }
}
