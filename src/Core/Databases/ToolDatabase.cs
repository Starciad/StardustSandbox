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
using StardustSandbox.Core.Enums.Tools;
using StardustSandbox.Core.Tools;
using StardustSandbox.Core.Tools.Environment;
using StardustSandbox.Core.Tools.Inks;

using System;

namespace StardustSandbox.Core.Databases
{
    internal static class ToolDatabase
    {
        private static Tool[] tools;
        private static bool isLoaded;

        internal static void Load()
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(ToolDatabase)} is already loaded.");
            }

            tools = [
                // [000] Heat Tool
                new HeatTool()
                {
                    Index = ToolIndex.HeatTool
                },

                // [001] Freeze Tool
                new FreezeTool()
                {
                    Index = ToolIndex.FreezeTool,
                },

                // [002] Ink Tool (Black)
                new InkTool()
                {
                    Index = ToolIndex.BlackInkTool,
                    InkColor = AAP64ColorPalette.DarkGray,
                },

                // [003] Ink Tool (White)
                new InkTool()
                {
                    Index = ToolIndex.WhiteInkTool,
                    InkColor = AAP64ColorPalette.White,
                },

                // [004] Ink Tool (Red)
                new InkTool()
                {
                    Index = ToolIndex.RedInkTool,
                    InkColor = AAP64ColorPalette.Crimson,
                },

                // [005] Ink Tool (Orange)
                new InkTool()
                {
                    Index = ToolIndex.OrangeInkTool,
                    InkColor = AAP64ColorPalette.Orange,
                },

                // [006] Ink Tool (Yellow)
                new InkTool()
                {
                    Index = ToolIndex.YellowInkTool,
                    InkColor = AAP64ColorPalette.Gold,
                },

                // [007] Ink Tool (Green)
                new InkTool()
                {
                    Index = ToolIndex.GreenInkTool,
                    InkColor = AAP64ColorPalette.GrassGreen,
                },

                // [008] Ink Tool (Blue)
                new InkTool()
                {
                    Index = ToolIndex.BlueInkTool,
                    InkColor = AAP64ColorPalette.RoyalBlue,
                },

                // [009] Ink Tool (Gray)
                new InkTool()
                {
                    Index = ToolIndex.GrayInkTool,
                    InkColor = AAP64ColorPalette.Slate,
                },

                // [010] Ink Tool (Violet)
                new InkTool()
                {
                    Index = ToolIndex.VioletInkTool,
                    InkColor = AAP64ColorPalette.Violet,
                },

                // [011] Ink Tool (Brown)
                new InkTool()
                {
                    Index = ToolIndex.BrownInkTool,
                    InkColor = AAP64ColorPalette.Brown,
                },
            ];

            isLoaded = true;
        }

        internal static Tool GetTool(ToolIndex toolIndex)
        {
            return tools[(int)toolIndex];
        }
    }
}
