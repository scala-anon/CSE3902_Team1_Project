using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Shared
{
    public abstract class GameState
    {
        public virtual bool ShowsHealthHud => true;

        public abstract void Update(Game1 game, GameTime gameTime);
        public abstract void DrawOverlay(Game1 game, SpriteBatch spriteBatch);
    }

    public sealed class PlayingState : GameState
    {
        public override void Update(Game1 game, GameTime gameTime)
        {
            game.UpdateKnight(gameTime);
            game.UpdateRoom(gameTime);
            game.UpdateCollisions();
            if (game.KnightIsDead())
            {
                game.SetGameOver();
                return;
            }

            game.UpdateEnemies(gameTime);
            game.UpdateKnightProjectiles(gameTime);
            game.UpdatePlatforms(gameTime);
            game.UpdateProjectiles(gameTime);
        }

        public override void DrawOverlay(Game1 game, SpriteBatch spriteBatch)
        {
        }
    }

    public sealed class PausedState : GameState
    {
        public override void Update(Game1 game, GameTime gameTime)
        {
        }

        public override void DrawOverlay(Game1 game, SpriteBatch spriteBatch) => game.DrawPausedStateOverlay();
    }

    public sealed class InventoryState : GameState
    {
        public override void Update(Game1 game, GameTime gameTime)
        {
        }

        public override void DrawOverlay(Game1 game, SpriteBatch spriteBatch) => game.DrawInventoryStateOverlay();
    }

    public sealed class GameOverState : GameState
    {
        public override bool ShowsHealthHud => false;

        public override void Update(Game1 game, GameTime gameTime)
        {
        }

        public override void DrawOverlay(Game1 game, SpriteBatch spriteBatch) => game.DrawGameOverStateOverlay();
    }

    public sealed class WinState : GameState
    {
        public override void Update(Game1 game, GameTime gameTime)
        {
        }

        public override void DrawOverlay(Game1 game, SpriteBatch spriteBatch) => game.DrawWinStateOverlay();
    }
}
