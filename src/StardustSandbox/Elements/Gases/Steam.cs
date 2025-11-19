using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class Steam : Gas
    {
        internal Steam(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.defaultTemperature = 200;
            this.defaultDensity = 1;
        }

        protected override void OnBeforeStep()
        {
            if (SSRandom.Chance(60))
            {
                return;
            }

            Point topPosition = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y - 1);

            if (this.Context.IsEmptySlotLayer(topPosition, this.Context.Layer))
            {
                this.Context.SetPosition(topPosition);
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 35)
            {
                this.Context.ReplaceElement(ElementIndex.Water);
            }
        }
    }
}
