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

using System;

namespace StardustSandbox.Core.Databases
{
    internal sealed class ToolDatabase
    {
        private Tool[] tools;

        internal void Load(AchievementManager achievementManager)
        {
            tools = [
                // [000] Heat Tool
                new HeatTool(ToolIndex.HeatTool, achievementManager),

                // [001] Freeze Tool
                new FreezeTool(ToolIndex.FreezeTool, achievementManager),

                // [002] Ink Tool (Black)
                new InkTool(ToolIndex.BlackInkTool, AAP64ColorPalette.DarkGray, achievementManager),

                // [003] Ink Tool (White)
                new InkTool(ToolIndex.WhiteInkTool, AAP64ColorPalette.White, achievementManager),

                // [004] Ink Tool (Red)
                new InkTool(ToolIndex.RedInkTool, AAP64ColorPalette.Crimson, achievementManager),

                // [005] Ink Tool (Orange)
                new InkTool(ToolIndex.OrangeInkTool, AAP64ColorPalette.Orange, achievementManager),

                // [006] Ink Tool (Yellow)
                new InkTool(ToolIndex.YellowInkTool, AAP64ColorPalette.Gold, achievementManager),

                // [007] Ink Tool (Green)
                new InkTool(ToolIndex.GreenInkTool, AAP64ColorPalette.GrassGreen, achievementManager),

                // [008] Ink Tool (Blue)
                new InkTool(ToolIndex.BlueInkTool, AAP64ColorPalette.RoyalBlue, achievementManager),

                // [009] Ink Tool (Gray)
                new InkTool(ToolIndex.GrayInkTool, AAP64ColorPalette.Slate, achievementManager),

                // [010] Ink Tool (Violet)
                new InkTool(ToolIndex.VioletInkTool, AAP64ColorPalette.Violet, achievementManager),

                // [011] Ink Tool (Brown)
                new InkTool(ToolIndex.BrownInkTool, AAP64ColorPalette.Brown, achievementManager),
            ];
        }

        internal Tool GetTool(ToolIndex toolIndex)
        {
            return tools[(int)toolIndex];
        }
    }
}
