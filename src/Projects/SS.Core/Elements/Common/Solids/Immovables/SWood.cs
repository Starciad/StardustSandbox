using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.Elements.Common.Solids.Immovables
{
    internal sealed class SWood : SImmovableSolid
    {
        internal SWood(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(67, 34, 0, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_15");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 35;
            this.defaultDensity = 700;
            this.defaultExplosionResistance = 1.5f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 300)
            {
                if (SRandomMath.Chance(65))
                {
                    this.Context.ReplaceElement(SElementConstants.FIRE_IDENTIFIER);
                }
                else
                {
                    this.Context.ReplaceElement(SElementConstants.ASH_IDENTIFIER);
                }
            }
        }
    }
}
