namespace StardustSandbox.UI.Common.Menus.Options
{
    internal abstract class Option(string name, string description)
    {
        internal string Name => name;
        internal string Description => description;

        internal abstract object GetValue();
        internal abstract void SetValue(object value);
    }
}
