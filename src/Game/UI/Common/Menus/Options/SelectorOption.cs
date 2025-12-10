using System;

namespace StardustSandbox.UI.Common.Menus.Options
{
    internal sealed class SelectorOption(string name, string description, object[] possibleValues) : Option(name, description)
    {
        internal object[] PossibleValues => possibleValues;

        private uint selectedValueIndex;

        internal override object GetValue()
        {
            return this.PossibleValues[this.selectedValueIndex];
        }

        internal override void SetValue(object value)
        {
            this.selectedValueIndex = (uint)Array.IndexOf(this.PossibleValues, value);
        }

        internal void Next()
        {
            this.selectedValueIndex = (this.selectedValueIndex + 1) % (uint)this.PossibleValues.Length;
        }

        internal void Previous()
        {
            this.selectedValueIndex = this.selectedValueIndex == 0
                ? (uint)(this.PossibleValues.Length - 1)
                : this.selectedValueIndex - 1;
        }
    }
}
