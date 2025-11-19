using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Directions;

using System.Text;

namespace StardustSandbox.Mathematics
{
    internal static class OriginMath
    {
        internal static Vector2 GetSpriteFontOriginPoint(this SpriteFont spriteFont, StringBuilder text, CardinalDirection direction)
        {
            Vector2 measuredString = spriteFont.MeasureString(text);
            return GetOriginPoint(new(measuredString.X, measuredString.Y), direction);
        }

        internal static Vector2 GetSpriteFontOriginPoint(this SpriteFont spriteFont, string text, CardinalDirection direction)
        {
            Vector2 measuredString = spriteFont.MeasureString(text);
            return GetOriginPoint(new(measuredString.X, measuredString.Y), direction);
        }

        internal static Vector2 GetTextureOriginPoint(this Texture2D texture, CardinalDirection direction)
        {
            return GetOriginPoint(new(texture.Width, texture.Height), direction);
        }

        internal static Vector2 GetOriginPoint(Vector2 size, CardinalDirection direction)
        {
            return direction switch
            {
                // (.)
                CardinalDirection.Center => size / 2f,

                // (↑)
                CardinalDirection.North => new Vector2(size.X / 2f, size.Y),

                // (↗)
                CardinalDirection.Northeast => new Vector2(0f, size.Y),

                // (→)
                CardinalDirection.East => new Vector2(0f, size.Y / 2f),

                // (↘)
                CardinalDirection.Southeast => new Vector2(0f, 0f),

                // (↓)
                CardinalDirection.South => new Vector2(size.X / 2f, 0f),

                // (↙)
                CardinalDirection.Southwest => new Vector2(size.X, 0f),

                // (←)
                CardinalDirection.West => new Vector2(size.X, size.Y / 2f),

                // (↖)
                CardinalDirection.Northwest => new Vector2(size.X, size.Y),

                // (.)
                _ => new Vector2(size.X, size.Y),
            };
        }
    }
}
