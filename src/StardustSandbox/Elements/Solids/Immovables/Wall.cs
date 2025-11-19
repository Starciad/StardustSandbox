using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Wall : ImmovableSolid
    {
        internal Wall(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.enableTemperature = false;
            this.isExplosionImmune = true;
            this.defaultDensity = 2200;
        }
    }
}
