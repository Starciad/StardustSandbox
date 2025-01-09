using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure
{
    internal enum SOptionType : byte
    {
        Selector = 0,
        Slider = 1,
        Color = 2,
        Toggle = 3,
    }

    internal sealed class SOption(string identifier, string name, string description, SOptionType optionType)
    {
        internal string Identififer => identifier;
        internal string Name => name;
        internal string Description => description;
        internal SOptionType OptionType => optionType;
        internal object[] Values { get; init; }
        internal Range Range { get; init; }
    }
}
