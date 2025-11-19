using StardustSandbox.UISystem.Results;

using System;

namespace StardustSandbox.UISystem.Settings
{
    internal sealed class SliderSettings
    {
        internal string Synopsis { get; set; }
        internal Range Range { get; set; }
        internal Action<SliderResult> OnSendCallback { get; set; }
    }
}
