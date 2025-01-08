namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure
{
    internal enum SOptionType
    {
        Button,
        Selector,
        Slider,
        Color,
        Toggle,
    }

    internal sealed class SOption(string identifier, string name, string description, SOptionType optionType)
    {
        internal string Identififer => identifier;
        internal string Name => name;
        internal string Description => description;
        internal SOptionType OptionType => optionType;
    }
}
