using MessagePack;

using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class Texture2DData
    {
        [Key(0)]
        public int Width { get; init; }

        [Key(1)]
        public int Height { get; init; }

        [Key(2)]
        public byte[] PixelData { get; init; }

        public Texture2DData()
        {

        }

        public Texture2DData(Texture2D texture2d)
        {
            this.Width = texture2d.Width;
            this.Height = texture2d.Height;
            this.PixelData = new byte[this.Width * this.Height * 4]; // RGBA

            texture2d.GetData(this.PixelData);
        }

        public Texture2D ToTexture2D(GraphicsDevice graphicsDevice)
        {
            Texture2D texture2d = new(graphicsDevice, this.Width, this.Height);
            texture2d.SetData(this.PixelData);
            return texture2d;
        }
    }
}
