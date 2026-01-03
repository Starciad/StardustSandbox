using StardustSandbox.Enums.Elements;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class Cloud : Gas
    {
        protected override void OnBeforeStep(ElementContext context)
        {
            if (SSRandom.Chance(35))
            {
                context.UpdateElementPosition(new(context.Slot.Position.X, context.Slot.Position.Y - 1));
            }
        }

        protected override void OnStep(ElementContext context)
        {
            if (SSRandom.Chance(15))
            {
                base.OnStep(context);
            }
            else
            {
                context.NotifyChunk();
            }
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            if (context.Position.Y <= PercentageMath.PercentageOfValue(context.World.Information.Size.Y, 15.0f) &&
                neighbors.CountNeighborsByElementIndex(ElementIndex.Cloud, context.Layer) >= 4 &&
                SSRandom.Chance(5))
            {
                context.ReplaceElement(ElementIndex.ChargedCloud);
            }
        }
    }
}
