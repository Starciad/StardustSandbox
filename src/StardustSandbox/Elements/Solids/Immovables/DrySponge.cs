using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Liquids;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class DrySponge : ImmovableSolid
    {
        internal DrySponge(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 25;
            this.defaultFlammabilityResistance = 10;
            this.defaultDensity = 550;
            this.defaultExplosionResistance = 0.5f;
        }

        private void AbsorbWaterAround()
        {
            foreach (Point position in ShapePointGenerator.GenerateSquarePoints(this.Context.Slot.Position, 1))
            {
                if (!this.Context.TryGetSlot(position, out Slot worldSlot))
                {
                    continue;
                }

                SlotLayer worldSlotLayer = worldSlot.GetLayer(this.Context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case Water:
                    case Saltwater:
                        this.Context.DestroyElement(position, this.Context.Layer);
                        break;

                    default:
                        break;
                }
            }

            this.Context.ReplaceElement(ElementIndex.WetSponge);
        }

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer worldSlotLayer = neighbor.GetLayer(this.Context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case Water:
                    case Saltwater:
                        AbsorbWaterAround();
                        return;

                    default:
                        continue;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 180)
            {
                if (SSRandom.Chance(70))
                {
                    this.Context.ReplaceElement(ElementIndex.Fire);
                }
                else
                {
                    this.Context.ReplaceElement(ElementIndex.Ash);
                }
            }
        }
    }
}
