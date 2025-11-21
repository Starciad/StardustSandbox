using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Mud : MovableSolid
    {
        internal Mud(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 18;
            this.defaultDensity = 1500;
            this.defaultExplosionResistance = 0.6f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement(ElementIndex.Dirt);
            }
        }
    }
}
