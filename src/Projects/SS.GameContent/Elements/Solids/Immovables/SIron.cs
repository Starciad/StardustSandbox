using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.GameContent.Elements.Solids.Immovables
{
    internal sealed class SIron : SImmovableSolid
    {
        internal SIron(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(66, 66, 66, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_13");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 30;
            this.defaultDensity = 7800;
            this.defaultExplosionResistance = 0.3f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 1200)
            {
                this.Context.ReplaceElement(SElementConstants.LAVA_IDENTIFIER);
                this.Context.SetStoredElement(SElementConstants.IRON_IDENTIFIER);
            }
        }
    }
}
