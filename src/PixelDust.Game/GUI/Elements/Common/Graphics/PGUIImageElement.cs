using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Enums.General;
using PixelDust.Game.Extensions;

namespace PixelDust.Game.GUI.Elements.Common.Graphics
{
    public sealed class PGUIImageElement : PGUIGraphicElement
    {
        public override Texture2D Texture => this.texture;
        public override Rectangle? TextureClipArea => this.textureClipArea;
        public override Color Color => this.color;
        public override SpriteEffects SpriteEffect => this.spriteEffects;
        public override PCardinalDirection OriginPivot => this.originPivot;
        public override float RotationAngle => this.rotationAngle;
        public override Vector2 Scale => this.scale;

        private Texture2D texture;
        private Rectangle? textureClipArea;
        private Color color;
        private float rotationAngle;
        private SpriteEffects spriteEffects;
        private PCardinalDirection originPivot;
        private Vector2 scale;

        public PGUIImageElement()
        {
            this.texture = null;
            this.textureClipArea = null;
            this.rotationAngle = 0f;
            this.spriteEffects = SpriteEffects.None;
            this.originPivot = PCardinalDirection.Northwest;
            this.scale = Vector2.One;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
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

        public override void SetOriginPivot(PCardinalDirection direction)
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