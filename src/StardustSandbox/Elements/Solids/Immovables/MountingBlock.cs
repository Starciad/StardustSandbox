using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class MountingBlock : ImmovableSolid
    {
        protected override void OnInstantiated()
        {
            this.Context.SetElementColorModifier(ElementConstants.COLORS_OF_MOUNTING_BLOCKS.GetRandomItem());
        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue > 300)
            {
                if (SSRandom.Chance(75))
                {
                    this.Context.ReplaceElement(ElementIndex.Fire);
                }
                else
                {
                    this.Context.ReplaceElement(ElementIndex.Ash);
                }
            }
        }
    }
}
