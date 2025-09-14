using System.Collections.Generic;

namespace StardustSandbox.Core.GUISystem.Common.GUIs.Menus.Options.Structure
{
    internal sealed class SRoot
    {
        internal IReadOnlyDictionary<string, SSection> Sections { get; init; }
    }
}
