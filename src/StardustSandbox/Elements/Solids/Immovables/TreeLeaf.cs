using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class TreeLeaf : ImmovableSolid
    {
        internal TreeLeaf(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 22;
            this.defaultFlammabilityResistance = 5;
            this.defaultDensity = 400;
            this.defaultExplosionResistance = 0.1f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 220)
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
