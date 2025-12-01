using StardustSandbox.Elements.Liquids;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ash : MovableSolid
    {
        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(context.Layer).Element)
                {
                    case Water:
                    case Saltwater:
                    case Lava:
                        context.DestroyElement();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
