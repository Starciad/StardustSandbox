/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Constants;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces;

using System;

namespace StardustSandbox.WorldSystem
{
    internal sealed class Simulation : IResettable
    {
        private float delayThresholdSeconds = SimulationConstants.NORMAL_SPEED_DELAY_SECONDS;
        private float accumulatedTimeSeconds;

        public void Reset()
        {
            this.delayThresholdSeconds = SimulationConstants.NORMAL_SPEED_DELAY_SECONDS;
            this.accumulatedTimeSeconds = 0.0f;
        }

        internal void Update(GameTime gameTime)
        {
            this.accumulatedTimeSeconds += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        }

        internal void SetSpeed(in SimulationSpeed speed)
        {
            this.delayThresholdSeconds = speed switch
            {
                SimulationSpeed.Normal => SimulationConstants.NORMAL_SPEED_DELAY_SECONDS,
                SimulationSpeed.Fast => SimulationConstants.FAST_SPEED_DELAY_SECONDS,
                SimulationSpeed.VeryFast => SimulationConstants.VERY_FAST_SPEED_DELAY_SECONDS,
                _ => SimulationConstants.NORMAL_SPEED_DELAY_SECONDS,
            };
        }

        internal bool CanContinueExecution()
        {
            if (this.accumulatedTimeSeconds >= this.delayThresholdSeconds)
            {
                this.accumulatedTimeSeconds = 0.0f;
                return true;
            }

            return false;
        }
    }
}

