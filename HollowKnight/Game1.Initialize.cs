using System;
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
using HollowKnight.Enemies;

namespace HollowKnight;

public partial class Game1
{
    private void InitializeRendering()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _overlayPixel = new Texture2D(GraphicsDevice, 1, 1);
        _overlayPixel.SetData(new[] { Color.White });
    }

    private void InitializeFullScreen()
    {
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();       
    }

    private void InitializeSharedResources()
    {
        SpriteFactory.Instance.LoadAllTextures(Content);
        _parallaxBackground = new ParallaxBackground(SpriteFactory.Instance.GetMainBackgroundTexture());

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
    private const string Room3 = "Content/levels/roomThree.xml";
     private const string Room4 = "Content/levels/roomFour.xml";
    private string[] rooms = new[] { Room1, Room2, Room3, Room4 };


    private int _currentRoom = 1;
    public int CurrentRoom => _currentRoom;

    private void InitializeDebug()
    {
        DebugRenderer.Initialize(GraphicsDevice);
        _hudFont = Content.Load<SpriteFont>("fonts/Credits");
        _titleBackgroundTexture = Content.Load<Texture2D>("sprites/title-screen-background");
        _titleLogoTexture = Content.Load<Texture2D>("sprites/title-screen-logo");
        DebugRenderer.LoadFont(_hudFont);
        TextureAtlas hudAtlas = TextureAtlas.FromFile(Content, "sprites/hud-atlas.xml");
        Texture2D healthPipTexture = Content.Load<Texture2D>("sprites/health-pip");
        _healthHud = new HealthHud(healthPipTexture);
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
        _knight.Dash.SetProjectileManager(_projectileManager);
        _knight.SetProjectileManager(_projectileManager);
    }

    private void InitializeCameraAndRooms()
    {
        int screenWidth = _graphics.PreferredBackBufferWidth;
        int screenHeight = _graphics.PreferredBackBufferHeight;

        Camera.Instance.Initialize(screenWidth, screenHeight);
        

        _roomManager = new RoomManager(
            _knight,
            screenWidth,
            screenHeight,
            GameConstants.DefaultLevelWidth,
            GameConstants.DefaultLevelHeight);
    }

    private void InitializeControllers()
    {
        _controllerList.Clear();
        int screenWidth = _graphics.PreferredBackBufferWidth;

        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this, _level.BossFight);

        _controllerList.Add(keyboard);
        _controllerList.Add(new MouseController(this, screenWidth));
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

    private void InitializeLevel()
    {
    DebugLogger.LogRoomTransition($"InitializeLevel: loading room 1 ({Room1})");
    _level = new LevelLoader();
    _level.SetGame(this);
    _currentRoom = 1;
    _level.Load(rooms[_currentRoom - 1]);
    LoadObstacles();
    DebugLogger.LogRoomTransition($"InitializeLevel: room 1 loaded");
    
    
    
}


// private void InitializeControllers()
// {
//     _controllerList.Clear();
//     int screenWidth = _graphics.PreferredBackBufferWidth;

//     KeyboardController keyboard = new KeyboardController();
//     KeyboardBindings.BindGameplay(keyboard, _knight, this, _roomManager, _level.BossFight);

//     _controllerList.Add(keyboard);
//     _controllerList.Add(new MouseController(this, screenWidth, _roomManager));
// }

public void TransitionToRoom(int roomNumber)
{
    if (roomNumber < 1 || roomNumber > rooms.Length)
    {
        DebugLogger.LogRoomTransition($"TransitionToRoom: invalid room number {roomNumber}");
        return;
    }

    int previousRoom = _currentRoom;
    string targetFile = rooms[roomNumber - 1];

    DebugLogger.LogRoomTransition($"TransitionToRoom: room {previousRoom} -> room {roomNumber} ({targetFile})");

    _level = new LevelLoader();
    _level.SetGame(this);
    _level.Load(targetFile);
    _currentRoom = roomNumber;
    ApplyRoomRespawnPoint();

    if (_roomEntryPoints.TryGetValue(roomNumber, out Vector2 entryPoint))
        _knight.SetPosition(entryPoint);
    else
        _knight.SetPosition(_level.KnightSpawn);

    Camera.Instance.SnapTo(_knight.GetPosition());

    LoadObstacles();
    _pendingControllerInit = true; // replaces InitializeControllers()
    DebugLogger.LogRoomTransition($"TransitionToRoom: room {roomNumber} loaded, knight at {_knight.position}");

    if (_level.BossFight != null)
        {
             DebugLogger.LogObject($"BossFight is initialized");

            _mantisProjectileLeft = new EnemyProjectile(_level.BossFight.Left, _projectileSpawner);
            _mantisProjectileMiddle = new EnemyProjectile(_level.BossFight.Middle, _projectileSpawner);
            _mantisProjectileRight = new EnemyProjectile(_level.BossFight.Right, _projectileSpawner);

            _level.BossFight.Left.Projectiles = _mantisProjectileLeft; 
            _level.BossFight.Middle.Projectiles = _mantisProjectileMiddle; 
            _level.BossFight.Right.Projectiles = _mantisProjectileRight; 

            AudioManager.Instance.PlaySong(AudioLoader.Instance.Get_Mantis_Lords_Music());
            AudioManager.Instance.TryPlayGoofySong(AudioLoader.Instance.Get_1001_Nights());
        }
}

internal void CheckBenchRespawnTransition()
{
    if (!_knight.NeedsBenchRoomTransition) return;
    _knight.ConsumeBenchRoomTransition();
    int benchRoom = _knight.BenchSpawnRoom;
    _isTransitioning = true; // prevent CheckTransitions from overwriting the bench fade callback
    _fader.StartFadeOut(() =>
    {
        // Remove any stale room-entry point for benchRoom so TransitionToRoom does not
        // snap the knight to the old door/zone position instead of the bench spawn.
        _roomEntryPoints.Remove(benchRoom);

        Camera.Instance.ExitBossClamp();
        TransitionToRoom(benchRoom); // always reload the room to reset enemy/interactable state

        // TransitionToRoom unconditionally sets _isTransitioning = false, but the fader
        // is still in FadingIn state.  Re-assert true so CheckTransitions cannot fire a
        // second spurious room load during the remainder of the fade-in, which would
        // replace _level with a different room's data and empty platforms/enemies.
        _isTransitioning = true;

        _knight.SetPosition(_knight.BenchSpawnPoint);
        Camera.Instance.SnapTo(_knight.BenchSpawnPoint);
        _knight.StartSittingIdle();
        DebugLogger.LogRoomTransition($"Bench respawn to room {benchRoom} at {_knight.BenchSpawnPoint}");
    });
}

internal void UpdateFade(GameTime gameTime)
{
    _fader.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
    // Once the fader fully completes (fade-in finished), release the transition lock so
    // the player can trigger room transitions again.  This covers the bench-respawn path
    // where _isTransitioning is kept true through the fade-in to prevent CheckTransitions
    // from firing a spurious second load while the screen is fading back in.
    if (!_fader.IsActive)
        _isTransitioning = false;
}

private void ApplyRoomRespawnPoint()
{
    if (_knight == null) return;
    if (_level.RespawnPoint.HasValue)
        _knight.SetRoomRespawnPoint(_level.RespawnPoint.Value);
    else
        _knight.ClearRoomRespawnPoint();
}

private void RestartGame()
{
    _roomEntryPoints.Clear();
    _destroyedObjects.Clear();
    _currentRoom = 1;
    _fader.Reset();
    InitializeNavigationGrid();
    _projectileManager.Clear();
    InitializeItems();
    InitializeLevel();
    InitializeGameplaySystems();
    InitializePlayerAndProjectiles();
    ApplyRoomRespawnPoint();
    InitializeCameraAndRooms();
    InitializeControllers();
    SetPlaying();
}
}
