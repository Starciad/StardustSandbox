using StardustSandbox.ToolSystem;

using System;

namespace StardustSandbox.Databases
{
    internal static class ToolDatabase
    {
        private static Tool[] tools;

        internal static void Load()
        {
            tools = [
                new HeatTool(),
                new FreezeTool()
            ];
        }

        internal static Tool GetToolByType(Type toolType)
        {
            return Array.Find(tools, x => x.GetType() == toolType);
        }
    }
}
