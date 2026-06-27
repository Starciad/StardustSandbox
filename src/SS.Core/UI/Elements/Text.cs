/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.UI.Texts;

using System;
using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class Text : UIElement
    {
        internal SpriteFont SpriteFont { get; set; }
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
        internal bool HasTextContent => !string.IsNullOrWhiteSpace(this.textContent);
        internal string TextContent
        {
            get => this.textContent;
            set
            {
                if (value is not null && !this.textContent.Equals(value))
                {
                    if (this.WrapText)
                    {
                        WrapContent(value);
                    }

                    this.textContent = value;
                    this.isTextContentDirty = true;

                    RepositionRelativeToParent();
                }
            }
        }

        internal Color Color { get; set; }
        internal TextBorderSettings BorderSettings { get; set; }
        internal bool WrapText { get; set; }

        private string textContent;

        private bool isTextContentDirty;
        private Vector2 measuredText;

        private readonly List<string> wrappedLines = [];

        private static readonly char[] WordSplitSeparators = [' '];

        public Text()
        {
            this.textContent = string.Empty;
            this.Color = Color.White;
        }

        public Text(SpriteFont spriteFont) : this()
        {
            this.SpriteFont = spriteFont;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (!this.HasTextContent)
            {
                return;
            }

            Vector2 position = new(0f, this.Position.Y);

            foreach (string line in this.wrappedLines)
            {
                position.X = this.Position.X;

                spriteBatch.DrawString(this.SpriteFont, line, position, this.Color, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);

                position.Y += this.LineHeight * this.SpriteFont.LineSpacing * this.Scale.Y;
            }
        }

        private void WrapContent(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            this.wrappedLines.Clear();
            string[] words = value.Split(WordSplitSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

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

        private Vector2 MeasureText()
        {
            if (this.wrappedLines.Count == 0 || string.IsNullOrWhiteSpace(this.textContent))
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

