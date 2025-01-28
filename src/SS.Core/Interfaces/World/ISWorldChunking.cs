using StardustSandbox.Core.World.Chunking;

using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorldChunking : ISWorldChunkingHandler
    {
        int GetActiveChunksCount();
        IEnumerable<SWorldChunk> GetActiveChunks();
    }
}
