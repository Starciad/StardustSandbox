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

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class Text : UIElement
    {
        private enum TextStyle : byte
        {
            Regular,
            Bold,
            Italic,
            BoldAndItalic
        }

        private enum TokenType : byte
        {
            Text,
            Action
        }

        private sealed class TextToken(TokenType type, string content)
        {
            internal TokenType Type => type;
            internal string Content => content;
        }

        private sealed class TextFragment
        {
            internal string Content { get; set; }
            internal Vector2 Position { get; set; }
            internal Vector2 Scale { get; set; }
            internal Color Color { get; set; }
            internal TextStyle Style { get; set; }
        }

        private sealed class TextFragmentContext(Vector2 origin, float lneHeight)
        {
            internal Vector2 Origin => origin;
            internal Vector2 Position { get; set; }
            internal Vector2 Scale { get; set; }
            internal Color Color { get; set; }
            internal float LineHeight => lneHeight;
            internal TextStyle Style { get; set; }
        }

        internal SpriteFont SpriteFont { get; set; }
        internal override Vector2 Size
        {
            get => this.measuredText;
            set => throw new InvalidOperationException("Cannot set Size of Text directly. Size is determined by the text content and wrapping.");
        }

        internal string TextContent
        {
            get => this.textContent;

            set
            {
                // If the new value is null or empty, clear the text content and reset the text fragments and tokens.
                if (string.IsNullOrEmpty(value))
                {
                    this.textContent = string.Empty;
                    ClearTextContent();
                    return;
                }

                // If the new value is the same as the current text content, do nothing.
                if (!string.IsNullOrEmpty(this.textContent) && this.textContent.Equals(value))
                {
                    return;
                }

                // If the new value is different, update the text content and rebuild the text fragments and tokens.
                this.textContent = value;
                RebuildText(value);
            }
        }

        internal bool WrapText { get; set; }

        internal Vector2 TextAreaSize { get; set; }
        internal float LineHeight { get; set; } = 2.5f;
        internal float WordSpacing { get; set; } = 26.0f;
        internal float CharacterSpacing { get; set; } = 0.0f;
        internal TextBorderSettings BorderSettings { get; set; }

        private string textContent;
        private Vector2 measuredText;

        private readonly List<TextFragment> textFragments = [];
        private readonly List<TextToken> textTokens = [];

        private static readonly Dictionary<string, Action<TextFragmentContext, string[]>> textActions = new()
        {
            ["BreakLine"] = (context, parameters) =>
            {
                context.Position = new(context.Origin.X, context.Position.Y + context.Scale.Y * context.LineHeight);
            },

            ["SetColor"] = (context, parameters) =>
            {
                if (parameters.Length != 4)
                {
                    throw new ArgumentException("SetColor action requires 4 parameters: R, G, B, A.");
                }

                if (!byte.TryParse(parameters[0], out byte r) ||
                    !byte.TryParse(parameters[1], out byte g) ||
                    !byte.TryParse(parameters[2], out byte b) ||
                    !byte.TryParse(parameters[3], out byte a))
                {
                    throw new ArgumentException("SetColor action parameters must be valid byte values (0-255).");
                }

                context.Color = new(r, g, b, a);
            },

            ["ResetColor"] = (context, parameters) =>
            {
                context.Color = Color.White;
            }
        };

        public Text()
        {

        }

        private void BuildTextTokens(string value)
        {
            string[] tokens = value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            int length = tokens.Length;

            for (int i = 0; i < length; i++)
            {
                string token = tokens[i];

                if (token.StartsWith('[') && token.EndsWith(']'))
                {
                    this.textTokens.Add(new(TokenType.Action, token[1..^1]));
                    continue;
                }

                this.textTokens.Add(new(TokenType.Text, token));
            }
        }

        private void AppendTextFragment(TextFragmentContext context, string content)
        {
            this.textFragments.Add(new()
            {
                Content = content,
                Position = context.Position,
                Scale = context.Scale,
                Color = context.Color,
                Style = context.Style
            });

            float offsetX = this.SpriteFont.MeasureString(content).X * context.Scale.X + this.WordSpacing;
            float offsetY = this.LineHeight * this.SpriteFont.LineSpacing * context.Scale.Y;

            Vector2 currentPosition = context.Position;
            Vector2 nextPosition = new(currentPosition.X + offsetX, currentPosition.Y);

            // Wrap Text if Necessary
            if (this.WrapText && nextPosition.X > this.Position.X + this.TextAreaSize.X)
            {
                nextPosition = new(context.Origin.X, currentPosition.Y + offsetY);
            }

            context.Position = nextPosition;

            // Based on the current position and the next position,
            // update the measured text size to ensure it encompasses
            // all text fragments.
            this.measuredText = new(
                Math.Max(this.measuredText.X, nextPosition.X - context.Origin.X),
                Math.Max(this.measuredText.Y, nextPosition.Y - context.Origin.Y + offsetY)
            );
        }

        private static void PerformTextAction(TextFragmentContext context, string name, string[] parameters)
        {
            if (textActions.TryGetValue(name, out Action<TextFragmentContext, string[]> action))
            {
                action(context, parameters);
                return;
            }

            throw new InvalidOperationException($"Unknown text action: {name}");
        }

        private void ProcessTextTokens(TextFragmentContext context, TextToken token)
        {
            switch (token.Type)
            {
                case TokenType.Text:
                    AppendTextFragment(context, token.Content);
                    break;

                case TokenType.Action:
                    string[] actionTokens = token.Content.Split(':', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    string actionName = actionTokens[0];
                    string[] actionParameters = actionTokens.Length > 1 ? actionTokens[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) : [];

                    PerformTextAction(context, actionName, actionParameters);
                    break;

                default:
                    break;
            }
        }

        private void BuildTextFragments()
        {
            Vector2 textOriginPosition = this.Position;

            TextFragmentContext context = new(textOriginPosition, this.LineHeight)
            {
                Color = Color.White,
                Position = textOriginPosition,
                Scale = this.Scale,
                Style = TextStyle.Regular
            };

            foreach (TextToken token in this.textTokens)
            {
                ProcessTextTokens(context, token);
            }
        }

        private void ClearTextContent()
        {
            this.textFragments.Clear();
            this.textTokens.Clear();
        }

        private void RebuildText(string value)
        {
            ClearTextContent();

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            BuildTextTokens(value);
            BuildTextFragments();
        }

        protected override void RepositionRelativeToParent()
        {
            base.RepositionRelativeToParent();
            RebuildText(this.textContent);
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            foreach (TextFragment fragment in this.textFragments)
            {
                spriteBatch.DrawString(this.SpriteFont, fragment.Content, fragment.Position, fragment.Color, 0f, Vector2.Zero, fragment.Scale, SpriteEffects.None, 0f);
            }
        }
    }
}

