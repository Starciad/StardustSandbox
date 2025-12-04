using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Acid : Liquid
    {
        internal Acid() : base()
        {

        }

        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer slotLayer = neighbor.GetLayer(context.Layer);

                if (slotLayer.HasState(ElementStates.IsEmpty))
                {
                    continue;
                }

                switch (slotLayer.Element)
                {
                    case Acid:
                    case Wall:
                    case Clone:
                    case Void:
                        continue;

                    default:
                        break;
                }

                context.DestroyElement();
                context.DestroyElement(neighbor.Position, context.Layer);
            }
        }
    }
}