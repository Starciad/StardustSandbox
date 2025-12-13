using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Void : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i) || neighbors.GetSlotLayer(i, context.Layer).HasState(ElementStates.IsEmpty))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
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
