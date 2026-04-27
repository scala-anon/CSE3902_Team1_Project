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
using HollowKnight.Enemies;

namespace HollowKnight;

public partial class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private NavigationGrid _navigationGrid;
    private List<IController> _controllerList;
    private readonly List<Spirit> _items = new();

    private GameState _gameState = new TitleState();

    private TheKnight _knight;
    private CollisionSystem _collisionSystem;
    private DebugOverlay _debugOverlay;
    private readonly ProjectileManager _projectileManager = new();
    private ProjectileSpawner _projectileSpawner;
    private KnightProjectile _knightProjectile;
    private EnemyProjectile _mantisProjectileLeft;
    private EnemyProjectile _mantisProjectileMiddle;
    private EnemyProjectile _mantisProjectileRight;
    private RoomManager _roomManager;
    private LevelLoader _level;
    internal List<Interfaces.IObject> LevelPlatforms => _level.Platforms;
    private Dictionary<int, Vector2> _roomEntryPoints = new();
    private readonly HashSet<string> _destroyedObjects = new();
    public void RegisterDestroyed(string key) => _destroyedObjects.Add(key);
    public bool IsDestroyed(string key) => _destroyedObjects.Contains(key);
    private bool _isTransitioning = false;
    private bool _pendingControllerInit = false;
    private readonly ScreenFader _fader = new();
    private ParallaxBackground _parallaxBackground;


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
        CollisionLayerMatrix.ValidateSymmetry();
        _controllerList = new List<IController>();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        InitializeAudio();
        InitializeRendering();
        InitializeSharedResources();
        InitializeNavigationGrid();
        InitializeItems();
        InitializeLevel();
        InitializeDebug();
        InitializeGameplaySystems();
        InitializePlayerAndProjectiles();
        ApplyRoomRespawnPoint();
        InitializeCameraAndRooms();
        InitializeControllers();
        InitializeFullScreen();
    }

    protected override void Update(GameTime gameTime)
    {
        if (_gameState is PlayingState)
        {
            UpdateCollisions();
        }

        UpdateControllers(gameTime);

        if (_restartRequested)
        {
            _restartRequested = false;
            RestartGame();
            base.Update(gameTime);
            return;
        }

        _gameState.Update(this, gameTime);
        UpdateAudio();

        if (_pendingControllerInit)
        {
            _pendingControllerInit = false;
            InitializeControllers();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Navy);

        if (!IsTitleScreenOpen())
        {
            DrawWorld();
            DrawHud();
        }

        DrawOverlay();
        DrawFadeOverlay();

        base.Draw(gameTime);
    }
}
