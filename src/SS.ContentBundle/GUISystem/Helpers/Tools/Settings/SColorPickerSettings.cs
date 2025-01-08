using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.ColorPicker;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.Settings
{
    internal sealed class SColorPickerSettings
    {
        internal string Caption { get; set; }
        internal Color[] HighlightedColors { get; set; }
        internal Action<SColorPickerResult> OnSelectCallback { get; set; }
    }
}
