using StardustSandbox.Core.Animations;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SLamp : SImmovableSolid
    {
        internal SLamp(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Rust;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_25");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(new SAnimation(gameInstance, [new(new(new(0), new(SSpritesConstants.SPRITE_SCALE)), 0)])));
            this.enableLightEmission = true;
            this.defaultLuminousIntensity = 10;
            this.defaultTemperature = 26;
            this.defaultDensity = 2800;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 600)
            {
                this.Context.DestroyElement();
            }
        }
    }
}