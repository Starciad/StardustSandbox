using StardustSandbox.Enums.UI.Tools;

using System;

namespace StardustSandbox.UI.Settings
{
    internal sealed class ConfirmSettings
    {
        internal string Caption { get; set; }
        internal string Message { get; set; }
        internal Action<ConfirmStatus> OnConfirmCallback { get; set; }
    }
}
