using Microsoft.Xna.Framework;

using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class Smoke : Gas
    {
        protected override void OnBeforeStep()
        {
            if (SSRandom.Chance(75))
            {
                return;
            }

            Point topPosition = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y - 1);

            if (this.Context.IsEmptySlotLayer(topPosition, this.Context.Layer))
            {
                this.Context.SetPosition(topPosition);
            }
        }
    }
}
