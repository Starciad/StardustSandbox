using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Background.Handlers;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Objects;

using System;

namespace StardustSandbox.Core.Background.Handlers
{
    internal sealed class SCelestialBodyHandler(ISGame gameInstance) : SGameObject(gameInstance), ISCelestialBodyHandler
    {
        public bool IsActive { get; set; }

        private Vector2 position;
        private float rotation;

        private Texture2D sunTexture;
        private Texture2D moonTexture;

        private readonly ISWorld world = gameInstance.World;

        public override void Initialize()
        {
            this.sunTexture = this.SGameInstance.AssetDatabase.GetTexture("bgo_celestial_body_1");
            this.moonTexture = this.SGameInstance.AssetDatabase.GetTexture("bgo_celestial_body_2");
        }

        public override void Update(GameTime gameTime)
        {
            float currentSeconds = (float)this.world.Time.CurrentTime.TotalSeconds;

            // Determine if it's day or night
            bool isDay = currentSeconds >= STimeConstants.DAY_START_IN_SECONDS && currentSeconds < STimeConstants.NIGHT_START_IN_SECONDS;

            // Calculate normalized time for the active interval
            float intervalDuration = isDay
                ? STimeConstants.NIGHT_START_IN_SECONDS - STimeConstants.DAY_START_IN_SECONDS // Day duration
                : STimeConstants.SECONDS_IN_A_DAY - (STimeConstants.NIGHT_START_IN_SECONDS - STimeConstants.DAY_START_IN_SECONDS); // Night duration

            float intervalProgress = isDay
                ? (currentSeconds - STimeConstants.DAY_START_IN_SECONDS) / intervalDuration
                : currentSeconds >= STimeConstants.NIGHT_START_IN_SECONDS
                    ? (currentSeconds - STimeConstants.NIGHT_START_IN_SECONDS) / intervalDuration
                    : (currentSeconds + STimeConstants.SECONDS_IN_A_DAY - STimeConstants.NIGHT_START_IN_SECONDS) / intervalDuration;

            // Calculate the angle of the celestial body
            float maxArcAngle = SBackgroundConstants.CELESTIAL_BODY_MAX_ARC_ANGLE;
            float angle = (maxArcAngle * (float)intervalProgress) + SBackgroundConstants.CELESTIAL_BODY_ARC_OFFSET;

            // Update position based on angle
            this.position = new(
                SBackgroundConstants.CELESTIAL_BODY_CENTER_PIVOT.X - (SBackgroundConstants.CELESTIAL_BODY_ARC_RADIUS * MathF.Cos(angle)),
                SBackgroundConstants.CELESTIAL_BODY_CENTER_PIVOT.Y - (SBackgroundConstants.CELESTIAL_BODY_ARC_RADIUS * MathF.Sin(angle))
            );

            // Update rotation for alignment
            this.rotation = angle - (MathF.PI / 2);
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
