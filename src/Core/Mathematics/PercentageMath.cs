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

namespace StardustSandbox.Core.Mathematics
{
    internal static class PercentageMath
    {
        internal static float PercentageOfValue(float value, float percentage)
        {
            return percentage < 0 || percentage > 100
                ? throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 1 and 100.")
                : value * percentage / 100f;
        }

        internal static float PercentageFromValue(float total, float partial)
        {
            return total == 0 ? throw new ArgumentException("Total value cannot be zero.", nameof(total)) : partial / total * 100f;
        }
    }
}

