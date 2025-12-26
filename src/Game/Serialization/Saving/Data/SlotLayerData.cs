using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class SlotLayerData
    {
        [Key("ElementIndex")]
        public ElementIndex ElementIndex { get; init; }

        [Key("StoredElementIndex")]
        public ElementIndex StoredElementIndex { get; init; }

        [Key("Temperature")]
        public float Temperature { get; init; }

        [Key("States")]
        public ElementStates States { get; init; }

        [Key("ColorModifierR")]
        public byte ColorModifierR { get; init; }

        [Key("ColorModifierG")]
        public byte ColorModifierG { get; init; }

        [Key("ColorModifierB")]
        public byte ColorModifierB { get; init; }

        [Key("ColorModifierA")]
        public byte ColorModifierA { get; init; }

        [Key("StepCycleFlag")]
        public UpdateCycleFlag StepCycleFlag { get; init; }

        [IgnoreMember]
        public Color ColorModifier
        {
            get => new(this.ColorModifierR, this.ColorModifierG, this.ColorModifierB, this.ColorModifierA);

            init
            {
                this.ColorModifierR = value.R;
                this.ColorModifierG = value.G;
                this.ColorModifierB = value.B;
                this.ColorModifierA = value.A;
            }
        }

        public SlotLayerData()
        {

        }

        public SlotLayerData(SlotLayer slotLayer)
        {
            this.ElementIndex = slotLayer.ElementIndex;
            this.Temperature = slotLayer.Temperature;
            this.States = slotLayer.States;
            this.ColorModifier = slotLayer.ColorModifier;
            this.StepCycleFlag = slotLayer.StepCycleFlag;

            if (slotLayer.StoredElementIndex is not ElementIndex.None)
            {
                this.StoredElementIndex = slotLayer.StoredElementIndex;
            }
        }
    }
}
