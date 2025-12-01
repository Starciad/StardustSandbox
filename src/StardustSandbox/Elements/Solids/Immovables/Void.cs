using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Void : ImmovableSolid
    {
        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer neighborLayer = neighbor.GetLayer(context.Layer);

                if (!neighborLayer.HasState(ElementStates.IsEmpty) && neighborLayer.Element.Index != ElementIndex.Wall && neighborLayer.Element.Index != ElementIndex.Void && neighborLayer.Element.Index != ElementIndex.Clone)
                {
                    context.DestroyElement(neighbor.Position, context.Layer);
                }
            }
        }
    }
}
