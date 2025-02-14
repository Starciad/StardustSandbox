using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.GameContent.Elements.Solids.Immovables
{
    internal sealed class SRedBrick : SImmovableSolid
    {
        internal SRedBrick(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Crimson;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_21");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 25;
            this.defaultDensity = 2400;
            this.defaultExplosionResistance = 2.5f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 1727)
            {
                this.Context.ReplaceElement(SElementConstants.LAVA_IDENTIFIER);
                this.Context.SetStoredElement(SElementConstants.RED_BRICK_IDENTIFIER);
            }
        }
    }
}
