using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Enums.Directions;

using System;
using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.UISystem.Elements.Textual
{
    internal abstract class TextualUIElement : UIElement
    {
        internal SpriteFont SpriteFont { get; set; }
        internal string Content => this.contentStringBuilder.ToString();
        protected StringBuilder ContentStringBuilder => this.contentStringBuilder;

        internal bool HasBorders
        {
            get
            {
                foreach (BorderSettings border in this.borderSettings.Values)
                {
                    if (border.IsEnabled)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private StringBuilder contentStringBuilder = new();

        private readonly Dictionary<CardinalDirection, BorderSettings> borderSettings = [];

        internal TextualUIElement()
        {
            this.IsVisible = true;
            this.ShouldUpdate = false;

            foreach (CardinalDirection direction in Enum.GetValues(typeof(CardinalDirection)))
            {
                this.borderSettings[direction] = new(false, AAP64ColorPalette.DarkGray, Vector2.Zero);
            }
        }

        protected void DrawBorders(SpriteBatch spriteBatch, StringBuilder text, Vector2 position, SpriteFont spriteFont, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
        {
            DrawBorders(spriteBatch, text.ToString(), position, spriteFont, rotation, origin, scale, effects);
        }

        protected void DrawBorders(SpriteBatch spriteBatch, string text, Vector2 position, SpriteFont spriteFont, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
        {
            if (!this.HasBorders)
            {
                return;
            }

            foreach (KeyValuePair<CardinalDirection, BorderSettings> border in this.borderSettings)
            {
                if (border.Value.IsEnabled)
                {
                    Vector2 offset = GetBorderOffset(border.Key) * border.Value.Offset;
                    spriteBatch.DrawString(spriteFont, text, position + offset, border.Value.Color, rotation, origin, scale, effects, 0f);
                }
            }
        }

        internal virtual void SetTextualContent(string value)
        {
            ClearTextualContent();
            _ = this.contentStringBuilder.Append(value);
        }

        internal virtual void SetTextualContent(StringBuilder value)
        {
            this.contentStringBuilder = value;
        }

        internal virtual void ClearTextualContent()
        {
            _ = this.contentStringBuilder.Clear();
        }

        internal virtual Vector2 GetStringSize()
        {
            Vector2 measureString = this.SpriteFont.MeasureString(this.Content) * this.Scale;
            return new(measureString.X, measureString.Y);
        }

        internal void SetBorder(CardinalDirection direction, bool isEnabled, Color color, Vector2 offset)
        {
            this.borderSettings[direction].IsEnabled = isEnabled;
            this.borderSettings[direction].Color = color;
            this.borderSettings[direction].Offset = offset;
        }

        internal void SetAllBorders(bool isEnabled, Color color, Vector2 offset)
        {
            foreach (CardinalDirection key in this.borderSettings.Keys)
            {
                SetBorder(key, isEnabled, color, offset);
            }
        }

        private static Vector2 GetBorderOffset(CardinalDirection direction)
        {
            return direction switch
            {
                CardinalDirection.North => new(0, -1),
                CardinalDirection.Northeast => new(1, -1),
                CardinalDirection.East => new(1, 0),
                CardinalDirection.Southeast => new(1, 1),
                CardinalDirection.South => new(0, 1),
                CardinalDirection.Southwest => new(-1, 1),
                CardinalDirection.West => new(-1, 0),
                CardinalDirection.Northwest => new(-1, -1),
                _ => Vector2.Zero,
            };
        }
    }
}
