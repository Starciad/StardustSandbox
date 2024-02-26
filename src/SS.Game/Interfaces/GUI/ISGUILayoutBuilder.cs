using StardustSandbox.Game.GUI.Elements;
using StardustSandbox.Game.GUI.Elements.Common;

namespace StardustSandbox.Game.Interfaces.GUI
{
    public interface ISGUILayoutBuilder
    {
        SGUIRootElement RootElement { get; }

        T CreateElement<T>() where T : SGUIElement;
    }
}
