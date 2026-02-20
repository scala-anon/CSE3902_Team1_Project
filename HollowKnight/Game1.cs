using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Controllers;
using HollowKnight.Factories;
using HollowKnight.Builders;
using HollowKnight.Player;
using System.Collections.Generic;

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

        var sprites = KnightSpriteBuilder.BuildKnightSprites(centerPosition);
        _knight = new TheKnight(sprites, centerPosition);

        // Setup keyboard controller
        KeyboardController keyboard = new KeyboardController();
        KeyboardBindings.BindGameplay(keyboard, _knight, this);
        _controllerList.Add(keyboard);
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
        //UNCOMMENT IF YOU ADD A FONT
        //SpriteFont font = Content.Load<SpriteFont>("fonts/Credits");
        //_spriteBatch.DrawString(font, "Hollow Knight Clone - Team 1", new Vector2(50, _screenHeight - 50), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
