using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.Core.Interfaces.Tools.Contexts
{
    public interface ISToolContext
    {
        ISWorld World { get; }
        Point Position { get; }
        SWorldLayer Layer { get; }

        void Update(Point position, SWorldLayer worldLayer);
    }
}
