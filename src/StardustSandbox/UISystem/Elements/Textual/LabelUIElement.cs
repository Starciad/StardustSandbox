using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Mathematics;

namespace StardustSandbox.UISystem.Elements.Textual
{
    internal sealed class LabelUIElement : TextualUIElement
    {
        internal override void Initialize()
        {
            return;
        }

        internal override void Update(GameTime gameTime)
        {
            return;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = this.SpriteFont.GetSpriteFontOriginPoint(this.ContentStringBuilder, this.OriginPivot);

            DrawBorders(spriteBatch, this.ContentStringBuilder, this.Position, this.SpriteFont, this.RotationAngle, origin, this.Scale, this.SpriteEffects);
            spriteBatch.DrawString(this.SpriteFont, this.ContentStringBuilder.ToString(), this.Position, this.Color, this.RotationAngle, origin, this.Scale, this.SpriteEffects, 0.0f);
        }
    }
}
