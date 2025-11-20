using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.IO.Saving.World.Information;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.IO.Saving.World.Content
{
    [MessagePackObject]
    public sealed class SaveFileSlot
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

        [Key(0)] public int PositionX { get; set; } = 0;
        [Key(1)] public int PositionY { get; set; } = 0;
        [Key(2)] public SaveFileSlotLayer ForegroundLayer { get; set; }
        [Key(3)] public SaveFileSlotLayer BackgroundLayer { get; set; }

        public SaveFileSlot()
        {

        }

        public SaveFileSlot(SaveFileWorldResources resources, Slot worldSlot)
        {
            this.Position = worldSlot.Position;

            if (!worldSlot.ForegroundLayer.HasState(ElementStates.IsEmpty))
            {
                this.ForegroundLayer = new(resources, worldSlot.ForegroundLayer);
            }

            if (!worldSlot.BackgroundLayer.HasState(ElementStates.IsEmpty))
            {
                this.BackgroundLayer = new(resources, worldSlot.BackgroundLayer);
            }
        }
    }
}
