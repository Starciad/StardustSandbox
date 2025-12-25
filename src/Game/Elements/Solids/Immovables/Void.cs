using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Void : ImmovableSolid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).ElementIndex)
                {
                    case ElementIndex.Wall:
                    case ElementIndex.Void:
                    case ElementIndex.Clone:
                        continue;

                    default:
                        break;
                }

                context.DestroyElement(neighbors.GetSlot(i).Position, context.Layer);
            }
        }
    }
}
