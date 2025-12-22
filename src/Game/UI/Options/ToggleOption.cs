using System;

namespace StardustSandbox.UI.Options
{
    internal sealed class ToggleOption(string name, string description) : Option(name, description)
    {
        private bool state;

        internal override object GetValue()
        {
            return this.state;
        }

        internal override void SetValue(object value)
        {
            this.state = Convert.ToBoolean(value);
        }

        internal void Toggle()
        {
            this.state = !this.state;
        }
    }
}
