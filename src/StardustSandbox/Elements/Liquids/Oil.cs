using StardustSandbox.Elements.Energies;
using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Oil : Liquid
    {
        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case Lava:
                    case Fire:
                        this.Context.ReplaceElement(ElementIndex.Fire);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 280)
            {
                this.Context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}
