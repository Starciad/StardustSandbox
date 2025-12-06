using Microsoft.Xna.Framework;

using StardustSandbox.Constants;

namespace StardustSandbox.WorldSystem.Chunking
{
    internal sealed class Chunk(Point position)
    {
        internal Point Position => position;
        internal bool ShouldUpdate => this.activeCooldown > 0;

        private byte activeCooldown = WorldConstants.CHUNK_DEFAULT_COOLDOWN;

        internal void Update()
        {
            if (this.activeCooldown > 0)
            {
                this.activeCooldown--;
            }
        }

        internal void Notify()
        {
            SetCooldown(WorldConstants.CHUNK_DEFAULT_COOLDOWN);
        }

        internal void SetCooldown(byte value)
        {
            this.activeCooldown = value;
        }
    }
}
