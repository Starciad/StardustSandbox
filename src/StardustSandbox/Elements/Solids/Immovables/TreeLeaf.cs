using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class TreeLeaf : ImmovableSolid
    {
        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 220)
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
