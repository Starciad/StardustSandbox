using System;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Options
{
    internal sealed class SSliderOption(string name, string description, float minimumValue = 0, float maximumValue = 1) : SOption(name, description)
    {
        public float MinimumValue => minimumValue;
        public float MaximumValue => maximumValue;
        public float CurrentValue => this.currentValue;

        private float currentValue;

        internal override object GetValue()
        {
            return this.currentValue;
        }
        internal override void SetValue(object value)
        {
            this.currentValue =  float.Clamp((float)value, this.MinimumValue, this.MaximumValue);
        }
    }
}
