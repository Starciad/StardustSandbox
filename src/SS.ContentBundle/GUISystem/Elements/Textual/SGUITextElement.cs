using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Textual
{
    internal sealed class SGUITextElement(ISGame gameInstance) : SGUITextualElement(gameInstance)
    {
        internal SSize2F TextAreaSize { get; set; }
        internal float LineHeight { get; set; } = 1.0f;
        internal float WordSpacing { get; set; } = 0.0f;
        internal int LineCount => this.wrappedLines.Count;

        private readonly List<string> wrappedLines = [];

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

                if (measureString + spaceWidth >= this.TextAreaSize.Width)
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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

        internal override SSize2F GetStringSize()
        {
            if (this.wrappedLines.Count == 0)
            {
                return SSize2F.Zero;
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
