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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.UI.Text.LexicalAnalyzer;
using StardustSandbox.Core.UI.Text.LexicalAnalyzer;
using StardustSandbox.Core.UI.Text.Tags;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.UI.Text.Processors
{
    internal static class MarkupProcessor
    {
        private static bool TryWrap(SpriteFont.Glyph glyph, ProcessingContext context)
        {
            if (!context.LayoutSettings.WrapContent)
            {
                return true;
            }

            Vector2 scale = context.LayoutSettings.Scale;

            float nextX = context.Position.X;

            if (context.IsFirstGlyphOfLine)
            {
                nextX = Math.Max(glyph.LeftSideBearing, 0) * scale.X;
            }
            else
            {
                nextX += glyph.LeftSideBearing * scale.X;
            }

            nextX += glyph.Width * scale.X;
            nextX += Math.Max(glyph.RightSideBearing, 0) * scale.X;

            if (nextX <= context.LayoutSettings.AreaSize.X)
            {
                return true;
            }

            context.NextLine();

            float nextBottom = context.Position.Y + context.DefaultLineHeight;

            if (nextBottom > context.LayoutSettings.AreaSize.Y)
            {
                context.IsTruncated = true;
                return false;
            }

            return true;
        }

        private static bool TryEmitGlyph(char character, ProcessingContext context, out Glyph emittedGlyph)
        {
            emittedGlyph = default;

            // If the context is truncated, we cannot emit any more glyphs.
            if (context.IsTruncated)
            {
                return false;
            }

            SpriteFont.Glyph glyph = context.SpriteFont.Glyphs[character];

            // If the glyph is not found, we cannot emit it.
            if (!TryWrap(glyph, context))
            {
                return false;
            }

            // If the glyph is found, we can emit it.
            // So, proceed to calculate its position and update the context accordingly.
            Vector2 position = context.Position;
            Vector2 scale = context.LayoutSettings.Scale;

            if (context.IsFirstGlyphOfLine)
            {
                position.X = Math.Max(glyph.LeftSideBearing, 0) * scale.X;
                context.IsFirstGlyphOfLine = false;
            }
            else
            {
                position.X += glyph.LeftSideBearing * scale.X;
            }

            Vector2 glyphPosition = new(
                position.X + (glyph.Cropping.X * scale.X),
                position.Y
            );

            emittedGlyph = new()
            {
                Character = character,
                Position = glyphPosition,
                RenderState = context.GetRenderState(),
            };

            position.X += glyph.Width * scale.X;
            position.X += glyph.RightSideBearing * scale.X;

            context.Position = position;
            context.CurrentLineHeight = Math.Max(context.CurrentLineHeight, glyph.Cropping.Height * scale.Y);

            Vector2 glyphSize = new(
                glyph.BoundsInTexture.Width * scale.X,
                glyph.BoundsInTexture.Height * scale.Y
            );

            context.ExpandBounds(glyphPosition, glyphSize);

            return true;
        }

        private static void EmitGlyphs(ReadOnlySpan<char> source, Token token, ProcessingContext context, Action<Glyph> onGlyphEmitted)
        {
            foreach (char character in source.Slice(token.StartIndex, token.Length))
            {
                if (!TryEmitGlyph(character, context, out Glyph emittedGlyph))
                {
                    break;
                }

                onGlyphEmitted?.Invoke(emittedGlyph);
            }
        }

        private static void ProcessTag(ReadOnlySpan<char> source, Token token, bool open, bool selfClosing, ProcessingContext context)
        {
            ReadOnlySpan<char> tagSource = source.Slice(token.StartIndex, token.Length);
            ReadOnlySpan<char> tagNameSource = default;
            ReadOnlySpan<char> tagParametersSource = default;
            Span<Range> parameterRanges = default;

            if (token.TagHasName)
            {
                tagNameSource = tagSource.Slice(token.TagNameStartIndex, token.TagNameLength);
            }

            if (token.TagHasParameters)
            {
                tagParametersSource = tagSource.Slice(token.TagParameterStartIndex, token.TagParameterLength);

                int parameterCount = tagParametersSource.Count(LexerConstants.TAG_PARAMETER_VALUE_SEPARATOR) + 1;
                parameterRanges = new Range[parameterCount];

                _ = tagParametersSource.Split(parameterRanges, LexerConstants.TAG_PARAMETER_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            if (!TagRegistry.TryGet(tagNameSource, out Tag tag))
            {
                return;
            }

            if (selfClosing)
            {
                tag.OnSelfClosing?.Invoke(tagParametersSource, parameterRanges, context);
            }
            else if (open)
            {
                tag.OnOpen?.Invoke(tagParametersSource, parameterRanges, context);
            }
            else
            {
                tag.OnClose?.Invoke(tagParametersSource, parameterRanges, context);
            }
        }

        internal static GlyphLayout BuildLayout(ReadOnlySpan<char> source, IEnumerable<Token> tokens, SpriteFont spriteFont, LayoutSettings layoutSettings)
        {
            List<Glyph> glyphs = [];
            ProcessingContext context = new(layoutSettings, spriteFont);

            foreach (Token token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Letters:
                    case TokenType.Digits:
                    case TokenType.Punctuation:
                    case TokenType.Symbol:
                    case TokenType.Whitespace:
                        EmitGlyphs(source, token, context, glyphs.Add);
                        break;

                    case TokenType.OpenTag:
                        ProcessTag(source, token, true, false, context);
                        break;

                    case TokenType.CloseTag:
                        ProcessTag(source, token, false, false, context);
                        break;

                    case TokenType.SelfClosingTag:
                        ProcessTag(source, token, false, true, context);
                        break;

                    default:
                        break;
                }
            }

            return new(glyphs, context.Bounds);
        }
    }
}
