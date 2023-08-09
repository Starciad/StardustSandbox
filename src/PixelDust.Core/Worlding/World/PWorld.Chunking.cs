using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        public bool TryGetChunkUpdateState(Vector2 pos, out bool result)
        {
            return _chunking.TryGetChunkUpdateState(pos, out result);
        }

        public bool TryNotifyChunk(Vector2 pos)
        {
            return _chunking.TryNotifyChunk(pos);
        }
    }
}