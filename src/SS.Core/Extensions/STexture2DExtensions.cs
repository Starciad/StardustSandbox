using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.General;

namespace StardustSandbox.Core.Extensions
{
    public static class STexture2DExtensions
    {
        public static Vector2 GetOrigin(this Texture2D texture, SCardinalDirection direction)
        {
            return direction switch
            {
                SCardinalDirection.Center => new Vector2(texture.Width, texture.Height) / 2f,
                SCardinalDirection.North => new Vector2(texture.Width / 2f, 0),
                SCardinalDirection.Northeast => new Vector2(texture.Width, 0),
                SCardinalDirection.East => new Vector2(texture.Width, texture.Height / 2f),
                SCardinalDirection.Southeast => new Vector2(texture.Width, texture.Height),
                SCardinalDirection.South => new Vector2(texture.Width / 2f, texture.Height),
                SCardinalDirection.Southwest => new Vector2(0, texture.Height),
                SCardinalDirection.West => new Vector2(0, texture.Height / 2f),
                SCardinalDirection.Northwest => Vector2.Zero,
                _ => Vector2.Zero,
            };
        }
    }
}
