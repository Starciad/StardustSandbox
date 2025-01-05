namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings
{
    internal sealed class STextArgumentResult
    {
        internal string Content { get; }

        internal STextArgumentResult(string content)
        {
            this.Content = content.Trim();
        }
    }
}
