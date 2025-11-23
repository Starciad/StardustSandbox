using StardustSandbox.Elements.Liquids;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ash : MovableSolid
    {
        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case Water:
                    case Saltwater:
                    case Lava:
                        this.Context.DestroyElement();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
