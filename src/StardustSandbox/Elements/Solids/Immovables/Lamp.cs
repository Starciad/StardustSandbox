using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Lamp : ImmovableSolid
    {
        internal Lamp(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementSingleRenderingMechanism());
            this.defaultTemperature = 26;
            this.defaultDensity = 2800;
            this.defaultExplosionResistance = 0.8f;
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