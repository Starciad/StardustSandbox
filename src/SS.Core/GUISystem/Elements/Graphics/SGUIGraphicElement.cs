using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.GUISystem.Elements.Graphics
{
    public abstract class SGUIGraphicElement(ISGame gameInstance) : SGUIElement(gameInstance)
    {
        public Texture2D Texture { get; set; }
        public Rectangle? TextureClipArea { get; set; }
    }
}
