using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.Sprites
{
    internal sealed class PElementSprite
    {
        internal Texture2D Texture { get; private set; }
        internal Vector2 Position { get; private set; }
        internal Rectangle Rectangle { get; private set; }

        public PElementSprite(Texture2D texture, Vector2 position, Rectangle rectangle)
        {
            Texture = texture;
            Position = position;
            Rectangle = rectangle;
        }
    }
}
