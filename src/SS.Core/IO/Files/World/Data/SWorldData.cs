using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.IO.Files.World.Data
{
    [MessagePackObject]
    public sealed class SWorldData
    {
        [IgnoreMember] public SSize2 Size => new(this.Width, this.Height);
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
            get => new(this.ColorR, this.ColorG, this.ColorB, this.ColorA);

            set
            {
                this.ColorR = value.R;
                this.ColorG = value.G;
                this.ColorB = value.B;
                this.ColorA = value.A;
            }
        }

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
        [Key(2)] public uint ElementId { get; set; }
        [Key(3)] public short Temperature { get; set; }
        [Key(4)] public bool FreeFalling { get; set; }
        [Key(5)] public byte ColorR { get; set; }
        [Key(6)] public byte ColorG { get; set; }
        [Key(7)] public byte ColorB { get; set; }
        [Key(8)] public byte ColorA { get; set; }

        public SWorldSlotData()
        {

        }

        public SWorldSlotData(ISWorldSlot worldSlot, Point position)
        {
            this.Position = position;
            this.ElementId = worldSlot.Element.Identifier;
            this.Temperature = worldSlot.Temperature;
            this.FreeFalling = worldSlot.FreeFalling;
            this.Color = worldSlot.Color;
        }
    }
}
