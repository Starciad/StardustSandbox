using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class Steam : Gas
    {
        protected override void OnBeforeStep(in ElementContext context)
        {
            if (SSRandom.Chance(60))
            {
                return;
            }

            Point topPosition = new(context.Slot.Position.X, context.Slot.Position.Y - 1);

            if (context.IsEmptySlotLayer(topPosition, context.Layer))
            {
                context.SetPosition(topPosition);
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue < 35)
            {
                context.ReplaceElement(ElementIndex.Water);
            }
        }
    }
}
