using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class TreeLeaf : ImmovableSolid
    {
        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue >= 220)
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
