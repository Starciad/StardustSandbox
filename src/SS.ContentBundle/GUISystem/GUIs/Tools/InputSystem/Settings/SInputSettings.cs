using StardustSandbox.ContentBundle.Enums.GUISystem;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings
{
    internal sealed class SInputSettings
    {
        internal string Synopsis { get; set; }
        internal string Content { get; set; }
        internal SInputType InputType { get; set; }
        internal uint MaxCharacters { get; set; }
        internal Range NumericRange { get; set; }
        internal Action<SArgumentResult> OnSendCallback { get; set; }
        internal Action OnCancelCallback { get; set; }
    }
}
