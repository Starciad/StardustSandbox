using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class MountingBlock : ImmovableSolid
    {
        protected override void OnInstantiated(in ElementContext context)
        {
            context.SetElementColorModifier(ElementConstants.COLORS_OF_MOUNTING_BLOCKS.GetRandomItem());
        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue > 300)
            {
                if (SSRandom.Chance(75))
                {
                    context.ReplaceElement(ElementIndex.Fire);
                }
                else
                {
                    context.ReplaceElement(ElementIndex.Ash);
                }
            }
        }
    }
}
