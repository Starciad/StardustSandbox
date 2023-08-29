using MLEM.Ui.Elements;

namespace PixelDust.Core.GUI
{
    public interface IPGUIBuilder
    {
        PGUIBuildElement Create(Element UIElement);
        void CreateClosed(Element UIElement);
    }
}
