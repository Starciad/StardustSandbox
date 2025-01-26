using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SGlass : SImmovableSolid
    {
        internal SGlass(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(249, 253, 254, 21);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_12");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 25;
            this.defaultDensity = 2500;
            this.defaultExplosionResistance = 0.5f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 620)
            {
                this.Context.ReplaceElement(SElementConstants.LAVA_IDENTIFIER);
                this.Context.SetStoredElement(SElementConstants.GLASS_IDENTIFIER);
            }
        }
    }
}