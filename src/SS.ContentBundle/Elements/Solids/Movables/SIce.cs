using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SIce : SMovableSolid
    {
        internal SIce(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(117, 215, 246, 255);
            this.texture = this.SGameInstance.AssetDatabase.GetTexture("element_6");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = -25;
            this.defaultDensity = 920;
            this.defaultExplosionResistance = 1.2f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 0)
            {
                if (this.Context.SlotLayer.StoredElement == null)
                {
                    this.Context.ReplaceElement(SElementConstants.WATER_IDENTIFIER);
                }
                else
                {
                    this.Context.ReplaceElement(this.Context.SlotLayer.StoredElement);
                }

                this.Context.SetElementTemperature(13);
            }
        }
    }
}
