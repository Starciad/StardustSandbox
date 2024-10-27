using StardustSandbox.Game.Elements.Templates.Gases;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Gases
{
    public class SSmoke : SGas
    {
        public SSmoke(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 019;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_20");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 100;
        }
    }
}
