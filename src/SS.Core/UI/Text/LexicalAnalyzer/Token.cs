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

using StardustSandbox.Core.Enums.UI.Text.LexicalAnalyzer;

namespace StardustSandbox.Core.UI.Text.LexicalAnalyzer
{
    internal readonly struct Token(TokenType type, int startIndex, int endIndex)
    {
        // GENERAL
        internal TokenType Type => type;
        internal int StartIndex => startIndex;
        internal int EndIndex => endIndex;

        // TAGS
        internal bool TagHasName { get; init; }
        internal bool TagHasParameters { get; init; }
        internal int TagNameStartIndex { get; init; }
        internal int TagNameEndIndex { get; init; }
        internal int TagParameterStartIndex { get; init; }
        internal int TagParameterEndIndex { get; init; }

        // LENGTHS
        internal int Length => endIndex - startIndex;
        internal int TagNameLength => this.TagNameEndIndex - this.TagNameStartIndex;
        internal int TagParameterLength => this.TagParameterEndIndex - this.TagParameterStartIndex;
    }
}
