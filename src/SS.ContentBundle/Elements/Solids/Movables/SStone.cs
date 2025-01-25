using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SStone : SMovableSolid
    {
        internal SStone(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(66, 65, 65, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_4");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
            this.defaultDensity = 2500;
            this.defaultExplosionResistance = 0.5f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 600)
            {
                this.Context.ReplaceElement(SElementConstants.IDENTIFIER_LAVA);
            }
        }
    }
}
