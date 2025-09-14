namespace StardustSandbox.Core.GUISystem.Common.Helpers.Tools.InputSystem
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
