using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Scenario
{
    internal sealed class CelestialBodyHandler(TimeHandler timeHandler, World world)
    {
        private float positionX;
        private float positionY;
        private float rotation;
        private float angle;

        private readonly TimeHandler timeHandler = timeHandler;
        private readonly World world = world;

        internal void Update()
        {
            // Calculate the angle of the celestial body
            this.angle = (BackgroundConstants.CELESTIAL_BODY_MAX_ARC_ANGLE * this.timeHandler.IntervalProgress) + BackgroundConstants.CELESTIAL_BODY_ARC_OFFSET;

            // Update position based on angle
            this.positionX = Convert.ToSingle(BackgroundConstants.CELESTIAL_BODY_CENTER_PIVOT.X - (BackgroundConstants.CELESTIAL_BODY_ARC_RADIUS * Math.Cos(this.angle)));
            this.positionY = Convert.ToSingle(BackgroundConstants.CELESTIAL_BODY_CENTER_PIVOT.Y - (BackgroundConstants.CELESTIAL_BODY_ARC_RADIUS * Math.Sin(this.angle)));

            // Update rotation for alignment
            this.rotation = this.angle - (MathF.PI / 2);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            // Determine the current day period
            DayPeriod dayPeriod = this.world.Time.GetCurrentDayPeriod();

            // Select the texture based on the time of day
            Rectangle rectangle = dayPeriod switch
            {
                DayPeriod.AnteLucan or DayPeriod.Night => new(32, 0, 32, 32),
                DayPeriod.Morning or DayPeriod.Afternoon => new(0, 0, 32, 32),
                _ => throw new InvalidOperationException("Unexpected day period."),
            };

            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.BgoCelestialBodies), new(this.positionX, this.positionY), rectangle, Color.White, this.rotation, Vector2.Zero, new Vector2(1.2f), SpriteEffects.None, 0f);
        }
    }
}
