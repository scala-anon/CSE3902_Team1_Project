using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Controllers;
using HollowKnight.Factories;
using HollowKnight.Builders;
using HollowKnight.Player;
using System.Collections.Generic;

namespace HollowKnight;

public class Game1 : Game
{
    private SpriteEffects _spriteEffects = SpriteEffects.None;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private List<IController> _controllerList;

    private int _screenWidth;
    private int _screenHeight;

    private TheKnight _knight;
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

        _level = new LevelLoader();
        _level.Load("Content/levels/levelOne.xml");

        var sprites = KnightSpriteBuilder.BuildKnightSprites(_level.KnightSpawn);
        _knight = new TheKnight(sprites, _level.KnightSpawn);

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

    protected override void Update(GameTime gameTime)
    {
        foreach (IController controller in _controllerList)
        {
            controller.Update(gameTime);
        }

        _knight.Update(gameTime);
        foreach (IObject obj in _level.Platforms)
        {
            obj.Update(gameTime);
        }
        foreach (IEnemy obj in _level.Enemies)
        {
            obj.Update(gameTime);
        }

        _roomManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _camera.GetTransform());

        foreach (IObject obj in _level.Platforms)
        {
            obj.Draw(_spriteBatch, _spriteEffects);
        }
        foreach (IEnemy obj in _level.Enemies)
        {
            obj.Draw(_spriteBatch, _spriteEffects);
        }
        _knight.Draw(_spriteBatch);

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
