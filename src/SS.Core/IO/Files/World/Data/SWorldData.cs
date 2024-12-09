using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.Core.IO.Files.World.Data
{
    [MessagePackObject]
    public sealed class SWorldData
    {
        [Key(0)] public int Width { get; set; }
        [Key(1)] public int Height { get; set; }
        [Key(2)] public SWorldSlotData[] Slots { get; set; }
    }

    [MessagePackObject]
    public sealed class SWorldSlotData
    {
        [IgnoreMember]
        public Color Color
        {
            get
            {
                return new(this.ColorR, this.ColorG, this.ColorB, this.ColorA);
            }

            set
            {
                this.ColorR = value.R;
                this.ColorG = value.G;
                this.ColorB = value.B;
                this.ColorA = value.A;
            }
        }

        [Key(0)] public uint ElementId { get; set; }
        [Key(1)] public bool IsEmpty { get; set; }
        [Key(2)] public short Temperature { get; set; }
        [Key(3)] public bool FreeFalling { get; set; }
        [Key(4)] public byte ColorR { get; set; }
        [Key(5)] public byte ColorG { get; set; }
        [Key(6)] public byte ColorB { get; set; }
        [Key(7)] public byte ColorA { get; set; }

        public SWorldSlotData()
        {

        }

        public SWorldSlotData(ISWorldSlot worldSlot)
        {
            this.ElementId = worldSlot.Element.Id;
            this.IsEmpty = worldSlot.IsEmpty;
            this.Temperature = worldSlot.Temperature;
            this.FreeFalling = worldSlot.FreeFalling;
            this.Color = worldSlot.Color;
        }
    }
}
