using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Lamp : ImmovableSolid
    {
        internal Lamp(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Single;
            this.characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 26;
            this.defaultDensity = 2800;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 600)
            {
                this.Context.DestroyElement();
            }
        }
    }
}