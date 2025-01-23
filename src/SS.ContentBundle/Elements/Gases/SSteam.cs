using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    internal sealed class SSteam : SGas
    {
        internal SSteam(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(171, 208, 218, 136);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_19");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 200;
            this.movementType = SGasMovementType.Spread;
            this.defaultDensity = 1;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 90)
            {
                this.Context.ReplaceElement(SElementConstants.IDENTIFIER_WATER);
            }
        }
    }
}
