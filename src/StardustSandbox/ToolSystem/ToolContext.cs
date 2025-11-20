using Microsoft.Xna.Framework;

using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Tools;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.ToolSystem
{
    internal sealed class ToolContext(World world) : IToolContext
    {
        internal World World => world;
        internal Point Position => this.position;
        internal LayerType Layer => this.layer;

        World IToolContext.World => this.World;

        Point IToolContext.Position => this.Position;

        LayerType IToolContext.Layer => this.Layer;

        private Point position;
        private LayerType layer;

        internal void Update(Point position, LayerType layer)
        {
            this.position = position;
            this.layer = layer;
        }

        void IToolContext.Update(Point position, LayerType layer)
        {
            Update(position, layer);
        }
    }
}
