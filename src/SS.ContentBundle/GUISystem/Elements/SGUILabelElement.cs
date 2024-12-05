using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.Elements
{
    public class SGUILabelElement : SGUIElement
    {
        public bool HasBorders => this.TopLeftBorder | this.TopRightBorder | this.BottomLeftBorder | this.BottomRightBorder;

        public bool TopLeftBorder { get; set; }
        public bool TopRightBorder { get; set; }
        public bool BottomLeftBorder { get; set; }
        public bool BottomRightBorder { get; set; }

        public Color TopLeftBorderColor { get; set; }
        public Color TopRightBorderColor { get; set; }
        public Color BottomLeftBorderColor { get; set; }
        public Color BottomRightBorderColor { get; set; }

        public Vector2 BorderOffset { get; set; }

        private readonly StringBuilder textContentStringBuilder = new();
        private SpriteFont textFont;

        public SGUILabelElement(ISGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.HasBorders)
            {
                if (this.TopLeftBorder)
                {
                    DrawTextWithBorder(spriteBatch, this.TopLeftBorderColor, -this.BorderOffset.X, -this.BorderOffset.Y);
                }

                if (this.TopRightBorder)
                {
                    DrawTextWithBorder(spriteBatch, this.TopRightBorderColor, this.BorderOffset.X, -this.BorderOffset.Y);
                }

                if (this.BottomLeftBorder)
                {
                    DrawTextWithBorder(spriteBatch, this.BottomLeftBorderColor, -this.BorderOffset.X, this.BorderOffset.Y);
                }

                if (this.BottomRightBorder)
                {
                    DrawTextWithBorder(spriteBatch, this.BottomRightBorderColor, this.BorderOffset.X, this.BorderOffset.Y);
                }
            }

            spriteBatch.DrawString(this.textFont, this.textContentStringBuilder, this.Position, this.Color, this.RotationAngle, this.textFont.GetSpriteFontOriginPoint(this.textContentStringBuilder, this.OriginPivot), this.Scale, this.SpriteEffects, 0f);
        }

        private void DrawTextWithBorder(SpriteBatch spriteBatch, Color color, float xOffset, float yOffset)
        {
            Vector2 offset = new(xOffset, yOffset);
            spriteBatch.DrawString(this.textFont, this.textContentStringBuilder, this.Position + offset, color, this.RotationAngle, this.textFont.GetSpriteFontOriginPoint(this.textContentStringBuilder, this.OriginPivot), this.Scale, this.SpriteEffects, 0f);
        }

        public SSize2 GetMeasureStringSize()
        {
            Vector2 result = this.textFont.MeasureString(this.textContentStringBuilder) * this.Scale / 2f;
            return new((int)result.X, (int)result.Y);
        }

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

        public void SetBorders(bool value)
        {
            this.TopLeftBorder = value;
            this.TopRightBorder = value;
            this.BottomLeftBorder = value;
            this.BottomRightBorder = value;
        }

        public void SetBordersColor(Color value)
        {
            this.TopLeftBorderColor = value;
            this.TopRightBorderColor = value;
            this.BottomLeftBorderColor = value;
            this.BottomRightBorderColor = value;
        }
    }
}
