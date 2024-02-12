using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Extensions;

namespace PixelDust.Game.GUI.Elements.Common
{
    public sealed class PGUIImageElement : PGUIElement
    {
        public Texture2D Texture => this.texture;
        public Rectangle? TextureClipArea => this.textureClipArea;
        public SpriteEffects SpriteEffect => this.spriteEffects;

        private Texture2D texture;
        private Rectangle? textureClipArea;
        private SpriteEffects spriteEffects;

        public PGUIImageElement()
        {
            this.texture = null;
            this.textureClipArea = null;
            this.spriteEffects = SpriteEffects.None;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position, this.textureClipArea, this.Style.Color, this.Style.RotationAngle, this.texture.GetOrigin(this.Style.OriginPivot), new Vector2(this.Style.Size.Width, this.Style.Size.Height), this.spriteEffects, 0f);
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
    }
}
