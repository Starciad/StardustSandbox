using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Seed : MovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            bool hasWater = false, hasFertileSoil = false;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Water:
                        hasWater = true;
                        context.DestroyElement(neighbors.GetSlot(i).Position);
                        break;

                    case ElementIndex.FertileSoil:
                        hasFertileSoil = true;
                        break;

                    default:
                        break;
                }
            }

            if (hasWater && hasFertileSoil && SSRandom.Chance(25, 500))
            {
                context.ReplaceElement(ElementIndex.Sapling);
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, in float currentValue)
        {
            if (currentValue >= 75.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}
