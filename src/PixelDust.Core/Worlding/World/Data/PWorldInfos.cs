using PixelDust.Core.Mathematics;

namespace PixelDust.Core.Worlding
{
    public class PWorldInfos
    {
        public Size2Int Size => worldSizes[1];

        private static readonly Size2Int[] worldSizes = new Size2Int[]
        {
            new(40, 20), // (0) Small
            new(80, 40), // (1) Medium
            new(160, 80), // (2) Large
        };
    }
}
