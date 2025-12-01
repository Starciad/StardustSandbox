using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Water : Liquid
    {
        internal Water() : base()
        {

        }

        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(context.Layer).Element)
                {
                    case Dirt:
                        context.DestroyElement();
                        context.ReplaceElement(neighbor.Position, context.Layer, ElementIndex.Mud);
                        break;

                    case Stone:
                        if (SSRandom.Range(0, 150) == 0)
                        {
                            context.DestroyElement();
                            context.ReplaceElement(neighbor.Position, context.Layer, ElementIndex.Sand);
                        }

                        break;

                    case Fire:
                        context.DestroyElement(neighbor.Position, context.Layer);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, double currentValue)
        {
            if (currentValue <= 0)
            {
                context.ReplaceElement(ElementIndex.Ice);
                return;
            }

            if (currentValue >= 100)
            {
                context.ReplaceElement(ElementIndex.Steam);
                return;
            }
        }
    }
}