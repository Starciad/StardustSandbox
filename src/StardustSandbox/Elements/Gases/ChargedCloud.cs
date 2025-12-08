using StardustSandbox.Enums.Elements;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class ChargedCloud : Gas
    {
        protected override void OnBeforeStep(in ElementContext context)
        {
            if (SSRandom.Chance(35))
            {
                context.UpdateElementPosition(new(context.Slot.Position.X, context.Slot.Position.Y - 1));
            }
        }

        protected override void OnStep(in ElementContext context)
        {
            if (SSRandom.Chance(10))
            {
                base.OnStep(context);
            }
            else
            {
                context.NotifyChunk();
            }
        }

        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            if (context.Position.Y <= PercentageMath.PercentageOfValue(context.GetWorldSize().Y, 10.0f) &&
                neighbors.CountNeighborsByElementIndex(ElementIndex.ChargedCloud, context.Layer) >= 5 &&
                SSRandom.Chance(1))
            {
                if (context.SlotLayer.Temperature < 0.0f)
                {
                    if (SSRandom.Chance(65))
                    {
                        context.ReplaceElement(ElementIndex.Snow);
                        context.SetElementTemperature(-55.0f);
                    }
                    else
                    {
                        context.ReplaceElement(ElementIndex.ThunderHead);
                    }
                }
                else
                {
                    context.ReplaceElement(ElementIndex.Water);
                    context.SetElementTemperature(2.5f);
                }
            }
        }
    }
}
