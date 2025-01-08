namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure
{
    internal sealed class SSection(string identifier, string name, string description, SOption[] options)
    {
        internal string Identifier => identifier;
        internal string Name => name;
        internal string Description => description;
        internal SOption[] Options => options;
    }
}
