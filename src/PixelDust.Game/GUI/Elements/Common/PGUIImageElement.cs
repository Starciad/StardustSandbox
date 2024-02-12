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
        public float RotationAngle { get; set; } = 0f;

        private Texture2D texture;
        private Rectangle? textureClipArea;
        private float rotationAngle;
        private SpriteEffects spriteEffects;
        private PCardinalDirection originPivot;

        public PGUIImageElement()
        {
            this.texture = null;
            this.textureClipArea = null;
            this.spriteEffects = SpriteEffects.None;
            this.originPivot = PCardinalDirection.Northwest;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position, this.textureClipArea, this.Style.Color, this.rotationAngle, this.texture.GetOrigin(this.OriginPivot), new Vector2(this.Style.Size.Width, this.Style.Size.Height), this.spriteEffects, 0f);
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
    }
}