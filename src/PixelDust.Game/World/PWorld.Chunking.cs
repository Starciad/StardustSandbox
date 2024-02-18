using PixelDust.Game.Mathematics;
using PixelDust.Game.World.Components.Common;

namespace PixelDust.Game.World
{
    public sealed partial class PWorld
    {
        public int GetActiveChunksCount()
        {
            return GetComponent<PWorldChunkingComponent>().GetActiveChunksCount();
        }

        public bool GetChunkUpdateState(Vector2Int pos)
        {
            _ = TryGetChunkUpdateState(pos, out bool result);
            return result;
        }
        public void NotifyChunk(Vector2Int pos)
        {
            _ = TryNotifyChunk(pos);
        }

        public bool TryGetChunkUpdateState(Vector2Int pos, out bool result)
        {
            return GetComponent<PWorldChunkingComponent>().TryGetChunkUpdateState(pos, out result);
        }
        public bool TryNotifyChunk(Vector2Int pos)
        {
            return GetComponent<PWorldChunkingComponent>().TryNotifyChunk(pos);
        }
    }
}
