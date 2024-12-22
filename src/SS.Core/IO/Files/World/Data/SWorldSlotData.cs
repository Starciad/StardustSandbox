using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.Core.IO.Files.World.Data
{
    [MessagePackObject]
    public sealed class SWorldSlotData
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
        [Key(2)] public SWorldSlotLayerData ForegroundLayer { get; set; }
        [Key(3)] public SWorldSlotLayerData BackgroundLayer { get; set; }

        public SWorldSlotData()
        {

        }

        public SWorldSlotData(ISWorldSlot worldSlot)
        {
            this.Position = worldSlot.Position;

            if (!worldSlot.ForegroundLayer.IsEmpty)
            {
                this.ForegroundLayer = new(worldSlot.ForegroundLayer);
            }

            if (!worldSlot.BackgroundLayer.IsEmpty)
            {
                this.BackgroundLayer = new(worldSlot.BackgroundLayer);
            }
        }
    }
}
