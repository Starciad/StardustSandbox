using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    public sealed class SSmoke : SGas
    {
        public SSmoke(ISGame gameInstance) : base(gameInstance)
        {
            this.id = (uint)SElementId.Smoke;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_20");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 100;
        }
    }
}
