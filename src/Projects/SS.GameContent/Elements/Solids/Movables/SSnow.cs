using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SSnow : SMovableSolid
    {
        internal SSnow(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(202, 242, 239, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_8");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = -15;
            this.defaultDensity = 600;
            this.defaultExplosionResistance = 0.3f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 8)
            {
                if (this.Context.SlotLayer.StoredElement == null)
                {
                    this.Context.ReplaceElement(SElementConstants.WATER_IDENTIFIER);
                }
                else
                {
                    this.Context.ReplaceElement(this.Context.SlotLayer.StoredElement);
                }

                this.Context.SetElementTemperature(12);
            }
        }
    }
}
