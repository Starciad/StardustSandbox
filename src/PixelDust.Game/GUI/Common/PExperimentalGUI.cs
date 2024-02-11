using PixelDust.Game.GUI.Interfaces;

namespace PixelDust.Game.GUI.Common
{
    public sealed class PExperimentalGUI : PGUISystem
    {
        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            _ = layout.OpenElement<PGUIContainerElement>();
            _ = layout.OpenElement<PGUIContainerElement>();
            layout.CloseElement();
            layout.CloseElement();
        }
    }
}
