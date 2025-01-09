using System;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Options
{
    internal sealed class SSelectorOption(string identifier, string name, string description, object[] possibleValues, uint defaultIndex = 0) : SOption(identifier, name, description)
    {
        internal object[] PossibleValues => possibleValues;

        private uint selectedValueIndex = defaultIndex;

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
            this.selectedValueIndex = (this.selectedValueIndex == 0)
                ? (uint)(this.PossibleValues.Length - 1)
                : this.selectedValueIndex - 1;
        }
    }
}
