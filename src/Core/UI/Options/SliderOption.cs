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

using System;

namespace StardustSandbox.Core.UI.Options
{
    internal sealed class SliderOption(string name, string description, int minimumValue, int maximumValue) : Option(name, description)
    {
        internal int MinimumValue => minimumValue;
        internal int MaximumValue => maximumValue;

        private int currentValue;

        internal override object GetValue()
        {
            return this.currentValue;
        }

        internal override void SetValue(object value)
        {
            this.currentValue = int.Clamp(Convert.ToInt32(value), this.MinimumValue, this.MaximumValue);
        }
    }
}

