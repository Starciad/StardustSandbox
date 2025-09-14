using System;

namespace StardustSandbox.Core.GUISystem.Common.Helpers.Options
{
    internal sealed class SButtonOption(string name, string description, Action onClickCallback) : SOption(name, description)
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
