using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    internal sealed class SSteam : SGas
    {
        internal SSteam(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(171, 208, 218, 136);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_19");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 200;
            this.defaultDensity = 1;
        }

        protected override void OnBeforeStep()
        {
            if (SRandomMath.Chance(60, 101))
            {
                return;
            }

            Point topPosition = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y - 1);

            if (this.Context.IsEmptyWorldSlotLayer(topPosition, this.Context.Layer))
            {
                this.Context.SetPosition(topPosition);
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 35)
            {
                this.Context.ReplaceElement(SElementConstants.IDENTIFIER_WATER);
            }
        }
    }
}
