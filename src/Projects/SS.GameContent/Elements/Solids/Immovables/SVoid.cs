using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.Elements.Solids.Immovables
{
    internal sealed class SVoid : SImmovableSolid
    {
        internal SVoid(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.DarkGray;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_26");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableTemperature = false;
            this.enableNeighborsAction = true;
            this.isExplosionImmune = true;
            this.defaultDensity = 220;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                SWorldSlotLayer neighborLayer = neighbor.GetLayer(this.Context.Layer);

                if (!neighborLayer.IsEmpty && neighborLayer.Element is not (SWall or SVoid or SClone))
                {
                    this.Context.DestroyElement(neighbor.Position, this.Context.Layer);
                }
            }
        }
    }
}
