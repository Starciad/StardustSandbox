using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.Engine
{
    /// <summary>
    /// Static class responsible for managing, storing and configuring game fonts.
    /// </summary>
    public static class PFonts
    {
        public static SpriteFont Arial { get; private set; }

        internal static void Load()
        {
            Arial = PContent.Load<SpriteFont>("Fonts/Arial");
        }
    }
}
