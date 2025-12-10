using Microsoft.Xna.Framework;

namespace StardustSandbox.UI.Results
{
    internal sealed class ColorPickerResult
    {
        internal Color SelectedColor { get; }

        internal ColorPickerResult(Color selectedColor)
        {
            this.SelectedColor = selectedColor;
        }
    }
}
