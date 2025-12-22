using Microsoft.Xna.Framework.Input;

using System;

namespace StardustSandbox.UI.Options
{
    internal sealed class KeyOption(string name, string description) : Option(name, description)
    {
        private Keys key;

        internal override object GetValue()
        {
            return this.key;
        }

        internal override void SetValue(object value)
        {
            this.key = (Keys)Convert.ChangeType(value, typeof(Keys));
        }
    }
}
