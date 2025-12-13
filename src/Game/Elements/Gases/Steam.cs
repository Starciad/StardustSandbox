using StardustSandbox.Enums.Elements;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class Steam : Gas
    {
        protected override void OnStep(in ElementContext context)
        {
            if (SSRandom.Chance(40))
            {
                context.UpdateElementPosition(new(context.Slot.Position.X, context.Slot.Position.Y - 1));
            }
            else
            {
                base.OnStep(context);
            }
        }

        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            if (context.Position.Y <= PercentageMath.PercentageOfValue(context.GetWorldSize().Y, 15.0f) &&
                neighbors.CountNeighborsByElementIndex(ElementIndex.Steam, context.Layer) >= 5 &&
                SSRandom.Chance(10))
            {
                context.ReplaceElement(ElementIndex.Cloud);
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, in float currentValue)
        {
            if (currentValue < 35.0f)
            {
                context.ReplaceElement(ElementIndex.Water);
            }
        }
    }
}
