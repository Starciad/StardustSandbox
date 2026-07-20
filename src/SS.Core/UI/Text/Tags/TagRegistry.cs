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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.UI.Text.Processors;

using System;

namespace StardustSandbox.Core.UI.Text.Tags
{
    internal static class TagRegistry
    {
        private static void OnTagClose(ReadOnlySpan<char> parameters, Span<Range> parameterRangers, ProcessingContext context)
        {
            context.PopRenderState();
        }

        private static void OnBoldOpen(ReadOnlySpan<char> parameters, Span<Range> parameterRangers, ProcessingContext context)
        {
            RenderState rs = context.GetRenderState();
            rs.Bold = true;
            context.PushRenderState(rs);
        }

        private static void OnItalicOpen(ReadOnlySpan<char> parameters, Span<Range> parameterRangers, ProcessingContext context)
        {
            RenderState rs = context.GetRenderState();
            rs.Italic = true;
            context.PushRenderState(rs);
        }

        private static void OnColorOpen(ReadOnlySpan<char> parameters, Span<Range> parameterRangers, ProcessingContext context)
        {
            ReadOnlySpan<char> colorParameter = parameters[parameterRangers[0]];

            RenderState rs = context.GetRenderState();
            rs.Color = AAP64ColorPalette.GetColorByName(colorParameter);
            context.PushRenderState(rs);
        }

        private static void OnBreakLineOpen(ReadOnlySpan<char> parameters, Span<Range> parameterRangers, ProcessingContext context)
        {
            context.NextLine();
        }

        private static readonly Tag[] tags =
        [
            new("bold", "b")
            {
                OnOpen = OnBoldOpen,
                OnClose = OnTagClose
            },

            new("italic", "i")
            {
                OnOpen = OnItalicOpen,
                OnClose = OnTagClose
            },

            new("color", "c")
            {
                OnOpen = OnColorOpen,
                OnClose = OnTagClose
            },

            new("breakline", "br")
            {
                OnSelfClosing = OnBreakLineOpen
            },
        ];

        internal static bool TryGet(ReadOnlySpan<char> name, out Tag tag)
        {
            foreach (Tag t in tags)
            {
                if (name.Equals(t.Name, StringComparison.OrdinalIgnoreCase))
                {
                    tag = t;
                    return true;
                }

                foreach (string alias in t.Aliases)
                {
                    if (name.Equals(alias, StringComparison.OrdinalIgnoreCase))
                    {
                        tag = t;
                        return true;
                    }
                }
            }

            tag = null;
            return false;
        }
    }
}
