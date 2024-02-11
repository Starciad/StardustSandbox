using PixelDust.Game.Mathematics;

namespace PixelDust.Game.World.Data
{
    public class PWorldInfos
    {
        public Size2Int Size => worldSizes[2];

        private static readonly Size2Int[] worldSizes = new Size2Int[]
        {
            // (0) Small
            new(40, 23),

            // (1) Medium
            new(80, 46),

            // (2) Large
            new(160, 92),
        };
    }
}
