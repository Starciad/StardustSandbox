using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Core.IO.Files.Saving.World.Information;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.IO.Files.Saving.World.Content
{
    [MessagePackObject]
    public sealed class SSaveFileWorldSlot
    {
        [IgnoreMember]
        public Point Position
        {
            get => new(this.PositionX, this.PositionY);

            set
            {
                this.PositionX = value.X;
                this.PositionY = value.Y;
            }
        }

        [Key(0)] public int PositionX { get; set; }
        [Key(1)] public int PositionY { get; set; }
        [Key(2)] public SSaveFileWorldSlotLayer ForegroundLayer { get; set; }
        [Key(3)] public SSaveFileWorldSlotLayer BackgroundLayer { get; set; }

        public SSaveFileWorldSlot()
        {

        }

        public SSaveFileWorldSlot(SSaveFileWorldResources resources, SWorldSlot worldSlot)
        {
            this.Position = worldSlot.Position;

            if (!worldSlot.ForegroundLayer.IsEmpty)
            {
                this.ForegroundLayer = new(resources, worldSlot.ForegroundLayer);
            }

            if (!worldSlot.BackgroundLayer.IsEmpty)
            {
                this.BackgroundLayer = new(resources, worldSlot.BackgroundLayer);
            }
        }
    }
}
