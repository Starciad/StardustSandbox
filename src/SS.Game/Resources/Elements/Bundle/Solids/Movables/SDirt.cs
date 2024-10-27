using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SDirt : SMovableSolid
    {
        public SDirt(ISGame gameInstance) : base(gameInstance)
        {
            this.Id = 000;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_1");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }
    }
}
