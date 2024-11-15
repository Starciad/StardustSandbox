using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.GUISystem.Elements.Graphics
{
    public sealed class SGUIImageElement : SGUIGraphicElement
    {
        public override Texture2D Texture => this.texture;
        public override Rectangle? TextureClipArea => this.textureClipArea;
        public override Color Color => this.color;
        public override SpriteEffects SpriteEffects => this.spriteEffects;
        public override SCardinalDirection OriginPivot => this.originPivot;
        public override float RotationAngle => this.rotationAngle;
        public override Vector2 Scale => this.scale;

        private Texture2D texture = null;
        private Rectangle? textureClipArea = null;
        private Color color = Color.White;
        private SpriteEffects spriteEffects = SpriteEffects.None;
        private SCardinalDirection originPivot = SCardinalDirection.Northwest;
        private float rotationAngle = 0f;
        private Vector2 scale = Vector2.One;

        public SGUIImageElement(ISGame gameInstance) : base(gameInstance)
        {
            this.ShouldUpdate = false;
            this.IsVisible = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.texture != null)
            {
                spriteBatch.Draw(this.texture, this.Position, this.textureClipArea, this.color, this.rotationAngle, this.texture.GetOrigin(this.OriginPivot), this.scale, this.spriteEffects, 0f);
            }
        }

        public override void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public override void SetTextureClipArea(Rectangle clipArea)
        {
            this.textureClipArea = clipArea;
        }

        public override void SetColor(Color color)
        {
            this.color = color;
        }

        public override void SetSpriteEffects(SpriteEffects spriteEffects)
        {
            this.spriteEffects = spriteEffects;
        }

        public override void SetOriginPivot(SCardinalDirection direction)
        {
            this.originPivot = direction;
        }

        public override void SetRotationAngle(float angle)
        {
            this.rotationAngle = angle;
        }

        public override void SetScale(Vector2 scale)
        {
            this.scale = scale;
        }
    }
}