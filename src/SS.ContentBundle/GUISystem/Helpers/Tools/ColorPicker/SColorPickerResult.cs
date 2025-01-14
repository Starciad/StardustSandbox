using Microsoft.Xna.Framework;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.ColorPicker
{
    internal sealed class SColorPickerResult
    {
        internal Color SelectedColor { get; }

        internal SColorPickerResult(Color selectedColor)
        {
            this.SelectedColor = selectedColor;
        }
    }
}
