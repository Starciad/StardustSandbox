using Microsoft.Xna.Framework;

using StardustSandbox.Enums.World;
using StardustSandbox.World;

namespace StardustSandbox.Interfaces.Tools
{
    internal interface IToolContext
    {
        GameWorld World { get; }
        Point Position { get; }
        Layer Layer { get; }

        void Update(Point position, Layer layer);
    }
}
