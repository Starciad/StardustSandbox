using StardustSandbox.Core.Interfaces.Tools.Contexts;

namespace StardustSandbox.Core.Interfaces.Tools
{
    public interface ISTool
    {
        string Identifier { get; }

        void Execute(ISToolContext context);
    }
}
