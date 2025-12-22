using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.UI.Settings
{
    internal readonly struct ColorPickerSettings
    {
        internal readonly Action<Color> OnSelectCallback { get; init; }
    }
}
