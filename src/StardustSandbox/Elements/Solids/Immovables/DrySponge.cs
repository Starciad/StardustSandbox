using Microsoft.Xna.Framework;

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

        protected override void OnTemperatureChanged(double currentValue)
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
