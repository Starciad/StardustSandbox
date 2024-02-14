using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Enums.General;

namespace PixelDust.Game.GUI.Elements.Common.Graphics
{
    public abstract class PGUIGraphicElement : PGUIElement
    {
        public virtual Texture2D Texture { get; }
        public virtual Rectangle? TextureClipArea { get; }
        public virtual Color Color { get; }
        public virtual SpriteEffects SpriteEffect { get; }
        public virtual PCardinalDirection OriginPivot { get; }
        public virtual float RotationAngle { get; }
        public virtual Vector2 Scale { get; }

        public virtual void SetTexture(Texture2D texture) { return; }
        public virtual void SetTextureClipArea(Rectangle clipArea) { return; }
        public virtual void SetColor(Color color) { return; }
        public virtual void SetSpriteEffects(SpriteEffects spriteEffects) { return; }
        public virtual void SetOriginPivot(PCardinalDirection direction) { return; }
        public virtual void SetRotationAngle(float angle) { return; }
        public virtual void SetScale(Vector2 scale) { return; }
    }
}
