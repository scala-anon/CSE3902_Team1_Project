using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Levels;
using HollowKnight.Player;
using HollowKnight.Projectiles;
using HollowKnight.Collision;
using System.Collections.Generic;
using HollowKnight.Pathfinding;
using HollowKnight.Shared;
using HollowKnight.Graphics;
using Microsoft.Xna.Framework.Audio;
using HollowKnight.Abilities;

namespace HollowKnight;

public partial class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private NavigationGrid _navigationGrid;
    private List<IController> _controllerList;
    private readonly List<Spirit> _items = new();

    private GameState _gameState = GameState.Playing;

    private TheKnight _knight;
    private CollisionSystem _collisionSystem;
    private DebugOverlay _debugOverlay;
    private readonly ProjectileManager _projectileManager = new();
    private ProjectileSpawner _projectileSpawner;
    private KnightProjectile _knightProjectile;
    private Camera _camera;
    private RoomManager _roomManager;
    private LevelLoader _level;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = GameConstants.ScreenWidth;
        _graphics.PreferredBackBufferHeight = GameConstants.ScreenHeight;
    }

    protected override void Initialize()
    {
        _controllerList = new List<IController>();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        InitializeRendering();
        InitializeSharedResources();
        InitializeNavigationGrid();
        InitializeItems();
        InitializeLevel();
        InitializeDebug();
        InitializeGameplaySystems();
        InitializePlayerAndProjectiles();
        InitializeCameraAndRooms();
        InitializeControllers();
        InitializeAudio();
    }

    protected override void Update(GameTime gameTime)
    {
        // Always process input so pause/unpause/quit work in any state
        UpdateControllers(gameTime);

        switch (_gameState)
        {
            case GameState.Playing:
                UpdateCollisions();
                UpdatePlayingLogic(gameTime);
                break;

            case GameState.Paused:
                break;

            case GameState.Inventory:
                break;

            case GameState.GameOver:
                break;

            case GameState.Win:
                break;
        }

        UpdateAudio();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        DrawWorld();
        DrawOverlay();

        base.Draw(gameTime);
    }
}