using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.Elements.Solids.Movables
{
    internal sealed class SSalt : SMovableSolid
    {
        internal SSalt(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.White;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_29");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultTemperature = 22;
            this.defaultDensity = 2200;
            this.defaultExplosionResistance = 0.7f;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case SWater:
                    case SIce:
                    case SSnow:
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement(neighbor.Position, this.Context.Layer, SElementConstants.SALTWATER_IDENTIFIER);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 900)
            {
                this.Context.ReplaceElement(SElementConstants.LAVA_IDENTIFIER);
                this.Context.SetStoredElement(SElementConstants.SALT_IDENTIFIER);
            }
        }
    }
}
