using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SAntColony : SImmovableSolid
    {
        internal SAntColony(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Sand;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_25");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
            this.defaultDensity = 1800;
        }
    }
}
