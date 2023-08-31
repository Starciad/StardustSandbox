using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;
using PixelDust.Core.Mathematics;

namespace PixelDust.Core.Worlding
{
    public class PWorldInfos
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private static readonly Size2Int[] worldSizes = new Size2Int[]
        {
            new(40, 20), // (0) Small
            new(80, 40), // (1) Medium
            new(160, 80), // (2) Large
        };

        public PWorldInfos()
        {
            Width = worldSizes[2].Width;
            Height = worldSizes[2].Height;
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
