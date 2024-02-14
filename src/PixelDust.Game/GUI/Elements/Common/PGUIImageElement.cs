using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Enums.General;
using PixelDust.Game.Extensions;

namespace PixelDust.Game.GUI.Elements.Common
{
    public sealed class PGUIImageElement : PGUIElement
    {
        public Texture2D Texture => this.texture;
        public Rectangle? TextureClipArea => this.textureClipArea;
        public SpriteEffects SpriteEffect => this.spriteEffects;
        public PCardinalDirection OriginPivot => this.originPivot;
        public float RotationAngle => this.rotationAngle;
        public Vector2 Scale => this.scale;

        private Texture2D texture;
        private Rectangle? textureClipArea;
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
                spriteBatch.Draw(this.texture, this.Position, this.textureClipArea, this.Style.Color, this.rotationAngle, this.texture.GetOrigin(this.OriginPivot), this.scale, this.spriteEffects, 0f);
            }
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void SetTextureClipArea(Rectangle clipArea)
        {
            this.textureClipArea = clipArea;
        }

        public void SetSpriteEffects(SpriteEffects spriteEffects)
        {
            this.spriteEffects = spriteEffects;
        }

        public void SetOriginPivot(PCardinalDirection direction)
        {
            this.originPivot = direction;
        }

        public void SetRotationAngle(float angle)
        {
            this.rotationAngle = angle;
        }

        public void SetScale(Vector2 scale)
        {
            this.scale = scale;
        }
    }
}