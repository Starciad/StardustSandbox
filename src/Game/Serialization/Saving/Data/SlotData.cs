using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class SlotData
    {
        [Key("PositionX")]
        public int PositionX { get; init; }

        [Key("PositionY")]
        public int PositionY { get; init; }

        [Key("ForegroundLayer")]
        public SlotLayerData ForegroundLayer { get; init; }

        [Key("BackgroundLayer")]
        public SlotLayerData BackgroundLayer { get; init; }

        [IgnoreMember]
        public Point Position
        {
            get => new(this.PositionX, this.PositionY);

            init
            {
                this.PositionX = value.X;
                this.PositionY = value.Y;
            }
        }

        public SlotData()
        {

        }

        public SlotData(Slot slot)
        {
            this.Position = slot.Position;

            if (!slot.Foreground.HasState(ElementStates.IsEmpty))
            {
                this.ForegroundLayer = new(slot.Foreground);
            }

            if (!slot.Background.HasState(ElementStates.IsEmpty))
            {
                this.BackgroundLayer = new(slot.Background);
            }
        }
    }
}
