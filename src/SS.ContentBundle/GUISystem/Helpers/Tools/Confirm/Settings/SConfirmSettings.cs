using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.Confirm;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.Confirm.Settings
{
    internal sealed class SConfirmSettings
    {
        internal string Caption { get; set; }
        internal string Message { get; set; }
        internal Action<SConfirmStatus> OnConfirmCallback { get; set; }
    }
}
