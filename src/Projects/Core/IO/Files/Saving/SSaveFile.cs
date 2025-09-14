namespace StardustSandbox.Core.IO.Files.Saving
{
    public sealed class SSaveFile
    {
        public SSaveFileHeader Header { get; set; } = new();
        public SSaveFileWorld World { get; set; } = new();
    }
}
