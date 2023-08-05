using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core
{
    public static class PFonts
    {
        public static SpriteFont Arial { get; private set; }

        internal static void Load()
        {
            Arial = PContent.Load<SpriteFont>("Fonts/Arial");
        }
    }
}
