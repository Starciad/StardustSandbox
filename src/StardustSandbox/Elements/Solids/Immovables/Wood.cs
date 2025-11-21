using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Wood : ImmovableSolid
    {
        internal Wood(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 20;
            this.defaultFlammabilityResistance = 35;
            this.defaultDensity = 700;
            this.defaultExplosionResistance = 1.5f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 300)
            {
                if (SSRandom.Chance(65))
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
