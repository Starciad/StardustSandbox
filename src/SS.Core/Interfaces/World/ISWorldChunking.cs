using Microsoft.Xna.Framework;

using StardustSandbox.Core.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorldChunking
    {
        int GetActiveChunksCount();
        IEnumerable<SWorldChunk> GetActiveChunks();

        bool GetChunkUpdateState(Point position);
        void NotifyChunk(Point position);
        bool TryGetChunkUpdateState(Point position, out bool result);
        bool TryNotifyChunk(Point position);
    }
}
