using StardustSandbox.Core.GUISystem;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISGUIDatabase
    {
        void RegisterGUISystem(SGUISystem guiSystem);
        SGUISystem GetGUISystemById(string identifier);
    }
}
