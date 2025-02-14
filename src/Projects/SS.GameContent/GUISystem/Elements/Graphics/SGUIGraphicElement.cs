using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Graphics
{
    internal abstract class SGUIGraphicElement(ISGame gameInstance) : SGUIElement(gameInstance)
    {
        internal Texture2D Texture { get; set; }
        internal Rectangle? TextureClipArea { get; set; }
    }
}
