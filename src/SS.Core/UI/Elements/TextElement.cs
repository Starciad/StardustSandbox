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

using StardustSandbox.Core.UI.Text.LexicalAnalyzer;
using StardustSandbox.Core.UI.Text.Processors;

using System;
using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class TextElement : UIElement
    {
        internal SpriteFont SpriteFont { get; set; }
        internal override Vector2 Size
        {
            get => this.measuredText;
            set => throw new InvalidOperationException("Cannot set Size of Text directly. Size is determined by the text content and wrapping.");
        }

        internal Vector2 AreaSize { get; set; }
        internal Color Color { get; set; }
        internal bool WrapContent { get; set; }

        private string textContent;
        private Vector2 measuredText = Vector2.One;

        private GlyphLayout glyphLayout;
        private IEnumerable<Token> tokens;

        private readonly StringBuilder charBuffer = new(1);

        public TextElement()
        {

        }

        private LayoutSettings GetLayoutSettings()
        {
            return new()
            {
                AreaSize = this.AreaSize,
                Color = this.Color,
                Origin = this.Position,
                Scale = this.Scale,
                WrapContent = this.WrapContent,
            };
        }

        private void BuildLayout()
        {
            if (this.SpriteFont == null || this.tokens == null)
            {
                return;
            }

            this.glyphLayout = MarkupProcessor.BuildLayout(this.textContent, this.tokens, this.SpriteFont, GetLayoutSettings());
            this.measuredText = this.glyphLayout.Bounds.Size;
        }

        internal void SetTextContent(string source)
        {
            if (this.textContent == source)
            {
                return;
            }

            this.textContent = source;

            this.tokens = Lexer.Parse(source);
            BuildLayout();
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (this.glyphLayout == null || this.SpriteFont == null)
            {
                return;
            }

            foreach (Glyph glyph in this.glyphLayout.Glyphs)
            {
                _ = this.charBuffer.Clear();
                _ = this.charBuffer.Append(glyph.Character);
                spriteBatch.DrawString(this.SpriteFont, this.charBuffer, glyph.Position, glyph.RenderState.Color, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f, false);
            }
        }

        protected override void RepositionRelativeToParent()
        {
            base.RepositionRelativeToParent();

            if (this.glyphLayout != null)
            {
                BuildLayout();
            }
        }

        public override void Reset()
        {
            _ = this.charBuffer.Clear();
            base.Reset();
        }
    }
}

