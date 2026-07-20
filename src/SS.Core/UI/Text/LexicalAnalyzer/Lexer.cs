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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.UI.Text.LexicalAnalyzer;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.UI.Text.LexicalAnalyzer
{
    internal static class Lexer
    {
        private static Token ParseDigits(ReadOnlySpan<char> source, int sourceLength, ref int currentIndex)
        {
            int startIndex = currentIndex;
            int i = startIndex;

            while (i < sourceLength && char.IsDigit(source[i]))
            {
                i++;
            }

            currentIndex = i;
            return new(TokenType.Digits, startIndex, i);
        }

        private static Token ParseLetters(ReadOnlySpan<char> source, int sourceLength, ref int currentIndex)
        {
            int startIndex = currentIndex;
            int i = startIndex;

            while (i < sourceLength && char.IsLetter(source[i]))
            {
                i++;
            }

            currentIndex = i;
            return new(TokenType.Letters, startIndex, i);
        }

        private static Token ParseSymbol(ref int currentIndex)
        {
            int startIndex = currentIndex;
            currentIndex++;
            return new(TokenType.Symbol, startIndex, currentIndex);
        }

        private static Token ParsePunctuation(ref int currentIndex)
        {
            int startIndex = currentIndex;
            currentIndex++;
            return new(TokenType.Punctuation, startIndex, currentIndex);
        }

        private static Token ParseTag(ReadOnlySpan<char> source, int sourceLength, ref int currentIndex)
        {
            int startIndex = currentIndex;
            int tagEndOffset = source[startIndex..sourceLength].IndexOf(LexerConstants.TAG_END);

            if (tagEndOffset != -1)
            {
                currentIndex += tagEndOffset + 1; // Includes the closing '>'
            }
            else
            {
                currentIndex = sourceLength;
            }

            ReadOnlySpan<char> tagSource = source[startIndex..currentIndex];
            TokenType tokenType;

            if (tagSource.StartsWith(LexerConstants.CLOSING_TAG_START))
            {
                tokenType = TokenType.CloseTag;
            }
            else
            {
                tokenType = tagSource.EndsWith(LexerConstants.SELF_CLOSING_TAG_END) ? TokenType.SelfClosingTag : TokenType.OpenTag;
            }

            int tagNameStartIndex = -1;
            int tagNameEndIndex = tagSource.IndexOfAny(LexerConstants.TAG_NAME_ENDING_CHARACTERS);
            int tagParameterIndex = tagSource.IndexOf(LexerConstants.TAG_PARAMETER_SEPARATOR);

            switch (tokenType)
            {
                case TokenType.OpenTag:
                    tagNameStartIndex = 1;
                    tagNameEndIndex--;
                    break;

                case TokenType.CloseTag:
                    tagNameStartIndex = 2;
                    tagNameEndIndex -= 2;
                    break;

                case TokenType.SelfClosingTag:
                    tagNameStartIndex = 1;
                    tagNameEndIndex -= 2;
                    break;

                default:
                    break;
            }

            int tagParameterStartIndex = -1;
            int tagParameterEndIndex = -1;

            if (tagParameterIndex != -1)
            {
                tagParameterStartIndex = tagParameterIndex + 1; // Exclude the ':'

                switch (tokenType)
                {
                    case TokenType.OpenTag:
                        tagParameterEndIndex = tagSource.IndexOfAny(LexerConstants.TAG_PARAMETERS_ENDING_CHARACTERS) - tagParameterStartIndex; // Exclude the '>'
                        break;
                    case TokenType.SelfClosingTag:
                    case TokenType.CloseTag:
                        tagParameterEndIndex = tagSource.IndexOfAny(LexerConstants.TAG_PARAMETERS_ENDING_CHARACTERS) - tagParameterStartIndex - 1; // Exclude the '/>'
                        break;
                    default:
                        break;
                }
            }

            return new(tokenType, startIndex, currentIndex)
            {
                TagHasName = tagNameStartIndex != -1 && tagNameEndIndex != -1,
                TagHasParameters = tagParameterStartIndex != -1 && tagParameterEndIndex != -1,
                TagNameStartIndex = tagNameStartIndex,
                TagNameEndIndex = tagNameEndIndex != -1 ? tagNameStartIndex + tagNameEndIndex : currentIndex,
                TagParameterStartIndex = tagParameterStartIndex,
                TagParameterEndIndex = tagParameterEndIndex != -1 ? tagParameterStartIndex + tagParameterEndIndex : currentIndex
            };
        }

        private static Token ParseWhiteSpace(ReadOnlySpan<char> source, int sourceLength, ref int currentIndex)
        {
            int startIndex = currentIndex;
            int i = startIndex;

            while (i < sourceLength && char.IsWhiteSpace(source[i]))
            {
                i++;
            }

            currentIndex = i;
            return new(TokenType.Whitespace, startIndex, i);
        }

        private static bool TryParseToken(ReadOnlySpan<char> source, int sourceLength, ref int currentIndex, out Token token)
        {
            char currentCharacter = source[currentIndex];

            // Tag (Opening, Closing, and Self-Closing)
            if (currentCharacter == LexerConstants.TAG_START)
            {
                token = ParseTag(source, sourceLength, ref currentIndex);
                return true;
            }

            // White Space
            if (char.IsWhiteSpace(currentCharacter))
            {
                token = ParseWhiteSpace(source, sourceLength, ref currentIndex);
                return true;
            }

            // Punctuation
            if (char.IsPunctuation(currentCharacter))
            {
                token = ParsePunctuation(ref currentIndex);
                return true;
            }

            // Symbol
            if (char.IsSymbol(currentCharacter))
            {
                token = ParseSymbol(ref currentIndex);
                return true;
            }

            // Letters
            if (char.IsLetter(currentCharacter))
            {
                token = ParseLetters(source, sourceLength, ref currentIndex);
                return true;
            }

            // Digits
            if (char.IsDigit(currentCharacter))
            {
                token = ParseDigits(source, sourceLength, ref currentIndex);
                return true;
            }

            currentIndex++;
            token = default;
            return false;
        }

        internal static IEnumerable<Token> Parse(string source)
        {
            int currentIndex = 0;
            int sourceLength = source.Length;

            while (currentIndex < sourceLength)
            {
                if (TryParseToken(source, sourceLength, ref currentIndex, out Token token))
                {
                    yield return token;
                }
            }
        }
    }
}
