using Microsoft.Xna.Framework;

namespace StardustSandbox.UISystem.Results
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
