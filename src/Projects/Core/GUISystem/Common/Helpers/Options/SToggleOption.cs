using System;

namespace StardustSandbox.Core.GUISystem.Common.Helpers.Options
{
    internal sealed class SToggleOption(string name, string description) : SOption(name, description)
    {
        public bool State => this.state;

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
