using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Text;

namespace StardustSandbox.Core.GUISystem.Elements
{
    public class SGUILabelElement : SGUIElement
    {
        public bool HasBorders => this.topLeftBorder | this.topRightBorder | this.bottomLeftBorder | this.bottomRightBorder;

        private readonly StringBuilder textContentStringBuilder = new();

        private SpriteFont textFont;

        private bool topLeftBorder, topRightBorder, bottomLeftBorder, bottomRightBorder;
        private Color topLeftBorderColor, topRightBorderColor, bottomLeftBorderColor, bottomRightBorderColor;
        private Vector2 borderOffset = Vector2.One;

        public SGUILabelElement(ISGame gameInstance) : base(gameInstance)
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

            spriteBatch.DrawString(this.textFont, this.textContentStringBuilder, this.Position, this.Color, this.RotationAngle, this.textFont.GetSpriteFontOriginPoint(this.textContentStringBuilder, this.OriginPivot), this.Scale, this.SpriteEffects, 0f);
        }

        private void DrawTextWithBorder(SpriteBatch spriteBatch, Color color, float xOffset, float yOffset)
        {
            Vector2 offset = new(xOffset, yOffset);
            spriteBatch.DrawString(this.textFont, this.textContentStringBuilder, this.Position + offset, color, this.RotationAngle, this.textFont.GetSpriteFontOriginPoint(this.textContentStringBuilder, this.OriginPivot), this.Scale, this.SpriteEffects, 0f);
        }

        // ========================================= //

        public SSize2 GetMeasureStringSize()
        {
            Vector2 result = this.textFont.MeasureString(this.textContentStringBuilder) * this.Scale / 2f;
            return new((int)result.X, (int)result.Y);
        }

        // ========================================= //

        public void AppendTextContent(string value)
        {
            _ = this.textContentStringBuilder.Append(value);
        }
        public void SetTextContent(string value)
        {
            _ = this.textContentStringBuilder.Clear();
            _ = this.textContentStringBuilder.Append(value);
        }

        public void SetFontFamily(string fontFamilyName)
        {
            this.textFont = this.SGameInstance.AssetDatabase.GetFont(fontFamilyName);
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
    }
}
