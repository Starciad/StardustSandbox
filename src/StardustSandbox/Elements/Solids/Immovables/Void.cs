using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Void : ImmovableSolid
    {
        internal Void(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.enableTemperature = false;
            this.enableNeighborsAction = true;
            this.isExplosionImmune = true;
            this.defaultDensity = 220;
        }

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer neighborLayer = neighbor.GetLayer(this.Context.Layer);

                if (!neighborLayer.IsEmpty && neighborLayer.Element is not (Wall or Void or Clone))
                {
                    this.Context.DestroyElement(neighbor.Position, this.Context.Layer);
                }
            }
        }
    }
}
