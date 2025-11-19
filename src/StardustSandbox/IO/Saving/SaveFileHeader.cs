using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.IO.Saving.Header;

namespace StardustSandbox.IO.Saving
{
    public sealed class SaveFileHeader
    {
        public Texture2D ThumbnailTexture { get; set; }
        public SaveFileMetadata Metadata { get; set; } = new();
        public SaveFileInformation Information { get; set; } = new();
    }
}
