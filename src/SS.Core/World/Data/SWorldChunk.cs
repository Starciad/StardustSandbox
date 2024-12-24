using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;

namespace StardustSandbox.Core.World.Data
{
    public sealed class SWorldChunk(Point position)
    {
        public Point Position => position;
        public bool ShouldUpdate => this.activeCooldown > 0;

        private byte activeCooldown = SWorldConstants.CHUNK_DEFAULT_COOLDOWN;

        public void Update()
        {
            if (this.activeCooldown > 0)
            {
                this.activeCooldown--;
            }
        }

        public void Notify()
        {
            SetCooldown(SWorldConstants.CHUNK_DEFAULT_COOLDOWN);
        }

        internal void SetCooldown(byte value)
        {
            this.activeCooldown = value;
        }
    }
}
