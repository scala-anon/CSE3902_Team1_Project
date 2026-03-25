using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Controllers;
using HollowKnight.Factories;
using HollowKnight.Levels;
using HollowKnight.Player;
using HollowKnight.Projectiles;
using HollowKnight.Collision;
using HollowKnight.Environment;
using HollowKnight.Enemies;
using System.Collections.Generic;
using HollowKnight.Pathfinding;
using HollowKnight.Abilities;
using HollowKnight.Storage;
using HollowKnight.Shared;
using HollowKnight.Graphics;

namespace HollowKnight;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private NavigationGrid _navigationGrid;
    private List<IController> _controllerList;
    private List<Spirit> items = new();

    private GameState _gameState = GameState.Playing;
    private TheKnight _knight;
    private CollisionSystem _collisionSystem;
    private DebugOverlay _debugOverlay;
    private ProjectileManager _projectileManager = new();
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
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteFactory.Instance.LoadAllTextures(Content);

        int screenWidth = _graphics.PreferredBackBufferWidth;
        int screenHeight = _graphics.PreferredBackBufferHeight;

        _navigationGrid ??= new NavigationGrid(GameConstants.DefaultLevelWidth, GameConstants.DefaultLevelHeight, GraphicsDevice, cellSize: GameConstants.NavGridCellSize);

        Spirit spirit = new Spirit(new Vector2(100, 100));
        Spirit spirit_2 = new Spirit(new Vector2(-100, -100));
        items.Add(spirit);
        items.Add(spirit_2);

        _level = new LevelLoader();
        _level.Load("Content/levels/levelOne.xml");
        LoadObstacles();

        DebugRenderer.Initialize(GraphicsDevice);
        DebugRenderer.LoadFont(Content.Load<SpriteFont>("fonts/Credits"));

        _collisionSystem = new CollisionSystem();
        _debugOverlay = new DebugOverlay(GraphicsDevice);
        _projectileSpawner = new ProjectileSpawner(_projectileManager);

        // Setup knight and camera
        var sprites = KnightSpriteBuilder.BuildKnightSprites(_level.KnightSpawn);
        _knight = new TheKnight(sprites, _level.KnightSpawn);
        _knightProjectile = new KnightProjectile(_knight, _projectileSpawner);

        _camera = new Camera(screenWidth, screenHeight, GameConstants.DefaultLevelWidth, GameConstants.DefaultLevelHeight);
        _roomManager = new RoomManager(
            _knight, _camera,
            screenWidth, screenHeight,
            GameConstants.DefaultLevelWidth, GameConstants.DefaultLevelHeight);

        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this, _roomManager);
        _controllerList.Add(keyboard);
        _controllerList.Add(new MouseController(screenWidth, _roomManager));
    }

    private void LoadObstacles()
    {
        foreach (IObject obj in _level.Platforms)
        {
            if (obj == null) continue;
            foreach (Rectangle rect in obj.GetBounds())
                _navigationGrid.AddObstacle(rect);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        // Input is always processed (so pause toggle works)
        foreach (IController controller in _controllerList)
            controller.Update(gameTime);

        if (_gameState == GameState.Playing)
        {
            _knight.Update(gameTime);
            _roomManager.Update(gameTime);

            _collisionSystem.Update(_knight, _level.Platforms, _level.Enemies, items, _projectileManager, _navigationGrid);

            foreach (IEnemy enemy in _level.Enemies)
                enemy.Update(gameTime);

            _knightProjectile.Update(gameTime);

            for (int i = 0; i < _level.Platforms.Count; i++)
            {
                if (_level.Platforms[i] != null)
                    _level.Platforms[i].Update(gameTime);
            }

            _projectileManager.Update(gameTime);
        }

        base.Update(gameTime);
    }

    public void TogglePause()
    {
        _gameState = _gameState == GameState.Playing ? GameState.Paused : GameState.Playing;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _camera.GetTransform());

        _knight.Draw(_spriteBatch);

        foreach (IObject obj in _level.Platforms)
            if (obj != null) obj.Draw(_spriteBatch, SpriteEffects.None);

        foreach (IEnemy enemy in _level.Enemies)
            enemy.Draw(_spriteBatch, SpriteEffects.None);

        foreach (Spirit item in items)
        {
            if (item.IsActive)
                item.Draw(_spriteBatch);
        }

        _debugOverlay.Draw(_spriteBatch, _knight, _level.Platforms, _level.Enemies, items, _projectileManager, _navigationGrid);

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    public void SwitchToNextRoom() => _roomManager.SwitchRoomByOffset(1);
    public void SwitchToPreviousRoom() => _roomManager.SwitchRoomByOffset(-1);
}
