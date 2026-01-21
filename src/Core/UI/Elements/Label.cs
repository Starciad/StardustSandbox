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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.UI.Elements.TextSystem;

using System;

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class Label : UIElement
    {
        internal override Vector2 Size
        {
            get
            {
                if (this.textContentDirty)
                {
                    this.measuredText = MeasureText();
                    this.textContentDirty = false;
                }

                return this.measuredText;
            }

            set => throw new InvalidOperationException("Cannot set Size of Label directly. Size is determined by the text content.");
        }

        internal string TextContent
        {
            get => this.textContent;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !this.textContent.Equals(value))
                {
                    this.textContent = value;
                    this.textContentDirty = true;

                    RepositionRelativeToParent();
                }
            }
        }

        internal SpriteFontIndex SpriteFontIndex
        {
            get => this.spriteFontIndex;
            set
            {
                this.spriteFontIndex = value;
                this.spriteFont = AssetDatabase.GetSpriteFont(value);
            }
        }

        internal Color Color { get; set; }

        internal LabelBorderDirection BorderDirections { get; set; } = LabelBorderDirection.None;
        internal float BorderThickness { get; set; }
        internal float BorderOffset { get; set; }
        internal Color BorderColor { get; set; }

        private SpriteFontIndex spriteFontIndex;
        private SpriteFont spriteFont;

        private string textContent;

        private Vector2 measuredText;
        private bool textContentDirty;

        internal Label()
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

        protected override void OnUpdate(GameTime gameTime)
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
                Vector2 position = this.Position;

                // Draw borders
                DrawBorders(spriteBatch, position);

                // Draw main text
                spriteBatch.DrawString(this.spriteFont, this.textContent, position, this.Color, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
            }
        }

        private Vector2 MeasureText()
        {
            return string.IsNullOrEmpty(this.textContent) ? Vector2.Zero : this.spriteFont.MeasureString(this.textContent) * this.Scale;
        }
    }
}

