using System;

namespace StardustSandbox.ContentBundle.GUISystem.Specials.Interactive
{
    internal sealed class SButton(string displayName, Action clickAction)
    {
        internal string DisplayName => displayName;
        internal Action ClickAction => clickAction;
    }
}
