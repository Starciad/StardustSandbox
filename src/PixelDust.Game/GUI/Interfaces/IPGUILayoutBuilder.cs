using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;

namespace PixelDust.Game.GUI.Interfaces
{
    public interface IPGUILayoutBuilder
    {
        PGUIRootElement Root { get; }
        PGUIElement[] Elements { get; }

        T CreateElement<T>() where T : PGUIElement;
    }
}
