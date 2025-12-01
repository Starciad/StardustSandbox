using System;

namespace StardustSandbox.UI.Options
{
    internal sealed class ButtonOption(string name, string description, Action onClickCallback) : Option(name, description)
    {
        internal Action OnClickCallback => onClickCallback;

        internal override object GetValue()
        {
            return default;
        }

        internal override void SetValue(object value)
        {
            return;
        }
    }
}
