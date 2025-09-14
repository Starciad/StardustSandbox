using StardustSandbox.Core.GUISystem.Common.Helpers.Tools.ColorPicker;

using System;

namespace StardustSandbox.Core.GUISystem.Common.Helpers.Tools.Settings
{
    internal sealed class SColorPickerSettings
    {
        internal Action<SColorPickerResult> OnSelectCallback { get; set; }
    }
}
