using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Game.Items
{
    public abstract class PItemDetails
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Category { get; protected set; }
        public Texture2D IconTexture { get; protected set; }
    }
}
