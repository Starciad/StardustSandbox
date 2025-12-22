using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.UI.Options
{
    internal sealed class ColorOption(string name, string description) : Option(name, description)
    {
        private Color color;

        internal override object GetValue()
        {
            return this.color;
        }

        internal override void SetValue(object value)
        {
            this.color = (Color)Convert.ChangeType(value, typeof(Color));
        }
    }
}
