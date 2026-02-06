using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HollowKnight.Interfaces;
using HollowKnight.Controllers;
using HollowKnight.Commands;
using HollowKnight.Factories;
using HollowKnight.Storage;
using System.Collections.Generic;

namespace HollowKnight;

/// <summary>
/// Main game class. Handles initialization, content loading, and the game loop.
/// </summary>
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // TODO: Replace with your game's sprite management
    private ISprite _currentSprite;
    private List<IController> _controllerList;

    private ISprite[] enviromentSprites;
    private int _screenWidth;
    private int _screenHeight;

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
        Texture2DStorage.LoadAllTextures(Content);
        SpriteFactory.Instance.LoadAllTextures();

        _screenWidth = _graphics.PreferredBackBufferWidth;
        _screenHeight = _graphics.PreferredBackBufferHeight;

        // TODO: Create your game sprites using the factory
        // Vector2 playerStart = new Vector2(_screenWidth / 2, _screenHeight / 2);
        // ISprite playerSprite = SpriteFactory.Instance.CreatePlayerIdleSprite(playerStart);
        // _currentSprite = playerSprite;

        ISprite[] enviromentSprites = new ISprite[5];
        ISprite object_1 = SpriteFactory.Instance.createObject1Sprite(_position, _screenWidth);
        enviromentSprites[0] = object_1;
        /// Initialize other 4 or 5 objects (Do we want to have an easier way to create these with parameters???)




        // Setup keyboard controller
        KeyboardController keyboard = new KeyboardController();
        // TODO: Register your game commands
        // keyboard.RegisterCommand(Keys.Escape, new QuitCommand(this));
        // keyboard.RegisterCommand(Keys.Space, new JumpCommand(player));
        _controllerList.Add(keyboard);

        // Setup mouse controller (optional)
        MouseController mouse = new MouseController(_screenWidth, _screenHeight, this);
        mouse.RegisterRightClickCommand(new QuitCommand(this));
        _controllerList.Add(mouse);
    }

    /// <summary>
    /// Set the currently displayed sprite, preserving position.
    /// </summary>
    public void SetSprite(ISprite sprite)
    {
        if (_currentSprite != null)
        {
            Vector2 currentPosition = _currentSprite.GetPosition();
            sprite.SetPosition(currentPosition);
        }
        _currentSprite = sprite;
    }

    public Vector2 GetCurrentSpritePosition()
    {
        return _currentSprite?.GetPosition() ?? Vector2.Zero;
    }

    public void SetCurrentSpritePosition(Vector2 position)
    {
        _currentSprite?.SetPosition(position);
    }

    protected override void Update(GameTime gameTime)
    {
        // Update all controllers
        foreach (IController controller in _controllerList)
        {
            controller.Update(gameTime);
        }

        // TODO: Add your game update logic here
        // - Player movement
        // - Enemy AI
        // - Collision detection
        // - Game state management

        _currentSprite?.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Change background color to match your game
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Draw current sprite
        _currentSprite?.Draw(_spriteBatch);

        enviromentSprites[0].Draw(_spriteBatch); // Initially draw first sprite in array
        
        // TODO: Draw your game elements here
        // - Background layers
        // - Game objects
        // - UI elements
        // - Debug info

        // Example: Draw credits/debug text
        SpriteFont font = Texture2DStorage.GetDefaultFont();
        _spriteBatch.DrawString(font, "Hollow Knight Clone - Team 1", new Vector2(50, _screenHeight - 50), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
