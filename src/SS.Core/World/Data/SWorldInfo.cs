using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.World.Data
{
    public sealed class SWorldInfo
    {
        public SSize2 Size => this.size;

        private SSize2 size;

        public void SetSize(SSize2 size)
        {
            this.size = size;
        }
    }
}
