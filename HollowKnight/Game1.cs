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
using HollowKnight.Ability_Classes;
using HollowKnight.Storage;


namespace HollowKnight;

/// <summary>
/// Main game class. Handles initialization, content loading, and the game loop.
/// </summary>
public class Game1 : Game
{
    private SpriteEffects _spriteEffects;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public int enemy_index = 0;
    public int enviroment_index = 0;
    private IEnemy[] Enemies = new IEnemy[2];
    private IObject[] Objects = new IObject[7];
    // TODO: Replace with your game's sprite management
    private ISprite _currentSprite;
    private List<IController> _controllerList;
    private List<IPickup> items = new ();
    private int _screenWidth;
    private int _screenHeight;

    private TheKnight _knight;
    private ProjectileManager _projectileManager = new ProjectileManager();
    private float projectileTimer = 0f;
    private Texture2D _pixel;
    private CollisionHandler _collisionHandler;

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

        Vector2 centerPosition = new Vector2(_screenWidth / 2, _screenHeight / 2);

        IPickup spirit = new Spirit(new Vector2(100,100));
        IPickup spirit_2 = new Spirit(new Vector2(-100,-100));
        items.Add(spirit);
        items.Add(spirit_2);

        //loadEnemies();
        loadEnviroment();
        
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        

        var sprites = KnightSpriteBuilder.BuildKnightSprites(centerPosition);
        _knight = new TheKnight(sprites, centerPosition);

        _collisionHandler = new CollisionHandler();
        DebugRenderer.Initialize(GraphicsDevice);
        // TODO: Register collision responses here (sub-branches)

        
        
        
        
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<Spirit, TheKnight>(side, (a, b) => ((TheKnight)b).Collect(side));
            
        }


        // Setup keyboard controller
        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this);
        _controllerList.Add(keyboard);

        
        
    }



    public void loadEnviroment()
    {
        IObject Path_1 = new Path_1(new Vector2(0, 600));
        Objects[0] = Path_1;
        IObject Path_2 = new Path_2(new Vector2(50, 0));
        Objects[1] = Path_2;
        IObject Path_3 = new Path_3(new Vector2(250, 200));
        Objects[2] = Path_3;
        IObject Path_Ledge = new Path_ledge(new Vector2(800, 200));
        Objects[3] = Path_Ledge;
        IObject Spike = new Spike(new Vector2(950, 150));
        Objects[4] = Spike;
        IObject FloorSpike = new FloorSpike(new Vector2(900,400));
        Objects[5] = FloorSpike;
        IObject CeilingSpike = new CeilingSpike(new Vector2(300,0));
        Objects[6] = CeilingSpike;
    }
    public void loadEnemies()
    {
        IEnemy vengefly_1 = new Vengefly(new Vector2(0, 150));
        Enemies[0] = vengefly_1;
        IEnemy crawlid_1 = new Crawlid(new Vector2(0,150));
        Enemies[1] = crawlid_1;
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

    private void DrawRectangleOutline(Rectangle rect, Color color, int thickness = 2)
    {
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, rect.Width, thickness), color); // top
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Bottom - thickness, rect.Width, thickness), color); // bottom
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, thickness, rect.Height), color); // left
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Top, thickness, rect.Height), color); // right
    }

    protected override void Update(GameTime gameTime)
    {
        // Update all controllers
        foreach (IController controller in _controllerList)
        {
            controller.Update(gameTime);
        }

        _knight.Update(gameTime);

        // Future use case when we have more enemies
        // for (int i = 0; i < Enemies.Length; i++)
        // {
        //     Enemies[i].Update(gameTime);
        // }

        // for (int i = 0; i < Objects.Length; i++)
        // {
        //     Objects[i].Update(gameTime);
        // }

        projectileTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (projectileTimer > 2f)
        {
            projectileTimer = 0f;

            float projectileSpeed = 400f;
            Vector2 direction;
            Vector2 spawn;

            if (_knight.Facing == Direction.Right)
            {
                direction = new Vector2(1f, 0f);
                spawn = new Vector2(
                    _knight.Bounds.Right,
                    _knight.Bounds.Top + _knight.Bounds.Height / 2f
                );
            }
            else
            {
                direction = new Vector2(-1f, 0f);
                spawn = new Vector2(
                    _knight.Bounds.Left - 12f,
                    _knight.Bounds.Top + _knight.Bounds.Height / 2f
                );
            }

            _projectileManager.Spawn(
                new Projectile(spawn, direction * projectileSpeed, ProjectileFaction.Player)
            );
        }

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null)
                Enemies[i].Update(gameTime);
        }

        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] != null)
                Objects[i].Update(gameTime);
        }

        _projectileManager.Update(gameTime);

        ICollideTemp player = (ICollideTemp)_knight;

        List<ICollideTemp> enemyCollidables = new List<ICollideTemp>();
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] is ICollideTemp collidableEnemy)
            {
                enemyCollidables.Add(collidableEnemy);
            }
        }

        List<ICollideTemp> blockCollidables = new List<ICollideTemp>();
        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] is ICollideTemp collidableObject)
            {
                blockCollidables.Add(collidableObject);
            }
        }

        CollisionManager.ResolveProjectileCollisions(
            _projectileManager,
            player,
            enemyCollidables.ToArray(),
            blockCollidables.ToArray(),
            onPlayerHit: () => _knight.TakeDamage(),
            onEnemyHit: (enemyIndex) =>
            {
                // If your enemy has TakeDamage later, call it here.
                // For now you can mark dead or trigger state machine.
                // Example (if you add it): ((IDamageable)Enemies[enemyIndex]).TakeDamage(1);
            }
        );

        //Check all collisions between knight and objects and handle them
        foreach (IObject obj in Objects)
        {
            CollisionSide side = CollisionDetector.Detect(obj, _knight);
            _collisionHandler.HandleCollision(obj, _knight, side);
        }
        
        foreach (IPickup item in items){
            
            CollisionSide side = CollisionDetector.Detect(item, _knight);
            _collisionHandler.HandleCollision(item, _knight, side);
               
        }
        
        

        //foreach (IEnemy enemy in Enemies)
        //{
        //    CollisionSide side = CollisionDetector.Detect(enemy, _knight);
        //    _collisionHandler.HandleCollision(enemy, _knight, side);
        //}
        
        



       //Enemies[enemy_index].Update(gameTime);
        Objects[enviroment_index].Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Change background color to match your game
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Draw current sprite
        _knight.Draw(_spriteBatch);

        foreach (var p in _projectileManager.All)
        {
            DrawRectangleOutline(p.Bounds, Color.Red);
        }

    
        // Enemies[enemy_index].Draw(_spriteBatch, _spriteEffects);
        // Objects[enviroment_index].Draw(_spriteBatch, _spriteEffects);
        foreach (IPickup item in items)
        {
            item.Draw(_spriteBatch);
            
        }
            
        
        
        
        //Enemies[enemy_index].Draw(_spriteBatch, _spriteEffects);
        Objects[enviroment_index].Draw(_spriteBatch, _spriteEffects);

        // for (int i = 0; i < Enemies.Length; i++)
        // {
        //     if (Enemies[i] != null)
        //         Enemies[i].Draw(_spriteBatch, SpriteEffects.None);
        // }

        DebugRenderer.DrawBounds(_spriteBatch, _knight, DebugRenderer.ColorKnight);
        foreach (IObject obj in Objects)
        {
            DebugRenderer.DrawBounds(_spriteBatch, obj, DebugRenderer.ColorEnvironment);
        } 
        
        foreach (IPickup item in items){
        
            DebugRenderer.DrawBounds(_spriteBatch, item, DebugRenderer.ColorEnvironment);
            
        }
        //foreach (IEnemy enemy in Enemies)
        //{
        //    DebugRenderer.DrawBounds(_spriteBatch, enemy, DebugRenderer.ColorEnemy);
        //}

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
