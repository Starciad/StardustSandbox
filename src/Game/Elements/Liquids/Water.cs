using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Water : Liquid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
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
                            return;
                        }

                        break;

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
            if (currentValue <= 0.0f)
            {
                context.ReplaceElement(ElementIndex.Ice);
            }
            else if (currentValue >= 100.0f)
            {
                context.ReplaceElement(ElementIndex.Steam);
            }
        }
    }
}