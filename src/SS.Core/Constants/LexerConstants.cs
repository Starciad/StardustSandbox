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

namespace StardustSandbox.Core.Constants
{
    internal static class LexerConstants
    {
        internal const char TAG_START = '<';
        internal const char TAG_END = '>';
        internal const char TAG_SLASH = '/';
        internal const char TAG_PARAMETER_SEPARATOR = ':';
        internal const char TAG_PARAMETER_VALUE_SEPARATOR = ',';

        internal const string SELF_CLOSING_TAG_END = "/>";
        internal const string CLOSING_TAG_START = "</";

        internal static readonly char[] TAG_NAME_ENDING_CHARACTERS = [TAG_PARAMETER_SEPARATOR, TAG_END];
        internal static readonly char[] TAG_PARAMETERS_ENDING_CHARACTERS = [TAG_END];
    }
}
