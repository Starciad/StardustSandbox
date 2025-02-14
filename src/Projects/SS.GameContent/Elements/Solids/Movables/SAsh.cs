using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SAsh : SMovableSolid
    {
        internal SAsh(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.LightGrayBlue;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_39");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultTemperature = 40;
            this.defaultDensity = 350;
            this.defaultExplosionResistance = 0f;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case SWater:
                    case SSaltwater:
                    case SLava:
                        this.Context.DestroyElement();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
