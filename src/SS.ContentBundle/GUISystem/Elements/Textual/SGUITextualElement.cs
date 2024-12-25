using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Textual
{
    internal abstract class SGUITextualElement : SGUIElement
    {
        internal SpriteFont SpriteFont { get; set; }
        internal string Content => this.contentStringBuilder.ToString();
        protected StringBuilder ContentStringBuilder => this.contentStringBuilder;

        internal bool HasBorders
        {
            get
            {
                foreach (SBorderSettings border in this.borderSettings.Values)
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

        private readonly Dictionary<SCardinalDirection, SBorderSettings> borderSettings = [];

        internal SGUITextualElement(ISGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
            this.ShouldUpdate = false;

            foreach (SCardinalDirection direction in Enum.GetValues(typeof(SCardinalDirection)))
            {
                this.borderSettings[direction] = new(false, SColorPalette.DarkGray, Vector2.Zero);
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

            foreach (KeyValuePair<SCardinalDirection, SBorderSettings> border in this.borderSettings)
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

        internal virtual SSize2F GetStringSize()
        {
            Vector2 measureString = this.SpriteFont.MeasureString(this.Content) * this.Scale;
            return new(measureString.X, measureString.Y);
        }

        internal void SetBorder(SCardinalDirection direction, bool isEnabled, Color color, Vector2 offset)
        {
            this.borderSettings[direction].IsEnabled = isEnabled;
            this.borderSettings[direction].Color = color;
            this.borderSettings[direction].Offset = offset;
        }

        internal void SetAllBorders(bool isEnabled, Color color, Vector2 offset)
        {
            foreach (SCardinalDirection key in this.borderSettings.Keys)
            {
                SetBorder(key, isEnabled, color, offset);
            }
        }

        private static Vector2 GetBorderOffset(SCardinalDirection direction)
        {
            return direction switch
            {
                SCardinalDirection.North => new(0, -1),
                SCardinalDirection.Northeast => new(1, -1),
                SCardinalDirection.East => new(1, 0),
                SCardinalDirection.Southeast => new(1, 1),
                SCardinalDirection.South => new(0, 1),
                SCardinalDirection.Southwest => new(-1, 1),
                SCardinalDirection.West => new(-1, 0),
                SCardinalDirection.Northwest => new(-1, -1),
                _ => Vector2.Zero,
            };
        }
    }
}
