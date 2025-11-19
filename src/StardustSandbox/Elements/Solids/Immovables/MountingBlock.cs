using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class MountingBlock : ImmovableSolid
    {
        internal MountingBlock(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementSingleRenderingMechanism());
            this.defaultTemperature = 20;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 150;
            this.defaultDensity = 1800;
            this.defaultExplosionResistance = 1.5f;
        }

        protected override void OnInstantiated()
        {
            this.Context.SetElementColorModifier(this.Context.Layer, ElementConstants.COLORS_OF_MOUNTING_BLOCKS.GetRandomItem());
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 300)
            {
                if (SSRandom.Chance(75))
                {
                    this.Context.ReplaceElement(ElementIndex.Fire);
                }
                else
                {
                    this.Context.ReplaceElement(ElementIndex.Ash);
                }
            }
        }
    }
}
