using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.IO.Files.World.Data;

namespace StardustSandbox.Core.IO.Files.World
{
    public sealed class SWorldSaveFile
    {
        public Texture2D Texture { get; internal set; }
        public SWorldSaveFileMetadata Metadata { get; internal set; }

        // Data
        public SWorldData World { get; internal set; }
    }
}
