using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Generators;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Sapling : MovableSolid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            bool hasWater = false, hasFertileSoil = false;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                if (neighbors.GetSlotLayer(i, context.Layer).ElementIndex is ElementIndex.Water)
                {
                    hasWater = true;
                    context.DestroyElement(neighbors.GetSlot(i).Position);
                }

                if (i == (int)ElementNeighborDirection.South && neighbors.GetSlotLayer(i, context.Layer).ElementIndex is ElementIndex.FertileSoil)
                {
                    hasFertileSoil = true;
                }

                if (hasWater && hasFertileSoil)
                {
                    break;
                }
            }

            if (hasWater && hasFertileSoil && SSRandom.Chance(25, 350))
            {
                context.DestroyElement();
                TreeGenerator.Start(context.World, context.Position, SSRandom.Range(5, 8), 1, 2);
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 100.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}
