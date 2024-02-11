using PixelDust.Game.GUI.Elements;

namespace PixelDust.Game.GUI.Interfaces
{
    public interface IPGUILayoutBuilder
    {
        T OpenElement<T>() where T : PGUIElement;
        T CreateElement<T>() where T : PGUIElement;
        void CloseElement();
    }
}
