using PixelDust.Game.Mathematics;

namespace PixelDust.Game.World.Data
{
    public sealed class PWorldInfo
    {
        public Size2Int Size => worldSizes[2];

        private static readonly Size2Int[] worldSizes =
        [
            // (0) Small
            new Size2Int(40, 23),

            // (1) Medium
            new Size2Int(80, 46),

            // (2) Large
            new Size2Int(160, 92),
        ];
    }
}
