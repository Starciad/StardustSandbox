using System;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Options
{
    internal sealed class SButtonOption(string identifier, string name, string description, Action onClickCallback) : SOption(identifier, name, description)
    {
        public Action OnClickCallback => onClickCallback;

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
