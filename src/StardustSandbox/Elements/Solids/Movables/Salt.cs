using StardustSandbox.Elements.Liquids;
using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Salt : MovableSolid
    {
        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case Water:
                    case Ice:
                    case Snow:
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement(neighbor.Position, this.Context.Layer, ElementIndex.Saltwater);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue > 900)
            {
                this.Context.ReplaceElement(ElementIndex.Lava);
                this.Context.SetStoredElement(ElementIndex.Salt);
            }
        }
    }
}
