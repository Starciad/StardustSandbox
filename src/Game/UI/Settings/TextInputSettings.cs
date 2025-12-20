using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.UI.Results;
using StardustSandbox.UI.States;

using System;

namespace StardustSandbox.UI.Settings
{
    internal readonly struct TextInputSettings
    {
        internal readonly string Synopsis { get; init; }
        internal readonly string Content { get; init; }
        internal readonly bool AllowSpaces { get; init; }
        internal readonly InputMode InputMode { get; init; }
        internal readonly InputRestriction InputRestriction { get; init; }
        internal readonly uint MaxCharacters { get; init; }
        internal readonly Func<TextInputResult, TextValidationState> OnValidationCallback { get; init; }
        internal readonly Action<TextInputResult> OnSendCallback { get; init; }

        public TextInputSettings()
        {
            this.AllowSpaces = true;
        }
    }
}
