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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

using System;

namespace StardustSandbox.Core.WorldSystem
{
    internal sealed class TemperatureRange(TimeSpan startTime, TimeSpan endTime) : IResettable
    {
        internal TimeSpan StartTime => startTime;
        internal TimeSpan EndTime => endTime;
        internal float Temperature
        {
            get => this.temperature;
            set => this.temperature = TemperatureMath.Clamp(value);
        }
        internal bool CanApplyTemperature
        {
            get => this.canApplyTemperature;
            set => this.canApplyTemperature = value;
        }

        private float temperature = TemperatureConstants.WORLD_NONE_TEMPERATURE;
        private bool canApplyTemperature = false;

        public void Reset()
        {
            this.temperature = TemperatureConstants.WORLD_NONE_TEMPERATURE;
            this.canApplyTemperature = false;
        }
    }
}
