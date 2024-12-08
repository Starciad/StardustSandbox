using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.Extensions
{
    public static class STextureExtensions
    {
        public static SSize2 GetSize(this Texture2D texture)
        {
            return new(texture.Width, texture.Height);
        }
    }
}
