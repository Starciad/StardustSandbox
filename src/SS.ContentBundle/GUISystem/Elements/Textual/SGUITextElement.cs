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
        private sealed class TextLine(SpriteFont spriteFont)
        {
            public IEnumerable<TextWord> Words => this.words;

            private readonly List<TextWord> words = [];
            private readonly SpriteFont spriteFont = spriteFont;

            public void AddWord(string value)
            {
                this.words.Add(new(this.spriteFont, value));
            }
        }

        private sealed class TextWord(SpriteFont spriteFont, string content)
        {
            public int Length => content.Length;
            public string Content => content;

            private readonly SpriteFont spriteFont = spriteFont;

            public SSize2F GetSize()
            {
                Vector2 measureString = this.spriteFont.MeasureString(this.Content);

                return new(measureString.X, measureString.Y);
            }
        }

        private readonly StringBuilder textStringBuilder = new();
        private readonly Rectangle textRectangle = new();

        private readonly float lineHeight;
        private readonly float wordSpacing;

        private readonly List<TextLine> textLines = [];

        public void SetTextContent(string value)
        {
            this.textLines.Clear();

            float initialXPositioin = this.Position.X;
            float currentXPosition = this.Position.X;

            float horizontalWrapperPosition = this.textRectangle.Left;

            string[] words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            TextLine line = new(this.SpriteFont);

            for (int i = 0; i < words.Length; i++)
            {
                Vector2 measuredString = this.SpriteFont.MeasureString(words[i]);

                // Break Text
                if (currentXPosition + measuredString.X > horizontalWrapperPosition)
                {
                    currentXPosition = initialXPositioin;

                    this.textLines.Add(line);
                    continue;
                }

                line.AddWord(words[i]);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float initialXPosition = this.Position.X;

            Vector2 currentPosition = this.Position;

            foreach (TextLine line in this.textLines)
            {
                foreach (TextWord word in line.Words)
                {
                    SSize2 size = word.GetSize();
                    float xPosition = currentPosition.X + size.Width + this.wordSpacing;

                    if (xPosition > this.textRectangle.Left)
                    {
                        currentPosition.X = initialXPosition;
                        currentPosition.Y += this.lineHeight;
                    }
                    else
                    {
                        currentPosition.X = xPosition;
                    }

                    spriteBatch.DrawString(this.SpriteFont, word.Content, currentPosition, this.Color, this.RotationAngle, this.SpriteFont.GetSpriteFontOriginPoint(word.Content, this.OriginPivot), this.Scale, this.SpriteEffects, 0f, false);
                }
            }
        }
    }
}
