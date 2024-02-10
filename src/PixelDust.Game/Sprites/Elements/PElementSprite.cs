using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Game.Sprites.Elements
{
    public sealed class PElementSprite
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle Rectangle { get; private set; }

        public PElementSprite(Texture2D texture, Vector2 position, Rectangle rectangle)
        {
            this.Texture = texture;
            this.Position = position;
            this.Rectangle = rectangle;
        }
    }
}
