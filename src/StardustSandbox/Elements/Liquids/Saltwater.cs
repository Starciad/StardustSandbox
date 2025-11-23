using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Saltwater : Liquid
    {
        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case Dirt:
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement(neighbor.Position, this.Context.Layer, ElementIndex.Mud);
                        break;

                    case Stone:
                        if (SSRandom.Range(0, 150) == 0)
                        {
                            this.Context.DestroyElement();
                            this.Context.ReplaceElement(neighbor.Position, this.Context.Layer, ElementIndex.Sand);
                        }

                        break;

                    case Fire:
                        this.Context.DestroyElement(neighbor.Position, this.Context.Layer);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue <= 21)
            {
                this.Context.ReplaceElement(ElementIndex.Ice);
                this.Context.SetStoredElement(ElementIndex.Saltwater);
                return;
            }

            if (currentValue >= 110)
            {
                if (SSRandom.Chance(50))
                {
                    this.Context.ReplaceElement(ElementIndex.Steam);
                }
                else
                {
                    this.Context.ReplaceElement(ElementIndex.Saltwater);
                }

                return;
            }
        }
    }
}
