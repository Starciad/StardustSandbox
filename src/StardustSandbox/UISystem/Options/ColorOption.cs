using Microsoft.Xna.Framework;

namespace StardustSandbox.UISystem.Options
{
    internal sealed class ColorOption(string name, string description) : Option(name, description)
    {
        internal Color CurrentColor => this.currentColor;

        private Color currentColor;

        internal override object GetValue()
        {
            return this.currentColor;
        }

        internal override void SetValue(object value)
        {
            if (value is Color color)
            {
                this.currentColor = color;
            }
        }
    }
}
