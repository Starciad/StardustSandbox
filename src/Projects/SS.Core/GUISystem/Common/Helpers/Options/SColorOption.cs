using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.GUISystem.Common.Helpers.Options
{
    internal sealed class SColorOption(string name, string description) : SOption(name, description)
    {
        public Color CurrentColor => this.currentColor;

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
