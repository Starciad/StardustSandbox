using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Tools.Contexts;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.Tools.Contexts
{
    public sealed class SToolContext(ISWorld world) : ISToolContext
    {
        public ISWorld World => world;
        public Point Position => this.position;
        public SWorldLayer Layer => this.layer;

        private Point position;
        private SWorldLayer layer;

        public void Update(Point position, SWorldLayer worldLayer)
        {
            this.position = position;
            this.layer = worldLayer;
        }
    }
}
