using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SGold : SImmovableSolid
    {
        internal SGold(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Gold;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_36");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.defaultTemperature = 22;
            this.defaultDensity = 17_150;
            this.defaultExplosionResistance = 0.3f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 1060)
            {
                this.Context.ReplaceElement(SElementConstants.LAVA_IDENTIFIER);
                this.Context.SetStoredElement(SElementConstants.GOLD_IDENTIFIER);
            }
        }
    }
}
