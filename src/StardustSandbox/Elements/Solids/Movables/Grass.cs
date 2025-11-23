using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Grass : MovableSolid
    {
        internal Grass() : base()
        {

        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue > 200)
            {
                if (SSRandom.Chance(85))
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
