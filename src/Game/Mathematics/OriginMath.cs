using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Directions;

namespace StardustSandbox.Mathematics
{
    internal static class OriginMath
    {
        internal static Vector2 GetSpriteFontOriginPoint(this SpriteFont spriteFont, string text, UIDirection direction)
        {
            Vector2 measuredString = spriteFont.MeasureString(text);
            return GetOriginPoint(new(measuredString.X, measuredString.Y), direction);
        }

        internal static Vector2 GetOriginPoint(Vector2 size, UIDirection direction)
        {
            return direction switch
            {
                // (.)
                UIDirection.Center => size / 2f,

                // (↑)
                UIDirection.North => new(size.X / 2f, size.Y),

                // (↗)
                UIDirection.Northeast => new(0f, size.Y),

                // (→)
                UIDirection.East => new(0f, size.Y / 2f),

                // (↘)
                UIDirection.Southeast => new(0f, 0f),

                // (↓)
                UIDirection.South => new(size.X / 2f, 0f),

                // (↙)
                UIDirection.Southwest => new(size.X, 0f),

                // (←)
                UIDirection.West => new(size.X, size.Y / 2f),

                // (↖)
                UIDirection.Northwest => new(size.X, size.Y),

                // (.)
                _ => new(size.X, size.Y),
            };
        }
    }
}
