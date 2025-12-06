using Microsoft.Xna.Framework;

using StardustSandbox.Enums.World;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Interfaces.Tools
{
    internal interface IToolContext
    {
        World World { get; }
        Point Position { get; }
        Layer Layer { get; }

        void Update(Point position, Layer layer);
    }
}
