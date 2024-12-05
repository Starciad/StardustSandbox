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
    public sealed class SGUITextElement(ISGame gameInstance) : SGUITextualElement(gameInstance)
    {
        public SSize2F TextAreaSize { get; set; }
        public float LineHeight { get; set; } = 1.0f;
        public float WordSpacing { get; set; } = 0.0f;

        private readonly List<string> wrappedLines = [];

        public override void SetTextualContent(string value)
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
            float yPosition = this.Position.Y;

            foreach (string line in this.wrappedLines)
            {
                float xPosition = this.Position.X;

                spriteBatch.DrawString(this.SpriteFont, line, new Vector2(xPosition, yPosition), this.Color, this.RotationAngle, this.SpriteFont.GetSpriteFontOriginPoint(line, this.OriginPivot), this.Scale, this.SpriteEffects, 0f);
                yPosition += this.LineHeight * this.SpriteFont.LineSpacing * this.Scale.Y;
            }
        }
    }
}
