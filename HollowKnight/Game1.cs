using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Controllers;
using HollowKnight.Factories;
using HollowKnight.Builders;
using HollowKnight.Player;
using HollowKnight.Projectiles;
using HollowKnight.Collision;
using System.Collections.Generic;
using System.IO;
using HollowKnight.Pathfinding;
using IHCCollidable = HollowKnight.Collision.ICollidable;
using HollowKnight.Ability_Classes;
using HollowKnight.Storage;
using HollowKnight.Shared;
using System.Runtime.Remoting;


namespace HollowKnight;

/// <summary>
/// Main game class. Handles initialization, content loading, and the game loop.
/// </summary>
public class Game1 : Game
{
    private SpriteEffects _spriteEffects;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private NavigationGrid _navigationGrid;
    public int enemy_index = 0;
    public int enviroment_index = 0;
    private IEnemy[] Enemies = new IEnemy[2];
    private IObject[] Objects = new IObject[7];
    // TODO: Replace with your game's sprite management
    private ISprite _currentSprite;
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

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // TODO: Set your game's resolution
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

        // Load all textures and initialize the sprite factory
        SpriteFactory.Instance.LoadAllTextures(Content);

        _screenWidth = _graphics.PreferredBackBufferWidth;
        _screenHeight = _graphics.PreferredBackBufferHeight;

        _navigationGrid ??= new NavigationGrid(_screenWidth, _screenHeight, GraphicsDevice, cellSize: 12);

        Vector2 centerPosition = new Vector2(_screenWidth / 2, _screenHeight / 2);

        Spirit spirit = new Spirit(new Vector2(100, 100));
        Spirit spirit_2 = new Spirit(new Vector2(-100, -100));
        items.Add(spirit);
        items.Add(spirit_2);

        loadEnviroment();
        loadEnemies();

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        var sprites = KnightSpriteBuilder.BuildKnightSprites(centerPosition);
        _knight = new TheKnight(sprites, centerPosition);

        _collisionHandler = new CollisionHandler();
        DebugRenderer.Initialize(GraphicsDevice);
        DebugRenderer.LoadFont(Content.Load<SpriteFont>("fonts/Credits"));

        // Touching any enemy damages the knight
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<Crawlid, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage(side));
            _collisionHandler.Register<Vengefly, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage(side));
        }

        //Sword touching enemy damages enemy
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<SwordHitbox, Crawlid>(side, (a, b) => ((Crawlid)b).TakeDamage(side));
            _collisionHandler.Register<SwordHitbox, Vengefly>(side, (a, b) => ((Vengefly)b).TakeDamage(side));
        }

        _projectileSpawner = new ProjectileSpawner(_projectileManager);
        _knightProjectile = new KnightProjectile(_knight, _projectileSpawner);
        //TODO: delete later
        /* 
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<Spirit, TheKnight>(side, (a, b) => ((TheKnight)b).Collect(side));
            _collisionHandler.Register<FloorSpike, TheKnight>(side, (a, b) => ((TheKnight)b).Block(side));
            _collisionHandler.Register<Path_1, TheKnight>(side, (a, b) => ((TheKnight)b).Block(side));
            _collisionHandler.Register<Path_2, TheKnight>(side, (a, b) => ((TheKnight)b).Block(side));
            _collisionHandler.Register<Path_3, TheKnight>(side, (a, b) => ((TheKnight)b).Block(side));
            _collisionHandler.Register<Spike, TheKnight>(side, (a, b) => ((TheKnight)b).Block(side));
            _collisionHandler.Register<CeilingSpike, TheKnight>(side, (a, b) => ((TheKnight)b).Block(side));
            _collisionHandler.Register<Path_ledge, TheKnight>(side, (a, b) => ((TheKnight)b).Block(side));
        }
    */
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<Spirit, TheKnight>(side, (a, b) => ((TheKnight)b).Collect(side));
            _collisionHandler.Register<Path_1, TheKnight>(side, (a, b) => CollisionResponse.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<Path_2, TheKnight>(side, (a, b) => CollisionResponse.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<Path_3, TheKnight>(side, (a, b) => CollisionResponse.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<Path_ledge, TheKnight>(side, (a, b) => CollisionResponse.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<FloorSpike, TheKnight>(side, (a, b) => CollisionResponse.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<CeilingSpike, TheKnight>(side, (a, b) => CollisionResponse.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
            _collisionHandler.Register<Spike, TheKnight>(side, (a, b) => CollisionResponse.ResolvePlayerBlockCollision((TheKnight)b, (ICollidable)a));
        }



        DebugRenderer.Initialize(GraphicsDevice);
        DebugRenderer.LoadFont(Content.Load<SpriteFont>("fonts/Credits"));

        // Setup keyboard controller
        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this);
        _controllerList.Add(keyboard);
    }


    public void loadEnviroment()
    {
        IObject Path_1 = new Path_1(new Vector2(0, 550));
        Objects[0] = Path_1;
        IObject Path_2 = new Path_2(new Vector2(50, 0));
        Objects[1] = Path_2;
        IObject Path_3 = new Path_3(new Vector2(250, 200));
        Objects[2] = Path_3;
        IObject Path_Ledge = new Path_ledge(new Vector2(800, 200));
        Objects[3] = Path_Ledge;
        IObject Spike = new Spike(new Vector2(950, 150));
        Objects[4] = Spike;
        IObject FloorSpike = new FloorSpike(new Vector2(900, 300));
        Objects[5] = FloorSpike;
        IObject CeilingSpike = new CeilingSpike(new Vector2(600, 300));
        Objects[6] = CeilingSpike;
    }
    public void loadEnemies()
    {
        IEnemy vengefly_1 = new Vengefly(new Vector2(0, 150)); //TODO: change this hardcoded position
        Enemies[0] = vengefly_1;
        IEnemy crawlid_1 = new Crawlid(new Vector2(850, _screenHeight - 83)); //TODO: change this hardcorded position
        Enemies[1] = crawlid_1;

        // Initialize grid obstacles
        foreach (IObject obj in Objects)
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

    //Not being used (potentially can be removed)
    public void SetSprite(ISprite sprite)
    {
        if (_currentSprite != null)
        {
            Vector2 currentPosition = _currentSprite.GetPosition();
            sprite.SetPosition(currentPosition);
        }
        _currentSprite = sprite;
    }

    private void HandleProjectileCollisions()
    {
        ICollidable[] enemyCollidablesArray = new ICollidable[Enemies.Length];
        for (int i = 0; i < Enemies.Length; i++)
        {
            enemyCollidablesArray[i] = (ICollidable)Enemies[i];
        }

        ICollidable[] blockCollidablesArray = new ICollidable[Objects.Length];
        for (int i = 0; i < Objects.Length; i++)
        {
            blockCollidablesArray[i] = (ICollidable)Objects[i];
        }

        CollisionResponse.ResolveProjectileCollisions(
            _projectileManager,
            _knight,
            enemyCollidablesArray,
            blockCollidablesArray,
            onPlayerHit: () => _knight.TakeDamage(),
            onEnemyHit: i =>
            {

            }
        );
    }

    private void DrawRectangleOutline(Rectangle rect, Color color, int thickness = 2)
    {
        if (!HollowKnight.Collision.DebugRenderer.hitboxEnabled) return;
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, rect.Width, thickness), color); // top
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Bottom - thickness, rect.Width, thickness), color); // bottom
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, thickness, rect.Height), color); // left
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Top, thickness, rect.Height), color); // right
    }

    protected override void Update(GameTime gameTime)
    {
        foreach (IController controller in _controllerList)
        {
            controller.Update(gameTime);
        }

        _knight.Update(gameTime);

        // Screen floor boundary (keeps knight in-bounds while environment is not loaded)
        Rectangle[] knightBounds = new Rectangle[1];
        knightBounds = _knight.GetBounds();
        Rectangle kb = knightBounds[0];
        if (kb.Bottom >= _screenHeight)
        {
            _knight.position.Y = _screenHeight - kb.Height;
            _knight.Land();
        }
        if (_knight.position.X < 0) _knight.position.X = 0;
        if (kb.Right > _screenWidth) _knight.position.X = _screenWidth - kb.Width;

        Vector2 knightPosition = _knight.GetBounds()[0].Center.ToVector2(); //use center of knight for enemy detection


        //Check all collisions between knight and objects and handle them
        foreach (IObject obj in Objects)
        {
            if (obj == null) continue;
            CollisionSide side = CollisionDetector.Detect(obj, _knight);
            _collisionHandler.HandleCollision(obj, _knight, side);
        }
        foreach (IEnemy enemy in Enemies)
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
            foreach (IEnemy enemy in Enemies)
            {
                if (!enemy.IsActive) continue;
                CollisionSide side = CollisionDetector.Detect(swordHitbox, enemy);
                _collisionHandler.HandleCollision(swordHitbox, enemy, side);
            }
        }


        foreach (IEnemy enemy in Enemies)
            enemy.Update(gameTime);

        _knightProjectile.Update(gameTime);

        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] != null)
                Objects[i].Update(gameTime);
        }

        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] is HollowKnight.Collision.ICollidable blockObj)
                CollisionManager.ResolvePlayerBlockCollision(_knight, blockObj);
        }

        _projectileManager.Update(gameTime);

        IHCCollidable player = _knight;
        IHCCollidable[] enemyCollidables = CollisionGroupBuilder.GetCollidables(Enemies);
        IHCCollidable[] blockCollidables = CollisionGroupBuilder.GetCollidables(Objects);

        CollisionManager.ResolveProjectileCollisions(
            _projectileManager,
            player,
            enemyCollidables,
            blockCollidables,
            onPlayerHit: () => _knight.TakeDamage(),
            onEnemyHit: (enemyIndex) =>
            {
                // Enemy TakeDamage will be called here
            }
        );

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

        _spriteBatch.Begin();

        if (NavigationGrid.GridEnabled) _navigationGrid.Draw(_spriteBatch);
        // Draw current sprite
        _knight.Draw(_spriteBatch);

        foreach (IObject obj in Objects)
            if (obj != null) obj.Draw(_spriteBatch, _spriteEffects);

        foreach (IEnemy enemy in Enemies)
        {
            enemy.Draw(_spriteBatch, _spriteEffects);
            DebugRenderer.DrawBounds(_spriteBatch, enemy, DebugRenderer.ColorEnemy);
            DrawRectangleOutline(enemy.GetHurtbox(), Color.DarkRed); // Show new combat hurtbox
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
        DrawRectangleOutline(_knight.GetHurtbox(), Color.DarkRed); // Show new combat hurtbox

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

        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] is IHCCollidable obj)
            {
                Objects[i].Draw(_spriteBatch, SpriteEffects.None);
                DrawRectangleOutline(obj.Bounds, Color.Blue);
            }
        }

        foreach (IObject obj in Objects)
            if (obj != null) DebugRenderer.DrawBounds(_spriteBatch, obj, DebugRenderer.ColorEnvironment);

        // Draw A* Paths
        foreach (IEnemy enemy in Enemies)
        {
            if (enemy == null || !enemy.IsActive) continue;
            DebugRenderer.DrawPath(_spriteBatch, enemy, enemy.GetCurrentPath(), Color.Yellow, Color.Red);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
