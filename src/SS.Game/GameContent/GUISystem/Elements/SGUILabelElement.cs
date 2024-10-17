using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.GUISystem.Elements;

using System.Text;

namespace StardustSandbox.Game.GameContent.GUISystem.Elements
{
    public class SGUILabelElement : SGUIElement
    {
        public bool HasBorders => this.topLeftBorder | this.topRightBorder | this.bottomLeftBorder | this.bottomRightBorder;

        private readonly StringBuilder textContentStringBuilder = new();

        private SpriteFont textFont;
        private Color textColor = Color.White;
        private float textRotation = 0f;
        private Vector2 textScale = Vector2.One;

        private bool topLeftBorder, topRightBorder, bottomLeftBorder, bottomRightBorder;
        private Color topLeftBorderColor, topRightBorderColor, bottomLeftBorderColor, bottomRightBorderColor;
        private Vector2 borderOffset = Vector2.One;

        public SGUILabelElement(SGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.HasBorders)
            {
                if (this.topLeftBorder)
                {
                    DrawTextWithBorder(spriteBatch, this.topLeftBorderColor, -this.borderOffset.X, -this.borderOffset.Y);
                }

                if (this.topRightBorder)
                {
                    DrawTextWithBorder(spriteBatch, this.topRightBorderColor, this.borderOffset.X, -this.borderOffset.Y);
                }

                if (this.bottomLeftBorder)
                {
                    DrawTextWithBorder(spriteBatch, this.bottomLeftBorderColor, -this.borderOffset.X, this.borderOffset.Y);
                }

                if (this.bottomRightBorder)
                {
                    DrawTextWithBorder(spriteBatch, this.bottomRightBorderColor, this.borderOffset.X, this.borderOffset.Y);
                }
            }

            spriteBatch.DrawString(this.textFont, this.textContentStringBuilder, this.Position, this.textColor, this.textRotation, GetOrigin(), this.textScale, SpriteEffects.None, 0f);
        }

        private void DrawTextWithBorder(SpriteBatch spriteBatch, Color color, float xOffset, float yOffset)
        {
            Vector2 offset = new(xOffset, yOffset);
            spriteBatch.DrawString(this.textFont, this.textContentStringBuilder, this.Position + offset, color, this.textRotation, GetOrigin(), this.textScale, SpriteEffects.None, 1f);
        }

        // ========================================= //

        public void SetTextContent(string value)
        {
            _ = this.textContentStringBuilder.Clear();
            _ = this.textContentStringBuilder.Append(value);
        }

        public void SetFontFamily(string fontFamilyName)
        {
            this.textFont = this.SGameInstance.AssetDatabase.GetFont(fontFamilyName);
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

        // ========================================= //
        // Borders

        public void SetBorders(bool value)
        {
            SetBorders(value, value, value, value);
        }

        public void SetBorders(bool topLeft, bool topRight, bool bottomLeft, bool bottomRight)
        {
            this.topLeftBorder = topLeft;
            this.topRightBorder = topRight;
            this.bottomLeftBorder = bottomLeft;
            this.bottomRightBorder = bottomRight;
        }

        public void SetBordersColor(Color color)
        {
            SetBordersColor(color, color, color, color);
        }

        public void SetBordersColor(Color topLeft, Color topRight, Color bottomLeft, Color bottomRight)
        {
            this.topLeftBorderColor = topLeft;
            this.topRightBorderColor = topRight;
            this.bottomLeftBorderColor = bottomLeft;
            this.bottomRightBorderColor = bottomRight;
        }

        public void SetBorderOffset(Vector2 offset)
        {
            this.borderOffset = offset;
        }

        // ========================================= //
        // Utilities

        private static Vector2 GetOrigin()
        {
            return Vector2.Zero;
        }
    }
}
