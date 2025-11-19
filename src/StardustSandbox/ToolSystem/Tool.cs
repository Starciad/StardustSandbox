using StardustSandbox.Interfaces.Tools;

namespace StardustSandbox.ToolSystem
{
    internal abstract class Tool
    {
        internal abstract void Execute(IToolContext context);
    }
}
