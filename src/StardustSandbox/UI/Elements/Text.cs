using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.UI;
using StardustSandbox.UI.Elements.TextSystem;

using System;
using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.UI.Elements
{
    internal sealed class Text : UIElement
    {
        internal override Vector2 Size
        {
            get
            {
                if (this.isTextContentDirty)
                {
                    this.measuredText = MeasureText();
                    this.isTextContentDirty = false;
                }

                return this.measuredText;
            }

            set => throw new InvalidOperationException("Cannot set Size of Text directly. Size is determined by the text content and wrapping.");
        }

        internal Vector2 TextAreaSize { get; set; }
        internal float LineHeight { get; set; } = 1.0f;
        internal float WordSpacing { get; set; } = 0.0f;
        internal int LineCount => this.wrappedLines.Count;
        internal SpriteFontIndex SpriteFontIndex
        {
            get => this.spriteFontIndex;
            set
            {
                this.spriteFontIndex = value;
                this.spriteFont = AssetDatabase.GetSpriteFont(value);
            }
        }
        internal string TextContent
        {
            get => this.textContent;
            set
            {
                if (!this.textContent.Equals(value))
                {
                    WrapContent(value);

                    this.textContent = value;
                    this.isTextContentDirty = true;

                    RepositionRelativeToParent();
                }
            }
        }
        internal Color Color { get; set; }

        internal LabelBorderDirection BorderDirections { get; set; }
        internal float BorderThickness { get; set; }
        internal float BorderOffset { get; set; }
        internal Color BorderColor { get; set; }

        private SpriteFontIndex spriteFontIndex;
        private SpriteFont spriteFont;

        private string textContent;

        private bool isTextContentDirty;
        private Vector2 measuredText;

        private readonly List<string> wrappedLines = [];

        private static readonly char[] WordSplitSeparators = [' '];

        internal Text()
        {
            this.CanDraw = true;
            this.CanUpdate = true;

            this.textContent = string.Empty;

            this.Color = AAP64ColorPalette.White;
        }

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnUpdate(in GameTime gameTime)
        {
            return;
        }

        private void DrawBorders(SpriteBatch spriteBatch, Vector2 position)
        {
            if (this.BorderDirections == LabelBorderDirection.None)
            {
                return;
            }

            for (int i = 0; i < TextConstants.BORDER_DIRECTION_OFFSETS.Length; i++)
            {
                BorderDirectionOffset borderDirectionOffset = TextConstants.BORDER_DIRECTION_OFFSETS[i];

                if ((this.BorderDirections & borderDirectionOffset.Direction) != 0)
                {
                    for (float t = 0; t < this.BorderThickness; t += 1.0f)
                    {
                        float scale = (t + 1) / this.BorderThickness;
                        Vector2 offset = borderDirectionOffset.Offset * this.BorderOffset * scale;
                        spriteBatch.DrawString(this.spriteFont, this.textContent, position + offset, this.BorderColor, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
                    }
                }
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(this.textContent))
            {
                Vector2 position = new(0f, this.Position.Y);

                foreach (string line in this.wrappedLines)
                {
                    position.X = this.Position.X;

                    DrawBorders(spriteBatch, position);
                    spriteBatch.DrawString(this.spriteFont, line, position, this.Color, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);

                    position.Y += this.LineHeight * this.spriteFont.LineSpacing * this.Scale.Y;
                }
            }
        }

        private void WrapContent(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            this.wrappedLines.Clear();
            string[] words = value.Split(WordSplitSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            StringBuilder lineBuilder = new();
            float spaceWidth = this.spriteFont.MeasureString(" ").X * this.Scale.X;

            foreach (string word in words)
            {
                float measureString = this.spriteFont.MeasureString(lineBuilder + word).X * this.Scale.X;

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

        private Vector2 MeasureText()
        {
            if (this.wrappedLines.Count == 0)
            {
                return Vector2.Zero;
            }

            float maxWidth = 0f;
            float totalHeight = this.LineHeight * this.spriteFont.LineSpacing * this.Scale.Y * this.wrappedLines.Count;

            foreach (string line in this.wrappedLines)
            {
                float lineWidth = this.spriteFont.MeasureString(line).X * this.Scale.X;

                if (lineWidth > maxWidth)
                {
                    maxWidth = lineWidth;
                }
            }

            return new(maxWidth, totalHeight);
        }
    }
}
