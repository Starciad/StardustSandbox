using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Acid : Liquid
    {
        internal Acid() : base()
        {

        }

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer slotLayer = neighbor.GetLayer(this.Context.Layer);

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

                this.Context.DestroyElement();
                this.Context.DestroyElement(neighbor.Position, this.Context.Layer);
            }
        }
    }
}