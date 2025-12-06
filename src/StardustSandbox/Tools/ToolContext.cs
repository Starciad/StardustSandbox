using Microsoft.Xna.Framework;

using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Tools;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Tools
{
    internal sealed class ToolContext(World world) : IToolContext
    {
        internal World World => world;
        internal Point Position => this.position;
        internal Layer Layer => this.layer;

        World IToolContext.World => this.World;
        Point IToolContext.Position => this.Position;
        Layer IToolContext.Layer => this.Layer;

        private Point position;
        private Layer layer;

        internal void Update(Point position, Layer layer)
        {
            this.position = position;
            this.layer = layer;
        }

        void IToolContext.Update(Point position, Layer layer)
        {
            Update(position, layer);
        }
    }
}
