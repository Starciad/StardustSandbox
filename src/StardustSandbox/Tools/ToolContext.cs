using Microsoft.Xna.Framework;

using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Tools;
using StardustSandbox.World;

namespace StardustSandbox.Tools
{
    internal sealed class ToolContext(GameWorld world) : IToolContext
    {
        internal GameWorld World => world;
        internal Point Position => this.position;
        internal LayerType Layer => this.layer;

        GameWorld IToolContext.World => this.World;
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
