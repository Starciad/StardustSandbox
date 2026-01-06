using System;

namespace StardustSandbox.UI.Options
{
    internal class ButtonOption(string name, string description, Action onClickAction) : Option(name, description)
    {
        internal override object GetValue()
        {
            return default;
        }

        internal override void SetValue(object value)
        {
            return;
        }

        internal void Click()
        {
            onClickAction();
        }
    }
}
