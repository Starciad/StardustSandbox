using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Grass : MovableSolid
    {
        internal Grass() : base()
        {

        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue > 200)
            {
                if (SSRandom.Chance(85))
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
