namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings
{
    internal sealed class SArgumentResult
    {
        internal string Content { get; }

        internal SArgumentResult(string content)
        {
            this.Content = content.Trim();
        }
    }
}
