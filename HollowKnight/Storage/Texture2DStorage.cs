using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Storage
{
    /// <summary>
    /// Static storage for all game textures and fonts.
    /// Load textures once here, then access them via getter methods.
    /// Add new textures as private static fields with public getters.
    /// </summary>
    public static class Texture2DStorage
    {
        // TODO: Add your sprite sheet textures here
        // private static Texture2D playerSpriteSheet;
        // private static Texture2D enemySpriteSheet;
        // private static Texture2D itemSpriteSheet;

        private static Texture2D knightSpriteSheet;

        public static Texture2D GetKnightSpriteSheet()
        {
            return knightSpriteSheet;
        }
        private static SpriteFont defaultFont;

        /// <summary>
        /// Load all game textures. Call this once in Game1.LoadContent().
        /// </summary>
        public static void LoadAllTextures(ContentManager content)
        {
            // TODO: Load your sprite sheets here
            // playerSpriteSheet = content.Load<Texture2D>("sprites/player_sprite_sheet");
            // enemySpriteSheet = content.Load<Texture2D>("sprites/enemy_sprite_sheet");

            knightSpriteSheet = content.Load<Texture2D>("sprites/HollowKnightSheet");
            // defaultFont = content.Load<SpriteFont>("fonts/Credits");
        }

        // TODO: Add getter methods for each texture
        // public static Texture2D GetPlayerSpriteSheet()
        // {
        //     return playerSpriteSheet;
        // }

        //UNCOMMENT IF YOU ADD A FONT
        // public static SpriteFont GetDefaultFont()
        // {
        //     return defaultFont;
        // }
    }
}
