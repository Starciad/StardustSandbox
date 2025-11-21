using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Energies
{
    internal abstract class Energy : Element
    {
        internal Energy(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.category = ElementCategory.Energy;
        }
    }
}
