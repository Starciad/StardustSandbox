using StardustSandbox.UI.Results;

using System;

namespace StardustSandbox.UI.Settings
{
    internal readonly struct ColorPickerSettings
    {
        internal readonly Action<ColorPickerResult> OnSelectCallback { get; init; }
    }
}
