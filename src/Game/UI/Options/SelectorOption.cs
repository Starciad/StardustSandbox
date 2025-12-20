using System;

namespace StardustSandbox.UI.Options
{
    internal sealed class SelectorOption(string name, string description, object[] values) : Option(name, description)
    {
        internal object[] Values => values;

        private int selectedValueIndex;

        internal override object GetValue()
        {
            return this.Values[this.selectedValueIndex];
        }

        internal override void SetValue(object value)
        {
            this.selectedValueIndex = Array.IndexOf(this.Values, value);
        }

        internal void Next()
        {
            this.selectedValueIndex = (this.selectedValueIndex + 1) % this.Values.Length;
        }

        internal void Previous()
        {
            this.selectedValueIndex = this.selectedValueIndex == 0
                ? this.Values.Length - 1
                : this.selectedValueIndex - 1;
        }
    }
}
