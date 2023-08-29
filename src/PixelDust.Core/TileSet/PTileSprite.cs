using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.TileSet
{
    internal sealed class PTileSprite
    {
        internal Texture2D Texture { get; private set; }
        internal Vector2 Position { get; private set; }
        internal Rectangle Rectangle { get; private set; }

        public PTileSprite(Texture2D texture)
        {
            Texture = texture;
        }

        internal PTileSprite Build(Vector2 position, Rectangle rectangle)
        {
            Position = position;
            Rectangle = rectangle;

            return this;
        }
    }
}
