using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    public class SSteam : SGas
    {
        public SSteam(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 018;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_19");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 100;
        }
    }
}
