using Microsoft.Xna.Framework;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Options
{
    internal sealed class SColorOption(string identifier, string name, string description, Color defaultColor) : SOption(identifier, name, description)
    {
        private Color currentColor = defaultColor;

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
