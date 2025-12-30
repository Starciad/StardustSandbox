using StardustSandbox.Serialization.Saving.Data;

namespace StardustSandbox.Serialization.Saving
{
    public sealed class SaveFile
    {
        public Texture2DData ThumbnailTextureData { get; init; }
        public Metadata Metadata { get; init; }
        public ManifestData Manifest { get; init; }
        public PropertyData Properties { get; init; }
        public EnvironmentData Environment { get; init; }
        public ContentData Content { get; init; }
    }
}
