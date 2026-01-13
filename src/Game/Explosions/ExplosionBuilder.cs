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

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Explosions
{
    internal readonly struct ExplosionBuilder
    {
        internal readonly float Radius { get; init; }
        internal readonly float Power { get; init; }
        internal readonly float Heat { get; init; }

        internal readonly bool AffectsWater { get; init; }
        internal readonly bool AffectsSolids { get; init; }
        internal readonly bool AffectsGases { get; init; }

        internal readonly ElementIndex[] ExplosionResidues { get; init; }
    }
}

