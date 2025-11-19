using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Acid : Liquid
    {
        internal Acid(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.defaultTemperature = 10;
            this.enableNeighborsAction = true;
            this.defaultDensity = 1100;
            this.defaultExplosionResistance = 0.2f;
        }

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer slotLayer = neighbor.GetLayer(this.Context.Layer);

                if (slotLayer.IsEmpty)
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