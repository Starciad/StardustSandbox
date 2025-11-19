using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.AmbientSystem.Handlers
{
    internal sealed class CelestialBodyHandler(TimeHandler timeHandler, World world)
    {
        internal bool IsActive { get; set; }

        private Vector2 position;
        private float rotation;
        private float angle;

        private readonly Texture2D sunTexture = AssetDatabase.GetTexture("texture_bgo_celestial_body_1");
        private readonly Texture2D moonTexture = AssetDatabase.GetTexture("texture_bgo_celestial_body_2");

        private readonly TimeHandler timeHandler = timeHandler;
        private readonly World world = world;

        internal void Update()
        {
            // Calculate the angle of the celestial body
            this.angle = (BackgroundConstants.CELESTIAL_BODY_MAX_ARC_ANGLE * this.timeHandler.IntervalProgress) + BackgroundConstants.CELESTIAL_BODY_ARC_OFFSET;

            // Update position based on angle
            this.position = new(
                BackgroundConstants.CELESTIAL_BODY_CENTER_PIVOT.X - (BackgroundConstants.CELESTIAL_BODY_ARC_RADIUS * MathF.Cos(this.angle)),
                BackgroundConstants.CELESTIAL_BODY_CENTER_PIVOT.Y - (BackgroundConstants.CELESTIAL_BODY_ARC_RADIUS * MathF.Sin(this.angle))
            );

            // Update rotation for alignment
            this.rotation = this.angle - (MathF.PI / 2);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            // Determine the current day period
            DayPeriod dayPeriod = this.world.Time.GetCurrentDayPeriod();

            // Select the texture based on the time of day
            Texture2D texture = dayPeriod switch
            {
                DayPeriod.AnteLucan or DayPeriod.Night => this.moonTexture,
                DayPeriod.Morning or DayPeriod.Afternoon => this.sunTexture,
                _ => throw new InvalidOperationException("Unexpected day period."),
            };

            spriteBatch.Draw(texture, this.position, null, Color.White, this.rotation, Vector2.Zero, new Vector2(1.2f), SpriteEffects.None, 0f);
        }
    }
}
