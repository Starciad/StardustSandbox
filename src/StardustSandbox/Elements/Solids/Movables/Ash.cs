using StardustSandbox.Elements.Liquids;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ash : MovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, IEnumerable<Slot> neighbors)
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
