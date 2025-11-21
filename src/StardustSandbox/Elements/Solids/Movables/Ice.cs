using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ice : MovableSolid
    {
        internal Ice(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = -25;
            this.defaultDensity = 920;
            this.defaultExplosionResistance = 1.2f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 0)
            {
                if (this.Context.SlotLayer.StoredElement == null)
                {
                    this.Context.ReplaceElement(ElementIndex.Water);
                }
                else
                {
                    this.Context.ReplaceElement(this.Context.SlotLayer.StoredElement);
                }

                this.Context.SetElementTemperature(13);
            }
        }
    }
}
