using StardustSandbox.Interfaces.Tools;

namespace StardustSandbox.Tools
{
    internal abstract class Tool
    {
        internal abstract void Execute(IToolContext context);
    }
}
