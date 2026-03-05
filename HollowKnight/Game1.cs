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
using HollowKnight.Pathfinding;

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
    // private IObject[] Objects = new IObject[7]; UPDATE commented out to test enemies only
    // TODO: Replace with your game's sprite management
    private ISprite _currentSprite;
    private List<IController> _controllerList;

    // private IObjects[] enviromentSprites;
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

        _navigationGrid ??= new NavigationGrid(_screenWidth, _screenHeight, GraphicsDevice);


        Vector2 centerPosition = new Vector2(_screenWidth / 2, _screenHeight / 2);

        loadEnemies();
        // loadEnviroment(); UPDATE commented out to test enemies only



        var sprites = KnightSpriteBuilder.BuildKnightSprites(centerPosition);
        _knight = new TheKnight(sprites, centerPosition);

        _collisionHandler = new CollisionHandler();
        DebugRenderer.Initialize(GraphicsDevice);
        DebugRenderer.LoadFont(Content.Load<SpriteFont>("fonts/Credits"));
        // Touching any enemy damages the knight
        foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
        {
            _collisionHandler.Register<Crawlid, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage());
            _collisionHandler.Register<Vengefly, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage());
        }

        // Setup keyboard controller
        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this);
        _controllerList.Add(keyboard);
    }


    /* UPDATE commented out to test enemies only
    public void loadEnviroment()
    {
        IObject Path_1 = new Path_1();
        Objects[0] = Path_1;
        IObject Path_2 = new Path_2();
        Objects[1] = Path_2;
        IObject Path_3 = new Path_3();
        Objects[2] = Path_3;
        IObject Path_Ledge = new Path_ledge();
        Objects[3] = Path_Ledge;
        IObject Spike = new Spike();
        Objects[4] = Spike;
        IObject FloorSpike = new FloorSpike();
        Objects[5] = FloorSpike;
        IObject CeilingSpike = new CeilingSpike();
        Objects[6] = CeilingSpike;
    }
    */
    public void loadEnemies()
    {
        IEnemy vengefly_1 = new Vengefly(new Vector2(0, 150));
        Enemies[0] = vengefly_1;
        IEnemy crawlid_1 = new Crawlid(new Vector2(850, 150));
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
        Vector2 knightPosition = _knight.GetBounds().Center.ToVector2(); //use center of knight for enemy detection


        //Check all collisions between knight and objects and handle them
        /*UPDATE commented out to test enemies only
        foreach (IObject obj in Objects)
        {
            CollisionSide side = CollisionDetector.Detect(obj, _knight);
            _collisionHandler.HandleCollision(obj, _knight, side);
        }
        */
        foreach (IEnemy enemy in Enemies)
        {
            enemy.SetKnightPosition(knightPosition);
            CollisionSide side = CollisionDetector.Detect(enemy, _knight);
            _collisionHandler.HandleCollision(enemy, _knight, side);
        }


        foreach (IEnemy enemy in Enemies)
            enemy.Update(gameTime);
        /* UPDATE commented out to test enemies only
        foreach (IObject obj in Objects)
            obj.Update(gameTime);
        */
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Change background color to match your game
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _navigationGrid.Draw(_spriteBatch);
        // Draw current sprite
        _knight.Draw(_spriteBatch);

        /* UPDATE commented out to test enemies only
        foreach (IObject obj in Objects)
            obj.Draw(_spriteBatch, _spriteEffects);
            */

        foreach (IEnemy enemy in Enemies)
        {
            enemy.Draw(_spriteBatch, _spriteEffects);
            DebugRenderer.DrawBounds(_spriteBatch, enemy, DebugRenderer.ColorEnemy);
            DebugRenderer.DrawStateLabel(_spriteBatch, enemy, enemy.GetStateName(), DebugRenderer.ColorEnemy);

            if (enemy.GetDetectionRadius() > 0)
            {
                Vector2 center = enemy.GetBounds().Center.ToVector2();
                DebugRenderer.DrawRadius(_spriteBatch, center, enemy.GetDetectionRadius(), DebugRenderer.ColorTrigger * 0.8f);
            }
        }

        DebugRenderer.DrawBounds(_spriteBatch, _knight, DebugRenderer.ColorKnight);
        DebugRenderer.DrawPoint(_spriteBatch, _knight.GetBounds().Center.ToVector2(), DebugRenderer.ColorMidpoint);

        /* UPDATE commented out to test enemies only
        foreach (IObject obj in Objects)
            DebugRenderer.DrawBounds(_spriteBatch, obj, DebugRenderer.ColorEnvironment);
            */
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
