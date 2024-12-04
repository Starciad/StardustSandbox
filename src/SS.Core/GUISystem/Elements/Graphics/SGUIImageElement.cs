using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.GUISystem.Elements.Graphics
{
    public sealed class SGUIImageElement : SGUIGraphicElement
    {
        public SGUIImageElement(ISGame gameInstance) : base(gameInstance)
        {
            this.ShouldUpdate = false;
            this.IsVisible = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.Texture != null)
            {
                spriteBatch.Draw(this.Texture, this.Position, this.TextureClipArea, this.Color, this.RotationAngle, this.Texture.GetTextureOriginPoint(this.OriginPivot), this.Scale, this.SpriteEffects, 0f);
            }
        }
    }
}