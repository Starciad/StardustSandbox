using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public static Vector2 GetOriginPoint(SSize2F size, SCardinalDirection direction)
        {
            return direction switch
            {
                // (.)
                SCardinalDirection.Center => (size / 2f).ToVector2(),

                // (↑)
                SCardinalDirection.North => new Vector2(size.Width / 2f, size.Height),

                // (↗)
                SCardinalDirection.Northeast => new Vector2(0f, size.Height),

                // (→)
                SCardinalDirection.East => new Vector2(0f, size.Height / 2f),

                // (↘)
                SCardinalDirection.Southeast => new Vector2(0f, 0f),

                // (↓)
                SCardinalDirection.South => new Vector2(size.Width / 2f, 0f),

                // (↙)
                SCardinalDirection.Southwest => new Vector2(size.Width, 0f),

                // (←)
                SCardinalDirection.West => new Vector2(size.Width, size.Height / 2f),

                // (↖)
                SCardinalDirection.Northwest => new Vector2(size.Width, size.Height),

                // (.)
                _ => new Vector2(size.Width, size.Height),
            };
        }
    }
}
