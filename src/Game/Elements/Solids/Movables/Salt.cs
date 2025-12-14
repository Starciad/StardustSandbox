using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Salt : MovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Water:
                    case ElementIndex.Ice:
                    case ElementIndex.Snow:
                        context.DestroyElement();
                        context.ReplaceElement(neighbors.GetSlot(i).Position, context.Layer, ElementIndex.Saltwater);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, in float currentValue)
        {
            if (currentValue > 900.0f)
            {
                context.ReplaceElement(ElementIndex.Lava);
                context.SetStoredElement(ElementIndex.Salt);
            }
        }
    }
}
