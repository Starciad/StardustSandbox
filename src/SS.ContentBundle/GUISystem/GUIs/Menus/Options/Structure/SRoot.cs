using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure
{
    internal sealed class SRoot
    {
        internal IReadOnlyDictionary<string, SSection> Sections { get; init; }
    }
}
