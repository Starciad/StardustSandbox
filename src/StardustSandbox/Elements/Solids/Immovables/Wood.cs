using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Wood : ImmovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, double currentValue)
        {
            if (currentValue >= 300)
            {
                if (SSRandom.Chance(65))
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
