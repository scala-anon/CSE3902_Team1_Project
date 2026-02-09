using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Storage
{
    public static class Texture2DStorage
    {
        private static SpriteFont defaultFont;

        public static void LoadAllTextures(ContentManager content)
        {
            // Knight and enemy textures are now loaded via XML atlases in SpriteFactory.
            // Only load non-atlas assets here (fonts, etc.)

            // defaultFont = content.Load<SpriteFont>("fonts/Credits");
        }

        //UNCOMMENT IF YOU ADD A FONT
        // public static SpriteFont GetDefaultFont()
        // {
        //     return defaultFont;
        // }
    }
}
