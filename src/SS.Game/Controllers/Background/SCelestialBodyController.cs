using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Objects;

using System;

namespace StardustSandbox.Game.Background.Details
{
    public class SCelestialBodyController : SGameObject
    {
        private readonly Texture2D _sunTexture;
        private readonly Texture2D _moonTexture;
        private readonly Texture2D _starsTexture;

        private Vector2 _position;
        private float _rotationAngle;
        private float _cycleDuration;
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
            _rotationAngle += (float)(MathHelper.TwoPi / _cycleDuration) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _isDay = _rotationAngle <= MathHelper.Pi;

            if (_rotationAngle >= MathHelper.TwoPi)
            {
                _rotationAngle = 0f;
            }

            _position.X = (float)Math.Cos(_rotationAngle) * SScreenConstants.DEFAULT_SCREEN_WIDTH / 2 + SScreenConstants.DEFAULT_SCREEN_WIDTH / 2;
            _position.Y = (float)Math.Sin(_rotationAngle) * SScreenConstants.DEFAULT_SCREEN_HEIGHT / 2 + SScreenConstants.DEFAULT_SCREEN_HEIGHT / 2;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_isDay)
            {
                spriteBatch.Draw(_sunTexture, _position, Color.White);
            }
            else
            {
                spriteBatch.Draw(_moonTexture, _position, Color.White);
                spriteBatch.Draw(_starsTexture, Vector2.Zero, Color.White);
            }
        }
    }
}
