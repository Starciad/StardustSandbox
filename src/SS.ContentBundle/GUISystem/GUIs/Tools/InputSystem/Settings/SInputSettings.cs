using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.InputSystem;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings
{
    internal sealed class SInputSettings
    {
        internal string Synopsis { get; set; }
        internal string Content { get; set; }
        internal bool AllowSpaces { get; set; }
        internal SInputMode InputMode { get; set; }
        internal SInputRestriction InputRestriction { get; set; }
        internal uint MaxCharacters { get; set; }
        internal Action<SArgumentResult> OnSendCallback { get; set; }
        internal Action OnCancelCallback { get; set; }
    }
}
