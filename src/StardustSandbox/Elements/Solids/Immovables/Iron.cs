using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Iron : ImmovableSolid
    {
        internal Iron(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.defaultTemperature = 30;
            this.defaultDensity = 7800;
            this.defaultExplosionResistance = 0.3f;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 1200)
            {
                this.Context.ReplaceElement(ElementIndex.Lava);
                this.Context.SetStoredElement(ElementIndex.Iron);
            }
        }
    }
}
