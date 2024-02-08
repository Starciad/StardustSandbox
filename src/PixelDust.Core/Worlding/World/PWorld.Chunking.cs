using PixelDust.Core.Worlding.Components.Chunking;
using PixelDust.Mathematics;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        internal int GetActiveChunksCount()
        {
            return GetComponent<PWorldChunkingComponent>().GetActiveChunksCount();
        }
        internal bool TryGetChunkUpdateState(Vector2Int pos, out bool result)
        {
            return GetComponent<PWorldChunkingComponent>().TryGetChunkUpdateState(pos, out result);
        }
        internal bool TryNotifyChunk(Vector2Int pos)
        {
            return GetComponent<PWorldChunkingComponent>().TryNotifyChunk(pos);
        }
    }
}
