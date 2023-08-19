using Microsoft.Xna.Framework;

namespace PixelDust.Core.Engine
{
    public static class PTime
    {
        public static GameTime UpdateGameTime { get; private set; }
        public static GameTime DrawGameTime { get; private set; }

        internal static void Update(GameTime value)
        {
            UpdateGameTime = value;
        }

        internal static void Draw(GameTime value)
        {
            DrawGameTime = value;
        }
    }
}
