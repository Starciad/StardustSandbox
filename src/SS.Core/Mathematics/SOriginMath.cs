using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SharpDX.Direct3D9;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Text;

namespace StardustSandbox.Core.Mathematics
{
    public static class SOriginMath
    {
        public static Vector2 GetSpriteFontOriginPoint(this SpriteFont spriteFont, StringBuilder text, SCardinalDirection direction)
        {
            Vector2 measuredString = spriteFont.MeasureString(text);
            return GetOriginPoint(new(measuredString.X, measuredString.Y), direction);
        }

        public static Vector2 GetTextureOriginPoint(this Texture2D texture, SCardinalDirection direction)
        {
            return GetOriginPoint(new(texture.Width, texture.Height), direction);
        }

        private static Vector2 GetOriginPoint(SSize2F size, SCardinalDirection direction)
        {
            return direction switch
            {
                SCardinalDirection.Center => new Vector2(size.Width, size.Height) / 2f,
                SCardinalDirection.North => new Vector2(size.Width / 2f, 0),
                SCardinalDirection.Northeast => new Vector2(size.Width, 0),
                SCardinalDirection.East => new Vector2(size.Width, size.Height / 2f),
                SCardinalDirection.Southeast => new Vector2(size.Width, size.Height),
                SCardinalDirection.South => new Vector2(size.Width / 2f, size.Height),
                SCardinalDirection.Southwest => new Vector2(0, size.Height),
                SCardinalDirection.West => new Vector2(0, size.Height / 2f),
                SCardinalDirection.Northwest => Vector2.Zero,
                _ => Vector2.Zero,
            };
        }
    }
}
