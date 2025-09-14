using StardustSandbox.Core.Enums.GUISystem.Tools.Confirm;

using System;

namespace StardustSandbox.Core.GUISystem.Common.Helpers.Tools.Settings
{
    internal sealed class SConfirmSettings
    {
        internal string Caption { get; set; }
        internal string Message { get; set; }
        internal Action<SConfirmStatus> OnConfirmCallback { get; set; }
    }
}
