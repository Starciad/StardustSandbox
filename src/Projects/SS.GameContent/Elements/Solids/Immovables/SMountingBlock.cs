using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SMountingBlock : SImmovableSolid
    {
        internal SMountingBlock(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.White;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_23");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.defaultTemperature = 20;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 150;
            this.defaultDensity = 1800;
            this.defaultExplosionResistance = 1.5f;
        }

        protected override void OnInstantiated()
        {
            this.Context.SetElementColorModifier(this.Context.Layer, SElementConstants.COLORS_OF_MOUNTING_BLOCKS.GetRandomItem());
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 300)
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
