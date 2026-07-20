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

using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.UI.Text.Processors
{
    internal class ProcessingContext
    {
        internal LayoutSettings LayoutSettings { get; }
        internal SpriteFont SpriteFont { get; }
        internal Vector2 Position { get; set; }

        internal RectangleF Bounds { get; private set; }
        public bool IsFirstGlyphOfLine { get; set; }
        public bool IsTruncated { get; set; }
        public float CurrentLineHeight { get; set; }
        public float DefaultLineHeight => this.SpriteFont.LineSpacing * this.LayoutSettings.Scale.Y;

        private readonly Stack<RenderState> renderStateStack = [];

        public ProcessingContext(LayoutSettings layoutSettings, SpriteFont spriteFont)
        {
            this.LayoutSettings = layoutSettings;
            this.SpriteFont = spriteFont;
            this.Position = layoutSettings.Origin;
            this.IsFirstGlyphOfLine = true;

            PushRenderState(new()
            {
                Bold = false,
                Color = layoutSettings.Color,
                Italic = false,
            });
        }

        internal void ExpandBounds(Vector2 position, Vector2 size)
        {
            if (this.Bounds == RectangleF.Empty)
            {
                this.Bounds = new(position, size);
                return;
            }

            float minX = Math.Min(this.Bounds.Left, position.X);
            float minY = Math.Min(this.Bounds.Top, position.Y);
            float maxX = Math.Max(this.Bounds.Right, position.X + size.X);
            float maxY = Math.Max(this.Bounds.Bottom, position.Y + size.Y);
            this.Bounds = new(minX, minY, maxX - minX, maxY - minY);
        }

        internal RenderState GetRenderState()
        {
            return this.renderStateStack.Peek();
        }

        internal void PushRenderState(RenderState value)
        {
            this.renderStateStack.Push(value);
        }

        internal void PopRenderState()
        {
            if (this.renderStateStack.Count > 1)
            {
                _ = this.renderStateStack.Pop();
            }
        }

        internal void NextLine()
        {
            this.Position = new(this.LayoutSettings.Origin.X, this.Position.Y + this.CurrentLineHeight);
            this.CurrentLineHeight = this.DefaultLineHeight;
            this.IsFirstGlyphOfLine = true;
        }
    }
}
