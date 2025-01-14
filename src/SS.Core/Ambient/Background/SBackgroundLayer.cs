using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Ambient.Background
{
    public sealed class SBackgroundLayer(ISGame gameInstance, Texture2D texture, Rectangle textureClippingRectangle, Vector2 parallaxFactor, Vector2 movementSpeed, bool lockX = false, bool lockY = false) : SGameObject(gameInstance)
    {
        private Vector2 layerPosition = Vector2.Zero;
        private Vector2 movementOffsetPosition = Vector2.Zero;

        private Vector2 horizontalLayerPosition = Vector2.Zero;
        private Vector2 verticalLayerPosition = Vector2.Zero;
        private Vector2 diagonalLayerPosition = Vector2.Zero;

        private readonly Texture2D _texture = texture;
        private readonly Rectangle _textureClippingRectangle = textureClippingRectangle;

        private readonly Vector2 _parallaxFactor = parallaxFactor;
        private readonly Vector2 _movementSpeed = movementSpeed;

        private readonly bool _lockX = lockX;
        private readonly bool _lockY = lockY;

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 cameraPosition = this.SGameInstance.CameraManager.Position;

            if (!this._lockX)
            {
                if (this._movementSpeed.X != 0)
                {
                    this.movementOffsetPosition.X += this._movementSpeed.X * elapsedSeconds;
                }

                this.layerPosition.X = this.movementOffsetPosition.X + (this._parallaxFactor.X * cameraPosition.X * elapsedSeconds * -1);
                this.layerPosition.X %= this._textureClippingRectangle.Width;
            }

            if (!this._lockY)
            {
                if (this._movementSpeed.Y != 0)
                {
                    this.movementOffsetPosition.Y += this._movementSpeed.Y * elapsedSeconds;
                }

                this.layerPosition.Y = this.movementOffsetPosition.Y + (this._parallaxFactor.Y * cameraPosition.Y * elapsedSeconds);
                this.layerPosition.Y %= this._textureClippingRectangle.Height;
            }

            this.horizontalLayerPosition.X = this.layerPosition.X >= 0
                ? this.layerPosition.X - this._textureClippingRectangle.Width
                : this.layerPosition.X + this._textureClippingRectangle.Width;

            this.verticalLayerPosition.Y = this.layerPosition.Y >= 0
                ? this.layerPosition.Y - this._textureClippingRectangle.Height
                : this.layerPosition.Y + this._textureClippingRectangle.Height;

            this.horizontalLayerPosition.Y = this.layerPosition.Y;
            this.verticalLayerPosition.X = this.layerPosition.X;

            this.diagonalLayerPosition.X = this.horizontalLayerPosition.X;
            this.diagonalLayerPosition.Y = this.verticalLayerPosition.Y;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, this.layerPosition, this._textureClippingRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            if (!this._lockX)
            {
                spriteBatch.Draw(this._texture, this.horizontalLayerPosition, this._textureClippingRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }

            if (!this._lockY)
            {
                spriteBatch.Draw(this._texture, this.verticalLayerPosition, this._textureClippingRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }

            if (!this._lockX && !this._lockY)
            {
                spriteBatch.Draw(this._texture, this.diagonalLayerPosition, this._textureClippingRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }
        }
    }
}
