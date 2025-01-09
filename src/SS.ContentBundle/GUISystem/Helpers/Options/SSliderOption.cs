using System;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Options
{
    internal sealed class SSliderOption(string name, string description, Range sliderRange) : SOption(name, description)
    {
        internal Range SliderRange => sliderRange;

        private int currentValue;

        internal override object GetValue()
        {
            return this.currentValue;
        }
        internal override void SetValue(object value)
        {
            if (value is int intValue && intValue >= this.SliderRange.Start.Value && intValue <= this.SliderRange.End.Value)
            {
                this.currentValue = intValue;
            }
        }
    }
}
