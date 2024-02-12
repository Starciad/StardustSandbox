using PixelDust.Game.GUI.Elements;

namespace PixelDust.Game.GUI.Interfaces
{
    public interface IPGUILayoutBuilder
    {
        T CreateElement<T>() where T : PGUIElement;
    }
}
