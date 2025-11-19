using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Mathematics;

namespace StardustSandbox.UISystem.Elements.Graphics
{
    internal sealed class ImageUIElement : GraphicUIElement
    {
        internal ImageUIElement()
        {
            this.ShouldUpdate = false;
            this.IsVisible = true;
        }

        internal override void Initialize()
        {

        }

        internal override void Update(GameTime gameTime)
        {

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Texture != null)
            {
                spriteBatch.Draw(this.Texture, this.Position, this.TextureClipArea, this.Color, this.RotationAngle, this.Texture.GetTextureOriginPoint(this.OriginPivot), this.Scale, this.SpriteEffects, 0f);
            }
        }
    }
}