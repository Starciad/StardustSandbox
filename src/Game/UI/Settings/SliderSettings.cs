using StardustSandbox.UI.Results;

using System;

namespace StardustSandbox.UI.Settings
{
    internal readonly struct SliderSettings
    {
        internal readonly string Synopsis { get; init; }
        internal readonly int MinimumValue { get; init; }
        internal readonly int MaximumValue { get; init; }
        internal readonly int CurrentValue { get; init; }
        internal readonly Action<SliderResult> OnSendCallback { get; init; }
    }
}
