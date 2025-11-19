using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Dirt : MovableSolid
    {
        internal Dirt(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
            this.defaultDensity = 1600;
            this.defaultExplosionResistance = 1f;
        }
    }
}
