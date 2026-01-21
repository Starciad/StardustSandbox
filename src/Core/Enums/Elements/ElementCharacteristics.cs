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

namespace StardustSandbox.Core.Enums.Elements
{
    [Flags]
    internal enum ElementCharacteristics : short
    {
        None = 0,
        AffectsNeighbors = 1 << 1,
        HasTemperature = 1 << 2,
        IsFlammable = 1 << 3,
        IsExplosive = 1 << 4,
        IsExplosionImmune = 1 << 5,
        IsCorruptible = 1 << 6,
        IsCorruption = 1 << 7,
        IsPushable = 1 << 8,
        IsConductive = 1 << 9,
    }
}

