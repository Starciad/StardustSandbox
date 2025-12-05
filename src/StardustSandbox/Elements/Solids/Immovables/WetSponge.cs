using Microsoft.Xna.Framework;

using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class WetSponge : ImmovableSolid
    {
        protected override void OnStep(in ElementContext context)
        {
            foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(context.Slot.Position, Direction.Down))
            {
                if (!context.TryGetElement(belowPosition, context.Layer, out Element element))
                {
                    return;
                }

                switch (element)
                {
                    case DrySponge:
                        context.SwappingElements(context.Position, belowPosition);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue >= 60)
            {
                context.ReplaceElement(ElementIndex.DrySponge);
            }
        }
    }
}
