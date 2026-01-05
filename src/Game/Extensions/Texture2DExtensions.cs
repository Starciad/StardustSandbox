using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Extensions
{
    internal static class Texture2DExtensions
    {
        internal static Point GetSize(this Texture2D texture)
        {
            return new(texture.Width, texture.Height);
        }
    }
}
