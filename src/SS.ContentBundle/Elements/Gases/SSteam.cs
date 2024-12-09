using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    public sealed class SSteam : SGas
    {
        public SSteam(ISGame gameInstance) : base(gameInstance)
        {
            this.id = (uint)SElementId.Steam;
            this.referenceColor = new(171, 208, 218, 136);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_19");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 100;
            this.movementType = SGasMovementType.Spread;
        }
    }
}
