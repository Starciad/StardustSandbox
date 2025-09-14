using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Elements.Common.Solids.Immovables
{
    internal sealed class SLamp : SImmovableSolid
    {
        internal SLamp(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Rust;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_25");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.defaultTemperature = 26;
            this.defaultDensity = 2800;
            this.defaultExplosionResistance = 0.8f;
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