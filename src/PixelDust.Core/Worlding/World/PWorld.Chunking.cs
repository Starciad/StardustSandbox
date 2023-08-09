using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.Worlding
{
    public static partial class PWorld
    {
        public static int GetActiveChunksCount()
        {
            return GetComponent<PWorldChunkingComponent>().GetActiveChunksCount();
        }

        public static bool TryGetChunkUpdateState(Vector2 pos, out bool result)
        {
            return GetComponent<PWorldChunkingComponent>().TryGetChunkUpdateState(pos, out result);
        }

        public static bool TryNotifyChunk(Vector2 pos)
        {
            return GetComponent<PWorldChunkingComponent>().TryNotifyChunk(pos);
        }
    }
}