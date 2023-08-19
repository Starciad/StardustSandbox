using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;

namespace PixelDust.Core.Worlding
{
    public class PWorldInfos
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public PWorldInfos()
        {
            Viewport viewport = PGraphics.Viewport;
            Width = viewport.Width / PWorld.Scale;
            Height = viewport.Height / PWorld.Scale;
        }

        internal void SetWidth(uint value)
        {
            Width = (int)value;
        }
        internal void SetHeight(uint value)
        {
            Height = (int)value;
        }
    }
}
