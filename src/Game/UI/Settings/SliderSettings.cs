using StardustSandbox.UI.Results;

using System;

namespace StardustSandbox.UI.Settings
{
    internal sealed class SliderSettings
    {
        internal string Synopsis { get; set; }
        internal int MinimumValue { get; set; }
        internal int MaximumValue { get; set; }
        internal int CurrentValue { get; set; }
        internal Action<SliderResult> OnSendCallback { get; set; }
    }
}
