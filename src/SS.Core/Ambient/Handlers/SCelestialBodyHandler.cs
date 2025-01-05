using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Ambient.Handlers;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Objects;

using System;

namespace StardustSandbox.Core.Ambient.Handlers
{
    internal sealed class SCelestialBodyHandler(ISGame gameInstance, ISTimeHandler timeHandler) : SGameObject(gameInstance), ISCelestialBodyHandler
    {
        public bool IsActive { get; set; }

        private Vector2 position;
        private float rotation;
        private double angle;

        private Texture2D sunTexture;
        private Texture2D moonTexture;

        private readonly ISWorld world = gameInstance.World;
        private readonly ISTimeHandler timeHandler = timeHandler;

        public override void Initialize()
        {
            this.sunTexture = this.SGameInstance.AssetDatabase.GetTexture("bgo_celestial_body_1");
            this.moonTexture = this.SGameInstance.AssetDatabase.GetTexture("bgo_celestial_body_2");
        }

        public override void Update(GameTime gameTime)
        {
            // Calculate the angle of the celestial body
            this.angle = (SBackgroundConstants.CELESTIAL_BODY_MAX_ARC_ANGLE * this.timeHandler.IntervalProgress) + SBackgroundConstants.CELESTIAL_BODY_ARC_OFFSET;

            // Update position based on angle
            this.position = new(
                SBackgroundConstants.CELESTIAL_BODY_CENTER_PIVOT.X - (SBackgroundConstants.CELESTIAL_BODY_ARC_RADIUS * MathF.Cos((float)this.angle)),
                SBackgroundConstants.CELESTIAL_BODY_CENTER_PIVOT.Y - (SBackgroundConstants.CELESTIAL_BODY_ARC_RADIUS * MathF.Sin((float)this.angle))
            );

            // Update rotation for alignment
            this.rotation = (float)this.angle - (MathF.PI / 2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Determine the current day period
            SDayPeriod dayPeriod = this.world.Time.GetCurrentDayPeriod();

            // Select the texture based on the time of day
            Texture2D texture = dayPeriod switch
            {
                SDayPeriod.AnteLucan or SDayPeriod.Night => this.moonTexture,
                SDayPeriod.Morning or SDayPeriod.Afternoon => this.sunTexture,
                _ => throw new InvalidOperationException("Unexpected day period."),
            };

            spriteBatch.Draw(texture, this.position, null, Color.White, this.rotation, Vector2.Zero, new Vector2(1.2f), SpriteEffects.None, 0f);
        }
    }
}
