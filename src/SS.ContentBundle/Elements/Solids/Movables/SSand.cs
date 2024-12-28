using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SSand : SMovableSolid
    {
        internal SSand(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(248, 246, 68, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_7");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
            this.defaultDensity = 1500;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 1500)
            {
                this.Context.ReplaceElement(SElementConstants.IDENTIFIER_GLASS);
            }
        }
    }
}
