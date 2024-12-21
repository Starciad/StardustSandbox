using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World.Data;

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
            this.ForegroundLayer = new(worldSlot.ForegroundLayer);
            this.ForegroundLayer = new(worldSlot.BackgroundLayer);
        }
    }

    [MessagePackObject]
    public sealed class SWorldSlotLayerData
    {
        [IgnoreMember]
        public Color ColorModifier
        {
            get => new(this.ColorModifierR, this.ColorModifierG, this.ColorModifierB, this.ColorModifierA);

            set
            {
                this.ColorModifierR = value.R;
                this.ColorModifierG = value.G;
                this.ColorModifierB = value.B;
                this.ColorModifierA = value.A;
            }
        }

        [Key(0)] public uint ElementId { get; set; }
        [Key(1)] public short Temperature { get; set; }
        [Key(2)] public bool FreeFalling { get; set; }
        [Key(3)] public byte ColorModifierR { get; set; }
        [Key(4)] public byte ColorModifierG { get; set; }
        [Key(5)] public byte ColorModifierB { get; set; }
        [Key(6)] public byte ColorModifierA { get; set; }

        public SWorldSlotLayerData()
        {

        }

        public SWorldSlotLayerData(ISWorldSlotLayer worldSlotLayer)
        {
            this.ElementId = worldSlotLayer.Element.Identifier;
            this.Temperature = worldSlotLayer.Temperature;
            this.FreeFalling = worldSlotLayer.FreeFalling;
            this.ColorModifier = worldSlotLayer.ColorModifier;
        }
    }
}
