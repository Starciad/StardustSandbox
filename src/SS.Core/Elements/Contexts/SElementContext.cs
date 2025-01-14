using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements.Contexts;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.Elements.Contexts
{
    internal sealed partial class SElementContext(ISWorld world) : ISElementContext
    {
        public SWorldSlot Slot => this.worldSlot;
        public SWorldSlotLayer SlotLayer => this.worldSlot.GetLayer(this.worldLayer);
        public Point Position => this.worldSlot.Position;
        public SWorldLayer Layer => this.worldLayer;

        private SWorldLayer worldLayer;
        private SWorldSlot worldSlot;

        private readonly ISWorld world = world;

        public void UpdateInformation(Point position, SWorldLayer worldLayer, SWorldSlot worldSlot)
        {
            worldSlot.SetPosition(position);

            this.worldLayer = worldLayer;
            this.worldSlot = worldSlot;
        }
    }
}