using Microsoft.Xna.Framework;

using StardustSandbox.Enums.World;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Tools
{
    internal sealed class ToolContext(World world)
    {
        internal World World => world;
        internal Point Position => this.position;
        internal Layer Layer => this.layer;

        private Point position;
        private Layer layer;

        internal void Update(in Point position, in Layer layer)
        {
            this.position = position;
            this.layer = layer;
        }
    }
}
