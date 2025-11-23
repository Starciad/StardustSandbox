using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class Steam : Gas
    {
        protected override void OnBeforeStep()
        {
            if (SSRandom.Chance(60))
            {
                return;
            }

            Point topPosition = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y - 1);

            if (this.Context.IsEmptySlotLayer(topPosition, this.Context.Layer))
            {
                this.Context.SetPosition(topPosition);
            }
        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue < 35)
            {
                this.Context.ReplaceElement(ElementIndex.Water);
            }
        }
    }
}
