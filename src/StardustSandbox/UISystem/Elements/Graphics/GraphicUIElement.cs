using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.UISystem.Elements.Graphics
{
    internal abstract class GraphicUIElement : UIElement
    {
        internal Texture2D Texture { get; set; }
        internal Rectangle? TextureClipArea { get; set; }
    }
}
