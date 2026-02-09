using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HollowKnight.Interfaces;
using HollowKnight.Controllers;
using HollowKnight.Commands;
using HollowKnight.Factories;
using HollowKnight.Storage;
using System.Collections.Generic;
using HollowKnight.Player;

namespace HollowKnight;

/// <summary>
/// Main game class. Handles initialization, content loading, and the game loop.
/// </summary>
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private List<IController> _controllerList;

    private int _screenWidth;
    private int _screenHeight;

    private TheKnight _knight;
    private Texture2D _knightSprite;

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

        _knightSprite = Content.Load<Texture2D>("knight"); 
        _knight = new TheKnight(_knightSprite, new Vector2(100, 400));

        // TODO: Create your game sprites using the factory
        // Vector2 playerStart = new Vector2(_screenWidth / 2, _screenHeight / 2);
        // ISprite playerSprite = SpriteFactory.Instance.CreatePlayerIdleSprite(playerStart);
        // _currentSprite = playerSprite;

        // Setup keyboard controller
        KeyboardController keyboard = new KeyboardController();
        // TODO: Register your game commands
        // keyboard.RegisterCommand(Keys.Escape, new QuitCommand(this));
        // keyboard.RegisterCommand(Keys.Space, new JumpCommand(player));
        keyboard.RegisterHeldCommand(Keys.A, new PlayerMoveLeftCommand(_knight));
        keyboard.RegisterHeldCommand(Keys.D, new PlayerMoveRightCommand(_knight));

        keyboard.RegisterReleasedCommand(Keys.A, new PlayerStopMovingHorizontalCommand(_knight));
        keyboard.RegisterReleasedCommand(Keys.D, new PlayerStopMovingHorizontalCommand(_knight));

        keyboard.RegisterPressedCommand(Keys.Space, new PlayerJumpCommand(_knight));
        keyboard.RegisterPressedCommand(Keys.X, new PlayerAttackCommand(_knight));

        keyboard.RegisterPressedCommand(Keys.Escape, new QuitCommand(this));
        _controllerList.Add(keyboard);

        // Setup mouse controller (optional)
        MouseController mouse = new MouseController(_screenWidth, _screenHeight, this);
        mouse.RegisterRightClickCommand(new QuitCommand(this));
        _controllerList.Add(mouse);
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

        _knight.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Change background color to match your game
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Draw current sprite
        _knight.Draw(_spriteBatch);

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
