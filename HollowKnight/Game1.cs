using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Controllers;
using HollowKnight.Factories;
using HollowKnight.Builders;
using HollowKnight.Player;
using System.Collections.Generic;
using System.IO;

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
    private IObject[] Objects = new IObject[17];
    // TODO: Replace with your game's sprite management
    private ISprite _currentSprite;
    private List<IController> _controllerList;

   // private IObjects[] enviromentSprites;
    private int _screenWidth;
    private int _screenHeight;

    private TheKnight _knight;
    private LevelLoader level;
    private Camera _camera;
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
         
        // loadEnemies();
        // loadEnviroment();
        level = new LevelLoader();
        level.Load("Content/levels/levelOne.xml");//"C:\\Users\\Thoma\\Downloads\\OSU\\CSE3902\\project\\CSE3902_Team1_Project\\HollowKnight\\Content\\levels\\levelOne.xml");
        

        var sprites = KnightSpriteBuilder.BuildKnightSprites(level.KnightSpawn);
        _knight = new TheKnight(sprites, level.KnightSpawn);

        // Setup keyboard controller
        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this);
        _controllerList.Add(keyboard);

        const int LEVEL_WIDTH = 3200;
        const int LEVEL_HEIGHT = 720;
        _camera = new Camera(_screenWidth, _screenHeight, LEVEL_WIDTH, LEVEL_HEIGHT);
    }



    // public void loadEnviroment()
    // {
    //     IObject Path_1 = new Path_1();
    //     Objects[0] = Path_1;
    //     IObject Path_2 = new Path_2();
    //     Objects[1] = Path_2;
    //     IObject Path_3 = new Path_3();
    //     Objects[2] = Path_3;
    //     IObject Path_Ledge = new Path_ledge();
    //     Objects[3] = Path_Ledge;
    //     IObject Spike = new Spike();
    //     Objects[4] = Spike;
    //     IObject FloorSpike = new FloorSpike();
    //     Objects[5] = FloorSpike;
    //     IObject CeilingSpike = new CeilingSpike();
    //     Objects[6] = CeilingSpike;
    //     IObject TutorialPlatform1 = new Tutorial_Platform_1();
    //     Objects[7] = TutorialPlatform1;
    //     IObject TutorialPlatfrom2 = new Tutorial_Platform_2();
    //     Objects[8] = TutorialPlatfrom2;
    //     IObject TutorialPlatfrom3 = new Tutorial_Platform_3();
    //     Objects[9] = TutorialPlatfrom3;
    //     IObject TutorialPlatfrom4 = new Tutorial_Platform_4();
    //     Objects[10] = TutorialPlatfrom4;
    //     IObject TutorialPlatfrom5 = new Tutorial_Platform_5();
    //     Objects[11] = TutorialPlatfrom5;

    //     IObject TutorialPlatform6 = new Tutorial_Platform_6();
    //     Objects[12] = TutorialPlatform6;
    //     IObject TutorialPlatfrom7 = new Tutorial_Platform_7();
    //     Objects[13] = TutorialPlatfrom7;
    //     IObject TutorialPlatfrom8 = new Tutorial_Platform_8();
    //     Objects[14] = TutorialPlatfrom8;
    //     IObject TutorialPlatfrom9 = new Tutorial_Platform_9();
    //     Objects[15] = TutorialPlatfrom9;
    //     IObject TutorialPlatfrom10 = new Tutorial_Platform_10();
    //     Objects[16] = TutorialPlatfrom10;

    // }
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
        foreach (IEnemy enemy in level.Enemies)
            enemy.Update(gameTime);

        foreach (IObject obj in level.Platforms)
            obj.Update(gameTime);
        _camera.Follow(_knight.GetPosition());
        // Enemies[enemy_index].Update(gameTime);
        // Objects[enviroment_index].Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Change background color to match your game
        GraphicsDevice.Clear(Color.CornflowerBlue);
        // shifts everything automatically w the transformation matrix as a parameter
        _spriteBatch.Begin(transformMatrix: _camera.GetTransform());

        // Knight
        _knight.Draw(_spriteBatch);
        // draws all the enimies 
        foreach (IEnemy enemy in level.Enemies)
            enemy.Draw(_spriteBatch, _spriteEffects);
        //draws all the platforms 
        foreach (IObject obj in level.Platforms)
            obj.Draw(_spriteBatch, _spriteEffects);
        // Enemies[enemy_index].Draw(_spriteBatch, _spriteEffects);
        // Objects[enviroment_index].Draw(_spriteBatch, _spriteEffects);



        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
