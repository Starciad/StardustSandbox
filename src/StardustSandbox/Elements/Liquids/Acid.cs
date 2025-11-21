using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Acid : Liquid
    {
        internal Acid(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 10;
            this.defaultDensity = 1100;
            this.defaultExplosionResistance = 0.2f;
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