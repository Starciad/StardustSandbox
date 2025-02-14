using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    internal sealed class SOil : SLiquid
    {
        internal SOil(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = Color.Black;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_28");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableFlammability = true;
            this.enableNeighborsAction = true;
            this.defaultFlammabilityResistance = 5;
            this.defaultDensity = 980;
            this.defaultExplosionResistance = 0.3f;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case SLava:
                    case SFire:
                        this.Context.ReplaceElement(SElementConstants.FIRE_IDENTIFIER);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 280)
            {
                this.Context.ReplaceElement(SElementConstants.FIRE_IDENTIFIER);
            }
        }
    }
}
