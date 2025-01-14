using Microsoft.Xna.Framework;

using StardustSandbox.Core.World.Chunking;

using System.Collections.Generic;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        public int GetActiveChunksCount()
        {
            return this.worldChunkingComponent.GetActiveChunksCount();
        }

        public IEnumerable<SWorldChunk> GetActiveChunks()
        {
            return this.worldChunkingComponent.GetActiveChunks();
        }

        public bool GetChunkUpdateState(Point position)
        {
            _ = TryGetChunkUpdateState(position, out bool result);
            return result;
        }

        public void NotifyChunk(Point position)
        {
            _ = TryNotifyChunk(position);
        }

        public bool TryGetChunkUpdateState(Point position, out bool result)
        {
            return this.worldChunkingComponent.TryGetChunkUpdateState(position, out result);
        }

        public bool TryNotifyChunk(Point position)
        {
            return this.worldChunkingComponent.TryNotifyChunk(position);
        }
    }
}
