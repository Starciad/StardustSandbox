using System;

namespace StardustSandbox.UISystem.Options
{
    internal sealed class ValueOption(string name, string description, int minimumValue, int maximumValue) : Option(name, description)
    {
        internal int MinimumValue => minimumValue;
        internal int MaximumValue => maximumValue;
        internal int CurrentValue => this.currentValue;

        private int currentValue;

        internal override object GetValue()
        {
            return this.currentValue;
        }

        internal override void SetValue(object value)
        {
            this.currentValue = int.Clamp(Convert.ToInt32(value), this.MinimumValue, this.MaximumValue);
        }

        internal void Increment()
        {
            this.currentValue = this.currentValue == this.MaximumValue
                ? this.MinimumValue
                : this.currentValue + 1;
        }

        internal void Decrement()
        {
            this.currentValue = this.currentValue == this.MinimumValue
                ? this.MaximumValue
                : this.currentValue - 1;
        }
    }
}
