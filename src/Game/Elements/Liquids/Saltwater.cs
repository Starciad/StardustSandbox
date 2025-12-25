using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Saltwater : Liquid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).ElementIndex)
                {
                    case ElementIndex.FertileSoil:
                    case ElementIndex.Dirt:
                        context.ReplaceElement(neighbors.GetSlot(i).Position, context.Layer, ElementIndex.Mud);
                        context.DestroyElement();
                        return;

                    case ElementIndex.Stone:
                        if (SSRandom.Range(0, 150) == 0)
                        {
                            context.ReplaceElement(neighbors.GetSlot(i).Position, context.Layer, ElementIndex.Sand);
                            context.DestroyElement();
                        }

                        return;

                    case ElementIndex.Fire:
                        context.DestroyElement(neighbors.GetSlot(i).Position, context.Layer);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue <= 21.0f)
            {
                context.ReplaceElement(ElementIndex.Ice);
                context.SetStoredElement(ElementIndex.Saltwater);
            }
            else if (currentValue >= 110.0f)
            {
                if (SSRandom.GetBool())
                {
                    context.ReplaceElement(ElementIndex.Steam);
                }
                else
                {
                    context.ReplaceElement(ElementIndex.Saltwater);
                }
            }
        }
    }
}
