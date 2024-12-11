using Microsoft.Xna.Framework;

using StardustSandbox.Core.World.Data;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorldChunking
    {
        int GetActiveChunksCount();
        SWorldChunk[] GetActiveChunks();

        bool GetChunkUpdateState(Point pos);
        void NotifyChunk(Point pos);
        bool TryGetChunkUpdateState(Point pos, out bool result);
        bool TryNotifyChunk(Point pos);
    }
}
