using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISGUIManager
    {
        SGUIEvents GUIEvents { get; }

        void ShowGUI(string identifier);
        void CloseGUI(string identifier);

        SGUISystem GetGUIById(string identifier);
        bool TryGetGUIById(string identifier, out SGUISystem guiSystem);
    }
}
