using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Camera;

namespace StardustSandbox.Backgrounds
{
    internal sealed class BackgroundLayer
    {
        private Vector2 layerPosition = Vector2.Zero;
        private Vector2 movementOffsetPosition = Vector2.Zero;

        private Vector2 horizontalLayerPosition = Vector2.Zero;
        private Vector2 verticalLayerPosition = Vector2.Zero;
        private Vector2 diagonalLayerPosition = Vector2.Zero;

        private readonly Vector2 parallaxFactor;
        private readonly Vector2 movementSpeed;

        private readonly bool lockX;
        private readonly bool lockY;

        internal BackgroundLayer(Vector2 parallaxFactor, Vector2 movementSpeed, bool lockX = false, bool lockY = false)
        {
            this.parallaxFactor = parallaxFactor;
            this.movementSpeed = movementSpeed;
            this.lockX = lockX;
            this.lockY = lockY;
        }

        internal void Update(GameTime gameTime, int textureWidth, int textureHeight)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 cameraPosition = SSCamera.Position;

            if (!this.lockX)
            {
                if (this.movementSpeed.X != 0)
                {
                    this.movementOffsetPosition.X += this.movementSpeed.X * elapsedSeconds;
                }

                this.layerPosition.X = this.movementOffsetPosition.X + (this.parallaxFactor.X * cameraPosition.X * elapsedSeconds * -1);
                this.layerPosition.X %= textureWidth;
            }

            if (!this.lockY)
            {
                if (this.movementSpeed.Y != 0)
                {
                    this.movementOffsetPosition.Y += this.movementSpeed.Y * elapsedSeconds;
                }

                this.layerPosition.Y = this.movementOffsetPosition.Y + (this.parallaxFactor.Y * cameraPosition.Y * elapsedSeconds);
                this.layerPosition.Y %= textureHeight;
            }

            this.horizontalLayerPosition.X = this.layerPosition.X >= 0
                ? this.layerPosition.X - textureWidth
                : this.layerPosition.X + textureWidth;

            this.verticalLayerPosition.Y = this.layerPosition.Y >= 0
                ? this.layerPosition.Y - textureHeight
                : this.layerPosition.Y + textureHeight;

            this.horizontalLayerPosition.Y = this.layerPosition.Y;
            this.verticalLayerPosition.X = this.layerPosition.X;

            this.diagonalLayerPosition.X = this.horizontalLayerPosition.X;
            this.diagonalLayerPosition.Y = this.verticalLayerPosition.Y;
        }

        internal void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, this.layerPosition, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            if (!this.lockX)
            {
                spriteBatch.Draw(texture, this.horizontalLayerPosition, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }

            if (!this.lockY)
            {
                spriteBatch.Draw(texture, this.verticalLayerPosition, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }

            if (!this.lockX && !this.lockY)
            {
                spriteBatch.Draw(texture, this.diagonalLayerPosition, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }
        }
    }
}
