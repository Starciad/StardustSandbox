using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Lava : Liquid
    {
        internal Lava(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.defaultTemperature = 1000;
            this.defaultDensity = 3000;
            this.defaultExplosionResistance = 0.4f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue <= 500)
            {
                if (this.Context.SlotLayer.StoredElement == null)
                {
                    this.Context.ReplaceElement(ElementIndex.Stone);
                }
                else
                {
                    this.Context.ReplaceElement(this.Context.SlotLayer.StoredElement);
                }

                this.Context.SetElementTemperature(500);
            }
        }
    }
}