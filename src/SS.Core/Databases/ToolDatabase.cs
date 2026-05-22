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
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Tools;
using StardustSandbox.Core.Tools.Environment;
using StardustSandbox.Core.Tools.Inks;

namespace StardustSandbox.Core.Databases
{
    internal sealed class ToolDatabase
    {
        private Tool[] tools;

        internal void Load(GameEvents gameEvents)
        {
            this.tools = [
                // [000] Heat Tool
                new HeatTool(ToolIndex.HeatTool, gameEvents),

                // [001] Freeze Tool
                new FreezeTool(ToolIndex.FreezeTool, gameEvents),

                // [002] Ink Tool (Black)
                new InkTool(ToolIndex.BlackInkTool, AAP64ColorPalette.DarkGray, gameEvents),

                // [003] Ink Tool (White)
                new InkTool(ToolIndex.WhiteInkTool, AAP64ColorPalette.White, gameEvents),

                // [004] Ink Tool (Red)
                new InkTool(ToolIndex.RedInkTool, AAP64ColorPalette.Crimson, gameEvents),

                // [005] Ink Tool (Orange)
                new InkTool(ToolIndex.OrangeInkTool, AAP64ColorPalette.Orange, gameEvents),

                // [006] Ink Tool (Yellow)
                new InkTool(ToolIndex.YellowInkTool, AAP64ColorPalette.Gold, gameEvents),

                // [007] Ink Tool (Green)
                new InkTool(ToolIndex.GreenInkTool, AAP64ColorPalette.GrassGreen, gameEvents),

                // [008] Ink Tool (Blue)
                new InkTool(ToolIndex.BlueInkTool, AAP64ColorPalette.RoyalBlue, gameEvents),

                // [009] Ink Tool (Gray)
                new InkTool(ToolIndex.GrayInkTool, AAP64ColorPalette.Slate, gameEvents),

                // [010] Ink Tool (Violet)
                new InkTool(ToolIndex.VioletInkTool, AAP64ColorPalette.Violet, gameEvents),

                // [011] Ink Tool (Brown)
                new InkTool(ToolIndex.BrownInkTool, AAP64ColorPalette.Brown, gameEvents),
            ];
        }

        internal Tool GetTool(ToolIndex toolIndex)
        {
            return toolIndex is ToolIndex.None ? null : this.tools[((byte)toolIndex) - 1];
        }
    }
}
