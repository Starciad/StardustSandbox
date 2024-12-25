using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.IO.Files.World.Data;

namespace StardustSandbox.Core.IO.Files.World
{
    public sealed class SWorldSaveFile
    {
        public Texture2D ThumbnailTexture { get; set; }
        public SWorldSaveFileMetadata Metadata { get; set; }
        public SWorldData World { get; set; }
    }
}
