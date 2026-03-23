using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Controllers;
using HollowKnight.Factories;
using HollowKnight.Builders;
using HollowKnight.Player;
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
    private List<Spirit> items = new ();
    private int _screenWidth;
    private int _screenHeight;

    private TheKnight _knight;
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

        Spirit spirit = new Spirit(new Vector2(100,100));
        Spirit spirit_2 = new Spirit(new Vector2(-100,-100));
        items.Add(spirit);
        items.Add(spirit_2);

        //loadEnemies();
        loadEnviroment();
        
        

        var sprites = KnightSpriteBuilder.BuildKnightSprites(centerPosition);
        _knight = new TheKnight(sprites, centerPosition);

        _collisionHandler = new CollisionHandler();
        DebugRenderer.Initialize(GraphicsDevice);
        // TODO: Register collision responses here (sub-branches)

        
        
        
        
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
        IObject FloorSpike = new FloorSpike(new Vector2(900,300));
        Objects[5] = FloorSpike;
        IObject CeilingSpike = new CeilingSpike(new Vector2(600,300));
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
    protected override void Update(GameTime gameTime)
    {
        // Update all controllers
        foreach (IController controller in _controllerList)
        {
            controller.Update(gameTime);
        }

        _knight.Update(gameTime);

        //Check all collisions between knight and objects and handle them
        foreach (IObject obj in Objects)
        {
            CollisionSide side = CollisionDetector.Detect(obj, _knight);
            _collisionHandler.HandleCollision(obj, _knight, side);
        }
        
        foreach (Spirit item in items){
            
            CollisionSide side = CollisionDetector.Detect(item, _knight);
            _collisionHandler.HandleCollision(item, _knight, side);
            if (side != CollisionSide.None)
            {
                item.IsActive = false;
            }
               
        }

        for (int i = 0; i < items.Count - 1; i++)
        {
            if (!items[i].IsActive)
            {
                items.Remove(items[i]);
            }
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

        foreach (Spirit item in items)
        {
            if (item.IsActive == true)
            {
                item.Draw(_spriteBatch);
                DebugRenderer.DrawBounds(_spriteBatch, item, DebugRenderer.ColorEnvironment);
            }
            
            
        }
            
        
        
        
        //Enemies[enemy_index].Draw(_spriteBatch, _spriteEffects);
        Objects[enviroment_index].Draw(_spriteBatch, _spriteEffects);


        DebugRenderer.DrawBounds(_spriteBatch, _knight, DebugRenderer.ColorKnight);
        foreach (IObject obj in Objects)
        {
            DebugRenderer.DrawBounds(_spriteBatch, obj, DebugRenderer.ColorEnvironment);
        } 
        
        //foreach (IEnemy enemy in Enemies)
        //{
        //    DebugRenderer.DrawBounds(_spriteBatch, enemy, DebugRenderer.ColorEnemy);
        //}

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
