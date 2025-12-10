using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Lava : Liquid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element)
                {
                    case Oil:
                    case Wood:
                    case TreeLeaf:
                    case DrySponge:
                    case Grass:
                        context.ReplaceElement(neighbors.GetSlot(i).Position, context.Layer, ElementIndex.Fire);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue <= 500.0f)
            {
                if (context.SlotLayer.StoredElement == null)
                {
                    context.ReplaceElement(ElementIndex.Stone);
                }
                else
                {
                    context.ReplaceElement(context.SlotLayer.StoredElement);
                }

                context.SetElementTemperature(500.0f);
            }
        }
    }
}