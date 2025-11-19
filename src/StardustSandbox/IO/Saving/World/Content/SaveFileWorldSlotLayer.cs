using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.IO.Saving.World.Information;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.IO.Saving.World.Content
{
    [MessagePackObject]
    public sealed class SaveFileSlotLayer
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

        [Key(0)] public uint ElementIndex { get; set; }
        [Key(1)] public short Temperature { get; set; }
        [Key(2)] public bool FreeFalling { get; set; }
        [Key(3)] public byte ColorModifierR { get; set; }
        [Key(4)] public byte ColorModifierG { get; set; }
        [Key(5)] public byte ColorModifierB { get; set; }
        [Key(6)] public byte ColorModifierA { get; set; }
        [Key(7)] public UpdateCycleFlag UpdateCycleFlag { get; set; }
        [Key(8)] public UpdateCycleFlag StepCycleFlag { get; set; }
        [Key(9)] public uint StoredElementIndex { get; set; }

        public SaveFileSlotLayer()
        {

        }

        public SaveFileSlotLayer(SaveFileWorldResources resources, SlotLayer worldSlotLayer)
        {
            this.ElementIndex = resources.Elements.FindIndexByValue(worldSlotLayer.Element.Index);
            this.Temperature = worldSlotLayer.Temperature;
            this.FreeFalling = worldSlotLayer.FreeFalling;
            this.ColorModifier = worldSlotLayer.ColorModifier;
            this.UpdateCycleFlag = worldSlotLayer.UpdateCycleFlag;
            this.StepCycleFlag = worldSlotLayer.StepCycleFlag;

            if (worldSlotLayer.StoredElement != null)
            {
                this.StoredElementIndex = resources.Elements.FindIndexByValue(worldSlotLayer.StoredElement.Index);
            }
        }
    }
}
