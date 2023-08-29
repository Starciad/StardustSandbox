using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace PixelDust.Core.Engine
{
    /// <summary>
    /// Static class responsible for managing, storing and configuring game fonts.
    /// </summary>
    public static class PFonts
    {
        public static SpriteFont Arial => fonts["Arial"];

        private static readonly Dictionary<string, SpriteFont> fonts = new();

        /// <summary>
        /// Loads all character fonts that will be used in the project.
        /// </summary>
        internal static void Load()
        {
            fonts.Add("Arial", PContent.Fonts.Load<SpriteFont>("Arial"));
        }
    }
}
