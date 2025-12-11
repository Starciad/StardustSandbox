using System;

namespace StardustSandbox.UI.Common.Menus.Options
{
    internal sealed class SliderOption(string name, string description, int minimumValue, int maximumValue) : Option(name, description)
    {
        internal int MinimumValue => minimumValue;
        internal int MaximumValue => maximumValue;
        
        private int currentValue;

        internal override object GetValue()
        {
            return this.currentValue;
        }

        internal override void SetValue(object value)
        {
            this.currentValue = int.Clamp(Convert.ToInt32(value), this.MinimumValue, this.MaximumValue);
        }
    }
}
