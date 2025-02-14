using StardustSandbox.GameContent.GUISystem.Helpers.Options;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Menus.Options.Structure
{
    internal sealed class SSection(string name, string description)
    {
        internal string Name => name;
        internal string Description => description;
        internal IReadOnlyDictionary<string, SOption> Options { get; init; }
    }
}
