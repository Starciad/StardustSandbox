using StardustSandbox.UI.Results;

using System;

namespace StardustSandbox.UI.Settings
{
    internal sealed class SliderSettings
    {
        internal string Synopsis { get; set; }
        internal Range Range { get; set; }
        internal Action<SliderResult> OnSendCallback { get; set; }
    }
}
