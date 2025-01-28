using StardustSandbox.Core.Interfaces.Tools;
using StardustSandbox.Core.Interfaces.Tools.Contexts;

namespace StardustSandbox.Core.Tools
{
    public abstract class STool(string identifier) : ISTool
    {
        public string Identifier => identifier;

        public abstract void Execute(ISToolContext context);
    }
}
