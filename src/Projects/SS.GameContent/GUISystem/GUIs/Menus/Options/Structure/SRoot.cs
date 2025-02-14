using System.Collections.Generic;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Menus.Options.Structure
{
    internal sealed class SRoot
    {
        internal IReadOnlyDictionary<string, SSection> Sections { get; init; }
    }
}
