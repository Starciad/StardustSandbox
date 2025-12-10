using StardustSandbox.UI.Results;

using System;

namespace StardustSandbox.UI.Settings
{
    internal sealed class ColorPickerSettings
    {
        internal Action<ColorPickerResult> OnSelectCallback { get; set; }
    }
}
