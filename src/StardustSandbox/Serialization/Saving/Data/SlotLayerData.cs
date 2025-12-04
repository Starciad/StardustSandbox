using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class SlotLayerData
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

        [Key("ElementIndex")]
        public ElementIndex ElementIndex { get; set; }

        [Key("Temperature")]
        public double Temperature { get; set; }

        [Key("States")]
        public ElementStates States { get; set; }

        [Key("ColorModifierR")]
        public byte ColorModifierR { get; set; }

        [Key("ColorModifierG")]
        public byte ColorModifierG { get; set; }

        [Key("ColorModifierB")]
        public byte ColorModifierB { get; set; }

        [Key("ColorModifierA")]
        public byte ColorModifierA { get; set; }

        [Key("StepCycleFlag")]
        public UpdateCycleFlag StepCycleFlag { get; set; }

        [Key("StoredElementIndex")]
        public ElementIndex StoredElementIndex { get; set; }

        public SlotLayerData()
        {

        }

        public SlotLayerData(World.SlotLayer worldSlotLayer)
        {
            this.ElementIndex = worldSlotLayer.Element.Index;
            this.Temperature = worldSlotLayer.Temperature;
            this.States = worldSlotLayer.States;
            this.ColorModifier = worldSlotLayer.ColorModifier;
            this.StepCycleFlag = worldSlotLayer.StepCycleFlag;

            if (worldSlotLayer.StoredElement != null)
            {
                this.StoredElementIndex = worldSlotLayer.StoredElement.Index;
            }
        }
    }
}
