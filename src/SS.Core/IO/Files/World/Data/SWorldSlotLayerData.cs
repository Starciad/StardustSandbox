using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.IO.Files.World.Data
{
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

        [Key(0)] public string ElementIdentifier { get; set; }
        [Key(1)] public short Temperature { get; set; }
        [Key(2)] public bool FreeFalling { get; set; }
        [Key(3)] public byte ColorModifierR { get; set; }
        [Key(4)] public byte ColorModifierG { get; set; }
        [Key(5)] public byte ColorModifierB { get; set; }
        [Key(6)] public byte ColorModifierA { get; set; }
        [Key(7)] public SUpdateCycleFlag UpdateCycleFlag { get; set; }
        [Key(8)] public SUpdateCycleFlag StepCycleFlag { get; set; }

        public SWorldSlotLayerData()
        {

        }

        public SWorldSlotLayerData(SWorldSlotLayer worldSlotLayer)
        {
            this.ElementIdentifier = worldSlotLayer.Element.Identifier;
            this.Temperature = worldSlotLayer.Temperature;
            this.FreeFalling = worldSlotLayer.FreeFalling;
            this.ColorModifier = worldSlotLayer.ColorModifier;
            this.UpdateCycleFlag = worldSlotLayer.UpdateCycleFlag;
            this.StepCycleFlag = worldSlotLayer.StepCycleFlag;
        }
    }
}
