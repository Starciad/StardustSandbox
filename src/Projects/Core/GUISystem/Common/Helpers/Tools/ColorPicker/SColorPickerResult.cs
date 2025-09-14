using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.GUISystem.Common.Helpers.Tools.ColorPicker
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
