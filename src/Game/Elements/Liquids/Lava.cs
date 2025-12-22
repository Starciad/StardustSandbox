using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Lava : Liquid
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
                    case ElementIndex.Oil:
                    case ElementIndex.Wood:
                    case ElementIndex.TreeLeaf:
                    case ElementIndex.DrySponge:
                    case ElementIndex.Grass:
                    case ElementIndex.DryBlackWool:
                    case ElementIndex.DryWhiteWool:
                    case ElementIndex.DryRedWool:
                    case ElementIndex.DryOrangeWool:
                    case ElementIndex.DryYellowWool:
                    case ElementIndex.DryGreenWool:
                    case ElementIndex.DryGrayWool:
                    case ElementIndex.DryBlueWool:
                    case ElementIndex.DryVioletWool:
                    case ElementIndex.DryBrownWool:
                        context.ReplaceElement(neighbors.GetSlot(i).Position, context.Layer, ElementIndex.Fire);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue <= 500.0f)
            {
                if (context.GetStoredElement() == null)
                {
                    context.ReplaceElement(ElementIndex.Stone);
                }
                else
                {
                    context.ReplaceElement(context.GetStoredElement());
                }

                context.SetElementTemperature(500.0f);
            }
        }
    }
}