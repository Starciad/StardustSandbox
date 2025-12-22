using StardustSandbox.Colors.Palettes;
using StardustSandbox.Enums.Tools;
using StardustSandbox.Tools;
using StardustSandbox.Tools.Environment;
using StardustSandbox.Tools.Inks;

using System;

namespace StardustSandbox.Databases
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
                new HeatTool()
                {
                    Index = ToolIndex.HeatTool
                },

                new FreezeTool()
                {
                    Index = ToolIndex.FreezeTool,
                },

                new BlackInkTool()
                {
                    Index = ToolIndex.BlackInkTool,
                    InkColor = AAP64ColorPalette.DarkGray,
                },

                new WhiteInkTool()
                {
                    Index = ToolIndex.WhiteInkTool,
                    InkColor = AAP64ColorPalette.White,
                },

                new RedInkTool()
                {
                    Index = ToolIndex.RedInkTool,
                    InkColor = AAP64ColorPalette.Crimson,
                },

                new OrangeInkTool()
                {
                    Index = ToolIndex.OrangeInkTool,
                    InkColor = AAP64ColorPalette.Orange,
                },

                new YellowInkTool()
                {
                    Index = ToolIndex.YellowInkTool,
                    InkColor = AAP64ColorPalette.Gold,
                },

                new GreenInkTool()
                {
                    Index = ToolIndex.GreenInkTool,
                    InkColor = AAP64ColorPalette.GrassGreen,
                },

                new BlueInkTool()
                {
                    Index = ToolIndex.BlueInkTool,
                    InkColor = AAP64ColorPalette.RoyalBlue,
                },

                new GrayInkTool()
                {
                    Index = ToolIndex.GrayInkTool,
                    InkColor = AAP64ColorPalette.Slate,
                },

                new VioletInkTool()
                {
                    Index = ToolIndex.VioletInkTool,
                    InkColor = AAP64ColorPalette.Violet,
                },

                new BrownInkTool()
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
