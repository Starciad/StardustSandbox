using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Managers;

namespace StardustSandbox.BackgroundSystem
{
    internal sealed class Background
    {
        internal bool IsAffectedByLighting => this.isAffectedByLighting;

        private readonly BackgroundLayer[] backgroundLayers;
        private readonly CameraManager cameraManager;
        private readonly bool isAffectedByLighting;
        private readonly int layerCount;
        private readonly Texture2D texture;

        internal Background(BackgroundLayer[] backgroundLayers, CameraManager cameraManager, bool isAffectedByLighting, Texture2D texture)
        {
            this.backgroundLayers = backgroundLayers;
            this.cameraManager = cameraManager;
            this.isAffectedByLighting = isAffectedByLighting;
            this.layerCount = backgroundLayers.Length;
            this.texture = texture;
        }

        internal void Update(GameTime gameTime)
        {
            for (byte i = 0; i < this.layerCount; i++)
            {
                this.backgroundLayers[i].Update(this.cameraManager, gameTime, this.texture.Width, this.texture.Height);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (byte i = 0; i < this.layerCount; i++)
            {
                this.backgroundLayers[i].Draw(spriteBatch, this.texture);
            }
        }
    }
}
