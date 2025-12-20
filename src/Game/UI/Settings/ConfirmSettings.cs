using StardustSandbox.Enums.UI.Tools;

using System;

namespace StardustSandbox.UI.Settings
{
    internal readonly struct ConfirmSettings
    {
        internal readonly string Caption { get; init; }
        internal readonly string Message { get; init; }
        internal readonly Action<ConfirmStatus> OnConfirmCallback { get; init; }
    }
}
