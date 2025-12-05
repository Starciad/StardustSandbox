using StardustSandbox.Elements.Liquids;
using StardustSandbox.Enums.Elements;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Salt : MovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(context.Layer).Element)
                {
                    case Water:
                    case Ice:
                    case Snow:
                        context.DestroyElement();
                        context.ReplaceElement(neighbor.Position, context.Layer, ElementIndex.Saltwater);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue > 900)
            {
                context.ReplaceElement(ElementIndex.Lava);
                context.SetStoredElement(ElementIndex.Salt);
            }
        }
    }
}
