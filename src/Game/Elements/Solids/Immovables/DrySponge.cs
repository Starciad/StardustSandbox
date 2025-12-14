using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class DrySponge : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            bool shouldBecomeWet = false;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Water:
                    case ElementIndex.Saltwater:
                        context.RemoveElement(neighbors.GetSlot(i).Position);
                        shouldBecomeWet = true;
                        break;

                    default:
                        break;
                }
            }

            if (shouldBecomeWet)
            {
                context.ReplaceElement(ElementIndex.WetSponge);
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, in float currentValue)
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
