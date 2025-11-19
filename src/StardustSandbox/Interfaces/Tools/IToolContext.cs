using Microsoft.Xna.Framework;

using StardustSandbox.Enums.World;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Interfaces.Tools
{
    internal interface IToolContext
    {
        World World { get; }
        Point Position { get; }
        LayerType Layer { get; }

        void Update(Point position, LayerType worldLayer);
    }
}
