using StardustSandbox.Enums.Tools;
using StardustSandbox.Tools;

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
            ];

            isLoaded = true;
        }

        internal static Tool GetTool(Type toolType)
        {
            return Array.Find(tools, x => x.GetType() == toolType);
        }
    }
}
