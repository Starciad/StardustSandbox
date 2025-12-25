using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class WetSponge : ImmovableSolid
    {
        protected override void OnStep(ElementContext context)
        {
            Point belowPosition = new(context.Slot.Position.X, context.Slot.Position.Y + 1);

            if (context.TryGetElement(belowPosition, context.Layer, out ElementIndex index))
            {
                switch (index)
                {
                    case ElementIndex.DrySponge:
                        context.SwappingElements(context.Position, belowPosition);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 60.0f)
            {
                context.ReplaceElement(ElementIndex.DrySponge);
            }
        }
    }
}
