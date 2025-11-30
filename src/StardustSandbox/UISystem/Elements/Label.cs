using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.UISystem.Elements.TextSystem;

namespace StardustSandbox.UISystem.Elements
{
    internal sealed class Label : UIElement
    {
        internal string TextContent
        {
            get => this.textContent;
            set
            {
                if (this.textContent != value)
                {
                    this.textContent = value;
                    this.textContentDirty = true;
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
        internal Vector2 MeasuredText
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
        }
        internal TextAlignment TextAlignment { get; set; }
        internal Color Color { get; set; }

        internal LabelBorderDirection BorderDirections { get; set; }
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
            this.TextAlignment = TextAlignment.Left;
            this.Color = Color.White;
            this.textContentDirty = true;

            this.BorderDirections = LabelBorderDirection.None;
            this.BorderThickness = 2.0f;
            this.BorderOffset = 2.0f;
        }

        internal override void Initialize()
        {
            base.Initialize();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void DrawBorders(SpriteBatch spriteBatch, Vector2 position, Vector2 origin)
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
                        spriteBatch.DrawString(this.spriteFont, this.textContent, position + offset, this.BorderColor, 0.0f, origin, this.Scale, SpriteEffects.None, 0.0f);
                    }
                }
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = this.Position;
            Vector2 origin = Vector2.Zero;
            Vector2 measured = this.MeasuredText;

            if (this.TextAlignment == TextAlignment.Center)
            {
                origin = new Vector2(measured.X * 0.5f, measured.Y * 0.5f);
            }
            else if (this.TextAlignment == TextAlignment.Right)
            {
                origin = new Vector2(measured.X, 0.0f);
            }

            // Draw borders
            DrawBorders(spriteBatch, position, origin);

            // Draw main text
            spriteBatch.DrawString(this.spriteFont, this.textContent, position, this.Color, 0.0f, origin, this.Scale, SpriteEffects.None, 0.0f);

            base.Draw(spriteBatch);
        }

        private Vector2 MeasureText()
        {
            return string.IsNullOrEmpty(this.textContent) ? Vector2.Zero : this.spriteFont.MeasureString(this.textContent) * this.Scale;
        }
    }
}
