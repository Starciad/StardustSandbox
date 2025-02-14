using System;

namespace StardustSandbox.GameContent.GUISystem.Helpers.Tools.Settings
{
    internal sealed class SConfirmSettings
    {
        internal string Caption { get; set; }
        internal string Message { get; set; }
        internal Action<SConfirmStatus> OnConfirmCallback { get; set; }
    }
}
