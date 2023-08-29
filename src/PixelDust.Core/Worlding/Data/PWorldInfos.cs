using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;

namespace PixelDust.Core.Worlding
{
    public class PWorldInfos
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private static readonly (int, int)[] worldSizes = new (int, int)[]
        {
            (40, 20), // (0) Small
            (80, 40), // (1) Medium
            (160, 80), // (2) Large
        };

        public PWorldInfos()
        {
            Width = worldSizes[2].Item1;
            Height = worldSizes[2].Item2;
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
