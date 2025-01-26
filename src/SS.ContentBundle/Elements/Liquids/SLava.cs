using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    internal sealed class SLava : SLiquid
    {
        internal SLava(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Orange;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_10");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableLightEmission = true;
            this.defaultLuminousIntensity = 5;
            this.defaultTemperature = 1000;
            this.defaultDensity = 3000;
            this.defaultExplosionResistance = 0.4f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue <= 500)
            {
                if (this.Context.SlotLayer.StoredElement == null)
                {
                    this.Context.ReplaceElement(SElementConstants.STONE_IDENTIFIER);
                }
                else
                {
                    this.Context.ReplaceElement(this.Context.SlotLayer.StoredElement);
                }

                this.Context.SetElementTemperature(500);
            }
        }
    }
}