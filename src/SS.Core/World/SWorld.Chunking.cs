using Microsoft.Xna.Framework;

using StardustSandbox.Core.World.Data;

namespace StardustSandbox.Core.World
{
    public sealed partial class SWorld
    {
        public int GetActiveChunksCount()
        {
            return this.worldChunkingComponent.GetActiveChunksCount();
        }

        public SWorldChunk[] GetActiveChunks()
        {
            return this.worldChunkingComponent.GetActiveChunks();
        }

        public bool GetChunkUpdateState(Point pos)
        {
            _ = TryGetChunkUpdateState(pos, out bool result);
            return result;
        }

        public void NotifyChunk(Point pos)
        {
            _ = TryNotifyChunk(pos);
        }

        public bool TryGetChunkUpdateState(Point pos, out bool result)
        {
            return this.worldChunkingComponent.TryGetChunkUpdateState(pos, out result);
        }

        public bool TryNotifyChunk(Point pos)
        {
            return this.worldChunkingComponent.TryNotifyChunk(pos);
        }
    }
}
