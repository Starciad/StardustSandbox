using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Acid : Liquid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Acid:
                    case ElementIndex.Wall:
                    case ElementIndex.Clone:
                    case ElementIndex.Void:
                    case ElementIndex.DownwardPusher:
                    case ElementIndex.UpwardPusher:
                    case ElementIndex.LeftwardPusher:
                    case ElementIndex.RightwardPusher:
                        continue;

                    default:
                        break;
                }

                if (SSRandom.GetBool())
                {
                    context.DestroyElement(neighbors.GetSlot(i).Position, context.Layer);
                    context.DestroyElement();
                }
            }
        }
    }
}