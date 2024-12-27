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
            this.defaultTemperature = 1000;
            this.defaultDensity = 3000;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue <= 500)
            {
                this.Context.ReplaceElement(SElementIdentifierConstants.STONE);
                this.Context.SetElementTemperature(500);
            }
        }
    }
}