using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SDirt : SMovableSolid
    {
        public SDirt(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 000;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_1");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
        }
    }
}
