using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SMud : SMovableSolid
    {
        internal SMud(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(75, 36, 38, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_2");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 18;
            this.defaultDensity = 1500;
            this.defaultExplosionResistance = 0.6f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement(SElementConstants.DIRT_IDENTIFIER);
            }
        }
    }
}
