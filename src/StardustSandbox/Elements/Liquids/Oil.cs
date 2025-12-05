using StardustSandbox.Elements.Energies;
using StardustSandbox.Enums.Elements;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Oil : Liquid
    {
        protected override void OnNeighbors(in ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(context.Layer).Element)
                {
                    case Lava:
                    case Fire:
                        context.ReplaceElement(ElementIndex.Fire);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue >= 280)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}
