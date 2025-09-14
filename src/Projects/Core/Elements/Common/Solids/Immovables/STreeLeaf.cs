using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.Elements.Common.Solids.Immovables
{
    internal sealed class STreeLeaf : SImmovableSolid
    {
        internal STreeLeaf(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.MossGreen;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_22");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 5;
            this.defaultDensity = 400;
            this.defaultExplosionResistance = 0.1f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 220)
            {
                if (SRandomMath.Chance(75))
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
