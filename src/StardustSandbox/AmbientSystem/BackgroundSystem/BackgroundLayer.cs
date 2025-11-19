using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Managers;

namespace StardustSandbox.AmbientSystem.BackgroundSystem
{
    internal sealed class BackgroundLayer(CameraManager cameraManager, Texture2D texture, Rectangle textureClippingRectangle, Vector2 parallaxFactor, Vector2 movementSpeed, bool lockX = false, bool lockY = false)
    {
        private Vector2 layerPosition = Vector2.Zero;
        private Vector2 movementOffsetPosition = Vector2.Zero;

        private Vector2 horizontalLayerPosition = Vector2.Zero;
        private Vector2 verticalLayerPosition = Vector2.Zero;
        private Vector2 diagonalLayerPosition = Vector2.Zero;

        private readonly Texture2D texture = texture;
        private readonly Rectangle textureClippingRectangle = textureClippingRectangle;

        private readonly Vector2 parallaxFactor = parallaxFactor;
        private readonly Vector2 movementSpeed = movementSpeed;

        private readonly bool lockX = lockX;
        private readonly bool lockY = lockY;

        private readonly CameraManager cameraManager = cameraManager;

        internal void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 cameraPosition = this.cameraManager.Position;

            if (!this.lockX)
            {
                if (this.movementSpeed.X != 0)
                {
                    this.movementOffsetPosition.X += this.movementSpeed.X * elapsedSeconds;
                }

                this.layerPosition.X = this.movementOffsetPosition.X + (this.parallaxFactor.X * cameraPosition.X * elapsedSeconds * -1);
                this.layerPosition.X %= this.textureClippingRectangle.Width;
            }

            if (!this.lockY)
            {
                if (this.movementSpeed.Y != 0)
                {
                    this.movementOffsetPosition.Y += this.movementSpeed.Y * elapsedSeconds;
                }

                this.layerPosition.Y = this.movementOffsetPosition.Y + (this.parallaxFactor.Y * cameraPosition.Y * elapsedSeconds);
                this.layerPosition.Y %= this.textureClippingRectangle.Height;
            }

            this.horizontalLayerPosition.X = this.layerPosition.X >= 0
                ? this.layerPosition.X - this.textureClippingRectangle.Width
                : this.layerPosition.X + this.textureClippingRectangle.Width;

            this.verticalLayerPosition.Y = this.layerPosition.Y >= 0
                ? this.layerPosition.Y - this.textureClippingRectangle.Height
                : this.layerPosition.Y + this.textureClippingRectangle.Height;

            this.horizontalLayerPosition.Y = this.layerPosition.Y;
            this.verticalLayerPosition.X = this.layerPosition.X;

            this.diagonalLayerPosition.X = this.horizontalLayerPosition.X;
            this.diagonalLayerPosition.Y = this.verticalLayerPosition.Y;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.layerPosition, this.textureClippingRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            if (!this.lockX)
            {
                spriteBatch.Draw(this.texture, this.horizontalLayerPosition, this.textureClippingRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }

            if (!this.lockY)
            {
                spriteBatch.Draw(this.texture, this.verticalLayerPosition, this.textureClippingRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }

            if (!this.lockX && !this.lockY)
            {
                spriteBatch.Draw(this.texture, this.diagonalLayerPosition, this.textureClippingRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }
        }
    }
}
