using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;

namespace PixelDust.Game.Interfaces.GUI
{
    public interface IPGUILayoutBuilder
    {
        PGUIRootElement RootElement { get; }

        T CreateElement<T>() where T : PGUIElement;
    }
}
