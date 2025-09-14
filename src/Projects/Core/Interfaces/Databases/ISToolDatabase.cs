using StardustSandbox.Core.Interfaces.Tools;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISToolDatabase
    {
        void RegisterTool(ISTool tool);
        ISTool GetToolByIdentifier(string identifier);
    }
}
