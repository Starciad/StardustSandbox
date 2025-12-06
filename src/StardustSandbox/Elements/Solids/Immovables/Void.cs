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
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                SlotLayer neighborSlotLayer = neighbors.GetSlotLayer(i, context.Layer);

                if (!neighborSlotLayer.HasState(ElementStates.IsEmpty) &&
                     neighborSlotLayer.Element.Index != ElementIndex.Wall &&
                     neighborSlotLayer.Element.Index != ElementIndex.Void &&
                     neighborSlotLayer.Element.Index != ElementIndex.Clone)
                {
                    context.DestroyElement(neighbors.GetSlot(i).Position, context.Layer);
                }
            }
        }
    }
}
