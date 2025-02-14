using StardustSandbox.GameContent.Enums.GUISystem.Tools.InputSystem;
using StardustSandbox.GameContent.GUISystem.Helpers.Tools.InputSystem;

using System;

namespace StardustSandbox.GameContent.GUISystem.Helpers.Tools.Settings
{
    internal sealed class STextInputSettings
    {
        internal string Synopsis { get; set; }
        internal string Content { get; set; }
        internal bool AllowSpaces { get; set; }
        internal SInputMode InputMode { get; set; }
        internal SInputRestriction InputRestriction { get; set; }
        internal uint MaxCharacters { get; set; }
        internal Action<STextValidationState, STextArgumentResult> OnValidationCallback { get; set; }
        internal Action<STextArgumentResult> OnSendCallback { get; set; }

        internal STextInputSettings()
        {
            this.AllowSpaces = true;
        }
    }
}
