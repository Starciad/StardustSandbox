using StardustSandbox.Enums.UISystem.Tools;
using StardustSandbox.UISystem.Results;
using StardustSandbox.UISystem.States;

using System;

namespace StardustSandbox.UISystem.Settings
{
    internal sealed class TextInputSettings
    {
        internal string Synopsis { get; set; }
        internal string Content { get; set; }
        internal bool AllowSpaces { get; set; }
        internal InputMode InputMode { get; set; }
        internal InputRestriction InputRestriction { get; set; }
        internal uint MaxCharacters { get; set; }
        internal Action<TextValidationState, TextArgumentResult> OnValidationCallback { get; set; }
        internal Action<TextArgumentResult> OnSendCallback { get; set; }

        internal TextInputSettings()
        {
            this.AllowSpaces = true;
        }
    }
}
