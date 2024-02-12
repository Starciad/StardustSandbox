using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Enums.General;

namespace PixelDust.Game.Extensions
{
    public static class PTexture2DExtensions
    {
        public static Vector2 GetOrigin(this Texture2D texture, PCardinalDirection direction)
        {
            return direction switch
            {
                PCardinalDirection.Center => new Vector2(texture.Width, texture.Height) / 2f,
                PCardinalDirection.North => new Vector2(texture.Width / 2f, 0),
                PCardinalDirection.Northeast => new Vector2(texture.Width, 0),
                PCardinalDirection.East => new Vector2(texture.Width, texture.Height / 2f),
                PCardinalDirection.Southeast => new Vector2(texture.Width, texture.Height),
                PCardinalDirection.South => new Vector2(texture.Width / 2f, texture.Height),
                PCardinalDirection.Southwest => new Vector2(0, texture.Height),
                PCardinalDirection.West => new Vector2(0, texture.Height / 2f),
                PCardinalDirection.Northwest => Vector2.Zero,
                _ => Vector2.Zero,
            };
        }
    }
}
