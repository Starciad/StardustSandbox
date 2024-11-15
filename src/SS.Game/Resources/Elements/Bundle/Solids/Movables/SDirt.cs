using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
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
