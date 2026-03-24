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
using System.Collections.Generic;
using HollowKnight.Pathfinding;
using HollowKnight.Abilities;
using HollowKnight.Storage;
using HollowKnight.Shared;
using IHCCollidable = HollowKnight.Collision.ICollidable;

namespace HollowKnight;

public class Game1 : Game
{
    private SpriteEffects _spriteEffects = SpriteEffects.None;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private NavigationGrid _navigationGrid;
    private List<IController> _controllerList;
    private List<Spirit> items = new();
    private int _screenWidth;
    private int _screenHeight;

    private TheKnight _knight;
    private CollisionHandler _collisionHandler;
    private ProjectileManager _projectileManager = new ProjectileManager();
    private ProjectileSpawner _projectileSpawner;
    private KnightProjectile _knightProjectile;
    private Texture2D _pixel;
    private Camera _camera;
    private RoomManager _roomManager;
    private LevelLoader _level;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
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

        _screenWidth = _graphics.PreferredBackBufferWidth;
        _screenHeight = _graphics.PreferredBackBufferHeight;

        _navigationGrid ??= new NavigationGrid(_screenWidth, _screenHeight, GraphicsDevice, cellSize: 12);

        Spirit spirit = new Spirit(new Vector2(100, 100));
        Spirit spirit_2 = new Spirit(new Vector2(-100, -100));
        items.Add(spirit);
        items.Add(spirit_2);

        _level = new LevelLoader();
        _level.Load("Content/levels/levelOne.xml");
        LoadObstacles();

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _collisionHandler = new CollisionHandler();
        DebugRenderer.Initialize(GraphicsDevice);
        DebugRenderer.LoadFont(Content.Load<SpriteFont>("fonts/Credits"));

        // Touching any enemy damages the knight
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<Crawlid, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage(side));
            _collisionHandler.Register<Vengefly, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage(side));
        }

        // Sword touching enemy damages enemy
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<SwordHitbox, Crawlid>(side, (a, b) => ((Crawlid)b).TakeDamage(side));
            _collisionHandler.Register<SwordHitbox, Vengefly>(side, (a, b) => ((Vengefly)b).TakeDamage(side));
        }

        _projectileSpawner = new ProjectileSpawner(_projectileManager);

        // Block collision registration for all environment types
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<Spirit, TheKnight>(side, (a, b) => ((TheKnight)b).Collect(side));
            _collisionHandler.Register<TutorialPlatform, TheKnight>(side, (a, b) => CollisionManager.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<Path, TheKnight>(side, (a, b) => CollisionManager.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<PathLedge, TheKnight>(side, (a, b) => CollisionManager.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<Spike, TheKnight>(side, (a, b) => CollisionManager.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
        }

        // Setup knight and camera
        var sprites = KnightSpriteBuilder.BuildKnightSprites(_level.KnightSpawn);
        _knight = new TheKnight(sprites, _level.KnightSpawn);
        _knightProjectile = new KnightProjectile(_knight, _projectileSpawner);

        const int levelWidth = 3200;
        const int levelHeight = 720;
        _camera = new Camera(_screenWidth, _screenHeight, levelWidth, levelHeight);
        _roomManager = new RoomManager(
            _knight,
            _camera,
            _screenWidth,
            _screenHeight,
            levelWidth,
            levelHeight);

        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this, _roomManager);
        _controllerList.Add(keyboard);
        _controllerList.Add(new MouseController(_screenWidth, _roomManager));
    }

    private void LoadObstacles()
    {
        foreach (IObject obj in _level.Platforms)
        {
            if (obj != null)
            {
                Rectangle[] hitBoxes = obj.GetBounds();
                foreach (Rectangle rect in hitBoxes)
                {
                    _navigationGrid.AddObstacle(rect);
                }
            }
        }
    }

    private void DrawRectangleOutline(Rectangle rect, Color color, int thickness = 2)
    {
        if (!DebugRenderer.hitboxEnabled) return;
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, rect.Width, thickness), color);
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Bottom - thickness, rect.Width, thickness), color);
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, thickness, rect.Height), color);
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Top, thickness, rect.Height), color);
    }

    protected override void Update(GameTime gameTime)
    {
        foreach (IController controller in _controllerList)
        {
            controller.Update(gameTime);
        }

        _knight.Update(gameTime);
        _roomManager.Update(gameTime);

        // Level boundary clamping
        const int levelWidth = 3200;
        Rectangle[] knightBounds = _knight.GetBounds();
        Rectangle kb = knightBounds[0];
        if (kb.Bottom >= _screenHeight)
        {
            _knight.position.Y = _screenHeight - kb.Height;
            _knight.Land();
        }
        if (_knight.position.X < 0) _knight.position.X = 0;
        if (kb.Right > levelWidth) _knight.position.X = levelWidth - kb.Width;

        Vector2 knightPosition = _knight.GetBounds()[0].Center.ToVector2();

        // Check all collisions between knight and objects
        foreach (IObject obj in _level.Platforms)
        {
            if (obj == null) continue;
            CollisionSide side = CollisionDetector.Detect(obj, _knight);
            _collisionHandler.HandleCollision(obj, _knight, side);
        }
        foreach (IEnemy enemy in _level.Enemies)
        {
            enemy.SetKnightPosition(knightPosition);
            enemy.SetNavigationGrid(_navigationGrid);
            if (!enemy.IsActive) continue;
            CollisionSide side = CollisionDetector.Detect(enemy, _knight);
            _collisionHandler.HandleCollision(enemy, _knight, side);
        }

        SwordHitbox swordHitbox = _knight.GetSwordHitbox();
        if (swordHitbox != null)
        {
            foreach (IEnemy enemy in _level.Enemies)
            {
                if (!enemy.IsActive) continue;
                CollisionSide side = CollisionDetector.Detect(swordHitbox, enemy);
                _collisionHandler.HandleCollision(swordHitbox, enemy, side);
            }
        }

        foreach (IEnemy enemy in _level.Enemies)
            enemy.Update(gameTime);

        _knightProjectile.Update(gameTime);

        for (int i = 0; i < _level.Platforms.Count; i++)
        {
            if (_level.Platforms[i] != null)
                _level.Platforms[i].Update(gameTime);
        }

        for (int i = 0; i < _level.Platforms.Count; i++)
        {
            if (_level.Platforms[i] is IHCCollidable blockObj)
                CollisionManager.ResolvePlayerBlockCollision(_knight, blockObj);
        }

        _projectileManager.Update(gameTime);

        IHCCollidable player = _knight;
        IHCCollidable[] enemyCollidables = CollisionGroupBuilder.GetCollidables(_level.Enemies.ToArray());
        IHCCollidable[] blockCollidables = CollisionGroupBuilder.GetCollidables(_level.Platforms.ToArray());

        CollisionManager.ResolveProjectileCollisions(
            _projectileManager,
            player,
            enemyCollidables,
            blockCollidables,
            onPlayerHit: () => _knight.TakeDamage(),
            onEnemyHit: (enemyIndex) =>
            {
                // TODO: Call enemy TakeDamage here
            }
        );

        // TODO: Refactor so that in xml we have an item list
        foreach (Spirit item in items)
        {
            CollisionSide side = CollisionDetector.Detect(item, _knight);
            _collisionHandler.HandleCollision(item, _knight, side);
            if (side != CollisionSide.None)
            {
                item.IsActive = false;
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Change background color to match game
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _camera.GetTransform());

        if (NavigationGrid.GridEnabled) _navigationGrid.Draw(_spriteBatch);
        _knight.Draw(_spriteBatch);

        foreach (IObject obj in _level.Platforms)
            if (obj != null) obj.Draw(_spriteBatch, _spriteEffects);

        foreach (IEnemy enemy in _level.Enemies)
        {
            enemy.Draw(_spriteBatch, _spriteEffects);
            DebugRenderer.DrawBounds(_spriteBatch, enemy, DebugRenderer.ColorEnemy);
            DrawRectangleOutline(enemy.GetHurtbox(), Color.DarkRed);
            DebugRenderer.DrawStateLabel(_spriteBatch, enemy, enemy.GetStateName(), DebugRenderer.ColorEnemy);

            if (enemy.GetDetectionRadius() > 0)
            {
                Vector2 center = enemy.GetBounds()[0].Center.ToVector2();
                DebugRenderer.DrawRadius(_spriteBatch, center, enemy.GetDetectionRadius(), DebugRenderer.ColorTrigger * 0.8f);
            }
        }
        foreach (var p in _projectileManager.All)
        {
            DrawRectangleOutline(p.Bounds, Color.Red);
        }

        // TODO: Refactor once we have list of items from level loader
        foreach (Spirit item in items)
        {
            if (item.IsActive == true)
            {
                item.Draw(_spriteBatch);
                DebugRenderer.DrawBounds(_spriteBatch, item, DebugRenderer.ColorEnvironment);
            }
        }

        DebugRenderer.DrawBounds(_spriteBatch, _knight, DebugRenderer.ColorKnight);
        DebugRenderer.DrawPoint(_spriteBatch, _knight.GetBounds()[0].Center.ToVector2(), DebugRenderer.ColorMidpoint);
        DrawRectangleOutline(_knight.Bounds, Color.LimeGreen);
        DrawRectangleOutline(_knight.GetHurtbox(), Color.DarkRed);

        DebugRenderer.DrawStateLabel(_spriteBatch, _knight, _knight.GetStateName(), DebugRenderer.ColorKnight);

        string atkText = $"Attack CoolDown: {_knight.GetAttackCooldownRemaining():F2}s";
        string invText = $"Invincibility CoolDown: {_knight.GetInvincibilityCooldownRemaining():F2}s";
        DebugRenderer.DrawText(_spriteBatch, atkText, new Vector2(10, 10), Color.White);
        DebugRenderer.DrawText(_spriteBatch, invText, new Vector2(10, 30), Color.White);

        SwordHitbox swordHitbox = _knight.GetSwordHitbox();
        if (swordHitbox != null)
        {
            DebugRenderer.DrawBounds(_spriteBatch, swordHitbox, DebugRenderer.ColorSword);
        }

        foreach (var p in _projectileManager.All)
        {
            DrawRectangleOutline(p.Bounds, Color.Red);
        }

        for (int i = 0; i < _level.Platforms.Count; i++)
        {
            if (_level.Platforms[i] is IHCCollidable obj)
            {
                _level.Platforms[i].Draw(_spriteBatch, SpriteEffects.None);
                DrawRectangleOutline(obj.Bounds, Color.Blue);
            }
        }

        foreach (IObject obj in _level.Platforms)
            if (obj != null) DebugRenderer.DrawBounds(_spriteBatch, obj, DebugRenderer.ColorEnvironment);

        foreach (IEnemy enemy in _level.Enemies)
        {
            if (enemy == null || !enemy.IsActive) continue;
            DebugRenderer.DrawPath(_spriteBatch, enemy, enemy.GetCurrentPath(), Color.Yellow, Color.Red);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    public void SwitchToNextRoom()
    {
        _roomManager.SwitchRoomByOffset(1);
    }

    public void SwitchToPreviousRoom()
    {
        _roomManager.SwitchRoomByOffset(-1);
    }
}
