using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SDirt : SMovableSolid
    {
        internal SDirt(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Burgundy;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_1");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
            this.defaultDensity = 1600;
            this.defaultExplosionResistance = 1f;
        }
    }
}
