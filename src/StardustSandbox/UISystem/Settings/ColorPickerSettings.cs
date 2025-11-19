using StardustSandbox.UISystem.Results;

using System;

namespace StardustSandbox.UISystem.Settings
{
    internal sealed class ColorPickerSettings
    {
        internal Action<ColorPickerResult> OnSelectCallback { get; set; }
    }
}
