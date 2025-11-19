using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Mathematics;

using System;
using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.UISystem.Elements.Textual
{
    internal sealed class TextUIElement : TextualUIElement
    {
        internal Vector2 TextAreaSize { get; set; }
        internal float LineHeight { get; set; } = 1.0f;
        internal float WordSpacing { get; set; } = 0.0f;
        internal int LineCount => this.wrappedLines.Count;

        private readonly List<string> wrappedLines = [];

        internal override void Initialize()
        {
            return;
        }

        internal override void Update(GameTime gameTime)
        {
            return;
        }

        internal override void SetTextualContent(string value)
        {
            base.SetTextualContent(value);
            WrapText();
        }

        internal override void SetTextualContent(StringBuilder value)
        {
            base.SetTextualContent(value);
            WrapText();
        }

        private void WrapText()
        {
            this.wrappedLines.Clear();
            string[] words = this.Content.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            StringBuilder lineBuilder = new();
            float spaceWidth = this.SpriteFont.MeasureString(" ").X * this.Scale.X;

            foreach (string word in words)
            {
                float measureString = this.SpriteFont.MeasureString(lineBuilder + word).X * this.Scale.X;

                if (measureString + spaceWidth >= this.TextAreaSize.X)
                {
                    this.wrappedLines.Add(lineBuilder.ToString().TrimEnd());
                    _ = lineBuilder.Clear();
                }

                _ = lineBuilder.Append(word + " ");
            }

            if (lineBuilder.Length > 0)
            {
                this.wrappedLines.Add(lineBuilder.ToString().TrimEnd());
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new(0f, this.Position.Y);

            foreach (string line in this.wrappedLines)
            {
                position.X = this.Position.X;
                Vector2 origin = this.SpriteFont.GetSpriteFontOriginPoint(line, this.OriginPivot);

                DrawBorders(spriteBatch, line, position, this.SpriteFont, this.RotationAngle, origin, this.Scale, this.SpriteEffects);
                spriteBatch.DrawString(this.SpriteFont, line, position, this.Color, this.RotationAngle, origin, this.Scale, this.SpriteEffects, 0f);

                position.Y += this.LineHeight * this.SpriteFont.LineSpacing * this.Scale.Y;
            }
        }

        internal override Vector2 GetStringSize()
        {
            if (this.wrappedLines.Count == 0)
            {
                return Vector2.Zero;
            }

            float maxWidth = 0f;
            float totalHeight = this.LineHeight * this.SpriteFont.LineSpacing * this.Scale.Y * this.wrappedLines.Count;

            foreach (string line in this.wrappedLines)
            {
                float lineWidth = this.SpriteFont.MeasureString(line).X * this.Scale.X;

                if (lineWidth > maxWidth)
                {
                    maxWidth = lineWidth;
                }
            }

            return new(maxWidth, totalHeight);
        }
    }
}
