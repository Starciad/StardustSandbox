using MessagePack;

using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Serialization.Saving.Data;

namespace StardustSandbox.Serialization.Saving
{
    [MessagePackObject]
    public sealed class SaveFile
    {
        [IgnoreMember]
        public Texture2D ThumbnailTexture { get; set; }

        [Key("Metadata")]
        public Metadata Metadata { get; set; }

        [Key("Manifest")]
        public ManifestData Manifest { get; set; }

        [Key("Properties")]
        public PropertiesData Properties { get; set; }

        [Key("Resources")]
        public ResourceData Resources { get; set; }

        [Key("Environment")]
        public EnvironmentData Environment { get; set; }

        [Key("Content")]
        public ContentData Content { get; set; }

        public SaveFile()
        {

        }
    }
}
