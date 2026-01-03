using StardustSandbox.Enums.Elements;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class ChargedCloud : Gas
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
            if (SSRandom.Chance(10))
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
            if (context.Position.Y <= PercentageMath.PercentageOfValue(context.World.Information.Size.Y, 10.0f) &&
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
                        context.ReplaceElement(ElementIndex.LightningHead);
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
