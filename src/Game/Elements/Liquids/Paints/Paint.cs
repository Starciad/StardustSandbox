using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Liquids.Paints
{
    internal abstract class Paint : Liquid
    {
        internal Color DyeingColor { get; init; }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer) ||
                    neighbors.GetSlotLayer(i, context.Layer).ElementIndex == this.Index)
                {
                    continue;
                }

                context.SetElementColorModifier(neighbors.GetSlot(i).Position, this.DyeingColor);
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 200.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}
