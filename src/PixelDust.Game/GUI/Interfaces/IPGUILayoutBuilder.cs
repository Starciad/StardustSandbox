using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;

namespace PixelDust.Game.GUI.Interfaces
{
    public interface IPGUILayoutBuilder
    {
        PGUIRootElement RootElement { get; }

        T CreateElement<T>() where T : PGUIElement;
        T CreateElement<T>(T value) where T : PGUIElement;
    }
}
