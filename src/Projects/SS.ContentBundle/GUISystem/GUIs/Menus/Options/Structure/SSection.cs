using StardustSandbox.ContentBundle.GUISystem.Helpers.Options;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure
{
    internal sealed class SSection(string name, string description)
    {
        internal string Name => name;
        internal string Description => description;
        internal IReadOnlyDictionary<string, SOption> Options { get; init; }
    }
}
