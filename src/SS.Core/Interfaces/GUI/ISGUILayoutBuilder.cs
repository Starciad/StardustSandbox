using StardustSandbox.Core.GUISystem.Elements;

namespace StardustSandbox.Core.Interfaces.GUI
{
    public interface ISGUILayoutBuilder
    {
        SGUIRootElement RootElement { get; }

        void AddElement<T>(T value) where T : SGUIElement;
    }
}
