using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        internal Water(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible;

            this.defaultDispersionRate = 3;
            this.defaultTemperature = 25;
            this.defaultDensity = 1000;
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
            if (currentValue <= 0)
            {
                this.Context.ReplaceElement(ElementIndex.Ice);
                return;
            }

            if (currentValue >= 100)
            {
                this.Context.ReplaceElement(ElementIndex.Steam);
                return;
            }
        }
    }
}