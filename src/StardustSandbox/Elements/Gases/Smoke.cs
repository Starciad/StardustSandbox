using Microsoft.Xna.Framework;

using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class Smoke : Gas
    {
        protected override void OnBeforeStep(in ElementContext context)
        {
            if (SSRandom.Chance(75))
            {
                return;
            }

            Point topPosition = new(context.Slot.Position.X, context.Slot.Position.Y - 1);

            if (context.IsEmptySlotLayer(topPosition, context.Layer))
            {
                context.SetPosition(topPosition);
            }
        }
    }
}
