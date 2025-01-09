using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure
{
    internal enum SOptionType : byte
    {
        Button = 0,
        Selector = 1,
        Slider = 2,
        Color = 3,
    }

    internal sealed class SOption(string identifier, string name, string description, SOptionType optionType)
    {
        internal string Identififer => identifier;
        internal string Name => name;
        internal string Description => description;
        internal SOptionType OptionType => optionType;
        internal object[] Values { get; init; } = [];
        internal Range Range { get; init; }
        internal Action OnClickCallback { get; init; }

        internal uint SelectedValueIndex { get; set; }
    }
}
