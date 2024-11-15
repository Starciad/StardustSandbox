using StardustSandbox.Game.Elements.Templates.Gases;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Gases
{
    public class SSmoke : SGas
    {
        public SSmoke(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 019;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_20");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 100;
        }
    }
}
