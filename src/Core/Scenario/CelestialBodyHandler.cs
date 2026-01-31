/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.Scenario
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
