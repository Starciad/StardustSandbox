using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SDirt : SMovableSolid
    {
        public SDirt(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Dirt;
            this.referenceColor = SColorPalette.Burgundy;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_1");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
        }
    }
}
