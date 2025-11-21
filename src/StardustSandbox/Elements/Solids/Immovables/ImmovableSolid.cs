using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal abstract class ImmovableSolid : Solid
    {
        internal ImmovableSolid(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.category = ElementCategory.ImmovableSolid;
        }
    }
}
