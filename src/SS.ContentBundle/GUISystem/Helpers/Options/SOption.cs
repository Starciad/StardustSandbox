namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Options
{
    internal abstract class SOption(string identifier, string name, string description)
    {
        internal string Identifier => identifier;
        internal string Name => name;
        internal string Description => description;

        internal abstract object GetValue();
        internal abstract void SetValue(object value);
    }
}
