using System;

namespace StardustSandbox.GameContent.GUISystem.Helpers.Options
{
    internal sealed class SValueOption(string name, string description, int minimumValue, int maximumValue) : SOption(name, description)
    {
        public int MinimumValue => minimumValue;
        public int MaximumValue => maximumValue;
        public int CurrentValue => this.currentValue;

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
