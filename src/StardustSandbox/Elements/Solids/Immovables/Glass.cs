using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Glass : ImmovableSolid
    {
        internal Glass(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.defaultTemperature = 25;
            this.defaultDensity = 2500;
            this.defaultExplosionResistance = 0.5f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 620)
            {
                this.Context.ReplaceElement(ElementIndex.Lava);
                this.Context.SetStoredElement(ElementIndex.Glass);
            }
        }
    }
}