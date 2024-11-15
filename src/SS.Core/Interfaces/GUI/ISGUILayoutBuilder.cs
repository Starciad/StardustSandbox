using StardustSandbox.Game.GUISystem.Elements;
using StardustSandbox.Game.Resources.GUISystem.Elements;

namespace StardustSandbox.Game.Interfaces.GUI
{
    public interface ISGUILayoutBuilder
    {
        SGUIRootElement RootElement { get; }

        void AddElement<T>(T value) where T : SGUIElement;
    }
}
