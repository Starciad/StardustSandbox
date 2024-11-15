using Microsoft.Xna.Framework;

using StardustSandbox.Core.World.Components;

namespace StardustSandbox.Core.World
{
    public sealed partial class SWorld
    {
        public int GetActiveChunksCount()
        {
            return GetComponent<SWorldChunkingComponent>().GetActiveChunksCount();
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
            return GetComponent<SWorldChunkingComponent>().TryGetChunkUpdateState(pos, out result);
        }
        public bool TryNotifyChunk(Point pos)
        {
            return GetComponent<SWorldChunkingComponent>().TryNotifyChunk(pos);
        }
    }
}
