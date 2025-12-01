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
        private static void AbsorbWaterAround(ElementContext context)
        {
            foreach (Point position in ShapePointGenerator.GenerateSquarePoints(context.Slot.Position, 1))
            {
                if (!context.TryGetSlot(position, out Slot worldSlot))
                {
                    continue;
                }

                SlotLayer worldSlotLayer = worldSlot.GetLayer(context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case Water:
                    case Saltwater:
                        context.DestroyElement(position, context.Layer);
                        break;

                    default:
                        break;
                }
            }

            context.ReplaceElement(ElementIndex.WetSponge);
        }

        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer worldSlotLayer = neighbor.GetLayer(context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case Water:
                    case Saltwater:
                        AbsorbWaterAround(context);
                        return;

                    default:
                        continue;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, double currentValue)
        {
            if (currentValue >= 180)
            {
                if (SSRandom.Chance(70))
                {
                    context.ReplaceElement(ElementIndex.Fire);
                }
                else
                {
                    context.ReplaceElement(ElementIndex.Ash);
                }
            }
        }
    }
}
