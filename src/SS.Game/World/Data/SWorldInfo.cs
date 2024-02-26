using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.World.Data
{
    public sealed class SWorldInfo
    {
        public SSize2 Size => worldSizes[2];

        private static readonly SSize2[] worldSizes =
        [
            // (0) Small
            new SSize2(40, 23),

            // (1) Medium
            new SSize2(80, 46),

            // (2) Large
            new SSize2(160, 92),
        ];
    }
}
