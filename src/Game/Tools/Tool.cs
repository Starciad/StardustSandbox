using StardustSandbox.Enums.Tools;

namespace StardustSandbox.Tools
{
    internal abstract class Tool
    {
        internal required ToolIndex Index { get; init; }

        internal abstract void Execute(ToolContext context);
    }
}
