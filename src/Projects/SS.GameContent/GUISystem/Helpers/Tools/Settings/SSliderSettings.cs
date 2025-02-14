using System;

namespace StardustSandbox.GameContent.GUISystem.Helpers.Tools.Settings
{
    internal sealed class SSliderSettings
    {
        internal string Synopsis { get; set; }
        internal Range Range { get; set; }
        internal Action<SSliderResult> OnSendCallback { get; set; }
    }
}
