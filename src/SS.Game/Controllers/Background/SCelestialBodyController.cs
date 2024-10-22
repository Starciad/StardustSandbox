using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Objects;

using System;

namespace StardustSandbox.Game.Controllers.Background
{
    public class SCelestialBodyController : SGameObject
    {
        private readonly Texture2D _sunTexture;
        private readonly Texture2D _moonTexture;
        private readonly Texture2D _starsTexture;

        private Vector2 _position;
        private float _rotationAngle;
        private readonly float _cycleDuration;
        private bool _isDay;

        public SCelestialBodyController(SGame gameInstance, Texture2D sunTexture, Texture2D moonTexture, Texture2D starsTexture, float cycleDuration) : base(gameInstance)
        {
            this._sunTexture = sunTexture;
            this._moonTexture = moonTexture;
            this._starsTexture = starsTexture;
            this._cycleDuration = cycleDuration;
            this._rotationAngle = 0f;
            this._isDay = true;
        }

        public override void Update(GameTime gameTime)
        {
            this._rotationAngle += (float)(MathHelper.TwoPi / this._cycleDuration) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            this._isDay = this._rotationAngle <= MathHelper.Pi;

            if (this._rotationAngle >= MathHelper.TwoPi)
            {
                this._rotationAngle = 0f;
            }

            this._position.X = ((float)Math.Cos(this._rotationAngle) * SScreenConstants.DEFAULT_SCREEN_WIDTH / 2) + (SScreenConstants.DEFAULT_SCREEN_WIDTH / 2);
            this._position.Y = ((float)Math.Sin(this._rotationAngle) * SScreenConstants.DEFAULT_SCREEN_HEIGHT / 2) + (SScreenConstants.DEFAULT_SCREEN_HEIGHT / 2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this._isDay)
            {
                spriteBatch.Draw(this._sunTexture, this._position, Color.White);
            }
            else
            {
                spriteBatch.Draw(this._moonTexture, this._position, Color.White);
                spriteBatch.Draw(this._starsTexture, Vector2.Zero, Color.White);
            }
        }
    }
}
