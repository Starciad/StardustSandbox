using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class Smoke : Gas
    {
        internal Smoke(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.defaultTemperature = 350;
            this.defaultDensity = 2;
        }

        protected override void OnBeforeStep()
        {
            if (SSRandom.Chance(75))
            {
                return;
            }

            Point topPosition = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y - 1);

            if (this.Context.IsEmptySlotLayer(topPosition, this.Context.Layer))
            {
                this.Context.SetPosition(topPosition);
            }
        }
    }
}
