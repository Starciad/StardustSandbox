using Microsoft.Xna.Framework;

using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class WetSponge : ImmovableSolid
    {
        protected override void OnStep()
        {
            foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(this.Context.Slot.Position, Direction.Down))
            {
                if (!this.Context.TryGetElement(belowPosition, this.Context.Layer, out Element element))
                {
                    return;
                }

                switch (element)
                {
                    case DrySponge:
                        this.Context.SwappingElements(this.Context.Position, belowPosition);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 60)
            {
                this.Context.ReplaceElement(ElementIndex.DrySponge);
            }
        }
    }
}
