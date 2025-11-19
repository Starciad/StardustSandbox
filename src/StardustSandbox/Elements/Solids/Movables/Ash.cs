using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Liquids;
using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ash : MovableSolid
    {
        internal Ash(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultTemperature = 40;
            this.defaultDensity = 350;
            this.defaultExplosionResistance = 0f;
        }

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case Water:
                    case Saltwater:
                    case Lava:
                        this.Context.DestroyElement();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
