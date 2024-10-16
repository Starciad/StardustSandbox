using StardustSandbox.Game.Elements.Templates.Gases;
using StardustSandbox.Game.GameContent.Elements.Rendering;

namespace StardustSandbox.Game.GameContent.Elements.Gases
{
    public class SSteam : SGas
    {
        public SSteam(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 018;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_19");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 100;
        }
    }
}
