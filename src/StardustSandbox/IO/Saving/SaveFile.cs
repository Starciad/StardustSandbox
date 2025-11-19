namespace StardustSandbox.IO.Saving
{
    public sealed class SaveFile
    {
        public SaveFileHeader Header { get; set; } = new();
        public SaveFileWorld World { get; set; } = new();
    }
}
