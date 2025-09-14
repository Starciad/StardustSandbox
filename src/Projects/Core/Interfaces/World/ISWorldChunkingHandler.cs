using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorldChunkingHandler
    {
        bool GetChunkUpdateState(Point position);
        void NotifyChunk(Point position);
        bool TryGetChunkUpdateState(Point position, out bool result);
        bool TryNotifyChunk(Point position);
    }
}
