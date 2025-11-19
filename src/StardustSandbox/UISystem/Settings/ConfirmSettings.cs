using StardustSandbox.Enums.UISystem.Tools;

using System;

namespace StardustSandbox.UISystem.Settings
{
    internal sealed class ConfirmSettings
    {
        internal string Caption { get; set; }
        internal string Message { get; set; }
        internal Action<ConfirmStatus> OnConfirmCallback { get; set; }
    }
}
