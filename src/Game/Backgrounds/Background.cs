using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Backgrounds
{
    internal sealed class Background
    {
        internal bool IsAffectedByLighting => this.isAffectedByLighting;

        private readonly BackgroundLayer[] backgroundLayers;
        private readonly bool isAffectedByLighting;
        private readonly int layerCount;
        private readonly Texture2D texture;

        internal Background(BackgroundLayer[] backgroundLayers, bool isAffectedByLighting, Texture2D texture)
        {
            this.backgroundLayers = backgroundLayers;
            this.isAffectedByLighting = isAffectedByLighting;
            this.layerCount = backgroundLayers.Length;
            this.texture = texture;
        }

        internal void Update(in GameTime gameTime)
        {
            for (int i = 0; i < this.layerCount; i++)
            {
                this.backgroundLayers[i].Update(gameTime, this.texture.Width, this.texture.Height);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.layerCount; i++)
            {
                this.backgroundLayers[i].Draw(spriteBatch, this.texture);
            }
        }
    }
}
