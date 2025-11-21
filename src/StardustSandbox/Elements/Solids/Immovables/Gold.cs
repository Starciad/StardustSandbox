using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Gold : ImmovableSolid
    {
        internal Gold(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Single;
            this.characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 22;
            this.defaultDensity = 17_150;
            this.defaultExplosionResistance = 0.3f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 1060)
            {
                this.Context.ReplaceElement(ElementIndex.Lava);
                this.Context.SetStoredElement(ElementIndex.Gold);
            }
        }
    }
}
