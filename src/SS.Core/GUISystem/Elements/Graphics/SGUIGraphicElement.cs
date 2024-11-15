using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.GUISystem.Elements.Graphics
{
    public abstract class SGUIGraphicElement(ISGame gameInstance) : SGUIElement(gameInstance)
    {
        public virtual Texture2D Texture { get; }
        public virtual Rectangle? TextureClipArea { get; }
        public virtual Color Color { get; }
        public virtual SpriteEffects SpriteEffects { get; }
        public virtual SCardinalDirection OriginPivot { get; }
        public virtual float RotationAngle { get; }
        public virtual Vector2 Scale { get; }

        public virtual void SetTexture(Texture2D texture) { return; }
        public virtual void SetTextureClipArea(Rectangle clipArea) { return; }
        public virtual void SetColor(Color color) { return; }
        public virtual void SetSpriteEffects(SpriteEffects spriteEffects) { return; }
        public virtual void SetOriginPivot(SCardinalDirection direction) { return; }
        public virtual void SetRotationAngle(float angle) { return; }
        public virtual void SetScale(Vector2 scale) { return; }
    }
}
