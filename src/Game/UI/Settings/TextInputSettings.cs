using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.UI.Results;
using StardustSandbox.UI.States;

using System;

namespace StardustSandbox.UI.Settings
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
