using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Textual
{
    public abstract class SGUITextualElement : SGUIElement
    {
        public SpriteFont SpriteFont { get; set; }
        public string Content => this.contentStringBuilder.ToString();
        protected StringBuilder ContentStringBuilder => this.contentStringBuilder;

        public bool HasBorders
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

        private readonly StringBuilder contentStringBuilder = new();
        private readonly Dictionary<SCardinalDirection, SBorderSettings> borderSettings = [];

        public SGUITextualElement(ISGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
            this.ShouldUpdate = false;

            foreach (SCardinalDirection direction in Enum.GetValues(typeof(SCardinalDirection)))
            {
                this.borderSettings[direction] = new(false, Color.Black, Vector2.Zero);
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

        public virtual void SetTextualContent(string value)
        {
            ClearTextualContent();
            _ = this.contentStringBuilder.Append(value);
        }

        public virtual void ClearTextualContent()
        {
            _ = this.contentStringBuilder.Clear();
        }

        public SSize2F GetStringSize()
        {
            Vector2 measureString = this.SpriteFont.MeasureString(this.Content) * this.Scale / 2f;
            return new SSize2F(measureString.X, measureString.Y);
        }

        public void SetBorder(SCardinalDirection direction, bool isEnabled, Color color, Vector2 offset)
        {
            this.borderSettings[direction].IsEnabled = isEnabled;
            this.borderSettings[direction].Color = color;
            this.borderSettings[direction].Offset = offset;
        }

        public void SetAllBorders(bool isEnabled, Color color, Vector2 offset)
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
                SCardinalDirection.North => new Vector2(0, -1),
                SCardinalDirection.Northeast => new Vector2(1, -1),
                SCardinalDirection.East => new Vector2(1, 0),
                SCardinalDirection.Southeast => new Vector2(1, 1),
                SCardinalDirection.South => new Vector2(0, 1),
                SCardinalDirection.Southwest => new Vector2(-1, 1),
                SCardinalDirection.West => new Vector2(-1, 0),
                SCardinalDirection.Northwest => new Vector2(-1, -1),
                _ => Vector2.Zero,
            };
        }
    }
}
