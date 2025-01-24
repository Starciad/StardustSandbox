using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    internal sealed class SSmoke : SGas
    {
        internal SSmoke(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(56, 56, 56, 191);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_20");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 350;
            this.defaultDensity = 2;
        }

        protected override void OnBeforeStep()
        {
            if (SRandomMath.Chance(75, 101))
            {
                return;
            }

            Point topPosition = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y - 1);

            if (this.Context.IsEmptyWorldSlotLayer(topPosition, this.Context.Layer))
            {
                this.Context.SetPosition(topPosition);
            }
        }
    }
}
