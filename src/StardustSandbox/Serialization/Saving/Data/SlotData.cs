using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class SlotData
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

        [Key("PositionX")]
        public int PositionX { get; set; }

        [Key("PositionY")]
        public int PositionY { get; set; }

        [Key("ForegroundLayer")]
        public SlotLayerData ForegroundLayer { get; set; }

        [Key("BackgroundLayer")]
        public SlotLayerData BackgroundLayer { get; set; }

        public SlotData()
        {

        }

        public SlotData(World.Slot slot)
        {
            this.Position = slot.Position;

            if (!slot.ForegroundLayer.HasState(ElementStates.IsEmpty))
            {
                this.ForegroundLayer = new(slot.ForegroundLayer);
            }

            if (!slot.BackgroundLayer.HasState(ElementStates.IsEmpty))
            {
                this.BackgroundLayer = new(slot.BackgroundLayer);
            }
        }
    }
}
