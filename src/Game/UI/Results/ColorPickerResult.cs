using Microsoft.Xna.Framework;

namespace StardustSandbox.UI.Results
{
    internal readonly struct ColorPickerResult(Color selectedColor)
    {
        internal Color SelectedColor => selectedColor;
    }
}
