using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Rendering;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Saltwater : Liquid
    {
        internal Saltwater(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultDispersionRate = 3;
            this.defaultTemperature = 25;
            this.defaultDensity = 1200;
            this.defaultExplosionResistance = 0.2f;
        }

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

        protected override void OnTemperatureChanged(short currentValue)
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
