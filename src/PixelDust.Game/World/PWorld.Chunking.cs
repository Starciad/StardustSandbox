using Microsoft.Xna.Framework;

using PixelDust.Game.World.Components.Common;

namespace PixelDust.Game.World
{
    public sealed partial class PWorld
    {
        public int GetActiveChunksCount()
        {
            return GetComponent<PWorldChunkingComponent>().GetActiveChunksCount();
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
            return GetComponent<PWorldChunkingComponent>().TryGetChunkUpdateState(pos, out result);
        }
        public bool TryNotifyChunk(Point pos)
        {
            return GetComponent<PWorldChunkingComponent>().TryNotifyChunk(pos);
        }
    }
}
