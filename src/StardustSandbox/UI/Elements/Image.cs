using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.UI.Elements
{
    internal sealed class Image : UIElement
    {
        internal bool HasTexture => this.texture != null;
        internal bool HasSourceRectangle => this.sourceRectangle != null;

        internal Texture2D Texture
        {
            get => this.texture;
            set => this.texture = value;
        }
        internal Rectangle? SourceRectangle
        {
            get => this.sourceRectangle;
            set => this.sourceRectangle = value;
        }
        internal Color Color
        {
            get => this.color;
            set => this.color = value;
        }

        private Color color;
        private Texture2D texture;
        private Rectangle? sourceRectangle;

        internal Image()
        {
            this.CanDraw = true;
            this.CanUpdate = true;

            this.color = Color.White;
        }

        internal override void Initialize()
        {
            base.Initialize();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (this.HasTexture)
            {
                spriteBatch.Draw(this.texture, this.Position, this.sourceRectangle, this.color, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);
            }

            base.Draw(spriteBatch);
        }
    }
}
