namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.InputSystem
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
