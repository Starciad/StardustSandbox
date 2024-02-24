using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Text;

namespace PixelDust.Game.GUI.Elements.Common
{
    public class PGUILabelElement : PGUIElement
    {
        private readonly StringBuilder textContentStringBuilder = new();

        private SpriteFont textFont;
        private Color textColor = Color.White;
        private float textRotation = 0f;
        private Vector2 textScale = Vector2.One;

        public PGUILabelElement()
        {
            this.IsVisible = true;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.textFont, this.textContentStringBuilder, this.Position, this.textColor, this.textRotation, GetOrigin(), this.textScale, SpriteEffects.None, 0f);
        }

        public void SetTextContent(string value)
        {
            this.textContentStringBuilder.Clear();
            this.textContentStringBuilder.Append(value);
        }

        public void SetFontFamily(string fontFamilyName)
        {
            this.textFont = this.Game.AssetDatabase.GetFont(fontFamilyName);
        }

        public void SetColor(Color color)
        {
            this.textColor = color;
        }

        public void SetRotation(float rotation)
        {
            this.textRotation = rotation;
        }

        public void SetScale(Vector2 scale)
        {
            this.textScale = scale;
        }

        private static Vector2 GetOrigin()
        {
            return Vector2.Zero;
        }
    }
}
