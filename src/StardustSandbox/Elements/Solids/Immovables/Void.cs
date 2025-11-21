using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Void : ImmovableSolid
    {
        internal Void(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.IsExplosionImmune;

            this.defaultDensity = 220;
        }

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer neighborLayer = neighbor.GetLayer(this.Context.Layer);

                if (!neighborLayer.HasState(ElementStates.IsEmpty) && (neighborLayer.Element.Index != ElementIndex.Wall || neighborLayer.Element.Index != ElementIndex.Void || neighborLayer.Element.Index != ElementIndex.Clone))
                {
                    this.Context.DestroyElement(neighbor.Position, this.Context.Layer);
                }
            }
        }
    }
}
