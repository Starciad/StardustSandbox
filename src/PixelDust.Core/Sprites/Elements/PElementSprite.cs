using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.Sprites.Elements
{
    internal sealed class PElementSprite
    {
        internal Texture2D Texture { get; private set; }
        internal Vector2 Position { get; private set; }
        internal Rectangle Rectangle { get; private set; }

        public PElementSprite(Texture2D texture, Vector2 position, Rectangle rectangle)
        {
            this.Texture = texture;
            this.Position = position;
            this.Rectangle = rectangle;
        }
    }
}
