using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.IO.Files.Saving.Header;

namespace StardustSandbox.Core.IO.Files.Saving
{
    public sealed class SSaveFileHeader
    {
        public Texture2D ThumbnailTexture { get; set; }
        public SSaveFileMetadata Metadata { get; set; } = new();
        public SSaveFileInformation Information { get; set; } = new();
    }
}
