using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.UI;
using StardustSandbox.UI.Elements.TextSystem;

using System;

namespace StardustSandbox.UI.Elements
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
                if (!this.textContent.Equals(value))
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
