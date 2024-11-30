using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.GUISystem.Elements.Graphics
{
    public abstract class SGUIGraphicElement(ISGame gameInstance) : SGUIElement(gameInstance)
    {
        public virtual Texture2D Texture { get; }
        public virtual Rectangle? TextureClipArea { get; }

        public virtual void SetTexture(Texture2D texture) { return; }
        public virtual void SetTextureClipArea(Rectangle clipArea) { return; }
    }
}
