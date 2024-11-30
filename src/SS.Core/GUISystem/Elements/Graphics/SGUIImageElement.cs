using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.GUISystem.Elements.Graphics
{
    public sealed class SGUIImageElement : SGUIGraphicElement
    {
        public override Texture2D Texture => this.texture;
        public override Rectangle? TextureClipArea => this.textureClipArea;

        private Texture2D texture = null;
        private Rectangle? textureClipArea = null;

        public SGUIImageElement(ISGame gameInstance) : base(gameInstance)
        {
            this.ShouldUpdate = false;
            this.IsVisible = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.texture != null)
            {
                spriteBatch.Draw(this.texture, this.Position, this.textureClipArea, this.Color, this.RotationAngle, this.texture.GetTextureOriginPoint(this.OriginPivot), this.Scale, this.SpriteEffects, 0f);
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
    }
}