namespace StardustSandbox.UI.Results
{
    internal sealed class TextArgumentResult
    {
        internal string Content { get; }

        internal TextArgumentResult(string content)
        {
            this.Content = content.Trim();
        }
    }
}
