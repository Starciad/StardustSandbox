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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Collections;

namespace StardustSandbox.Core.Explosions
{
    internal class Explosion : IPoolableObject
    {
        internal Point Position { get; private set; }

        internal float Radius { get; private set; }
        internal float Power { get; private set; }
        internal float Heat { get; private set; }

        internal bool AffectsWater { get; private set; }
        internal bool AffectsSolids { get; private set; }
        internal bool AffectsGases { get; private set; }

        internal Layer Layer { get; private set; }

        internal ElementIndex[] ExplosionResidues { get; private set; }

        internal void Build(in Point position, in Layer layer, in ExplosionBuilder builder)
        {
            this.Position = position;
            this.Layer = layer;

            this.Radius = builder.Radius;
            this.Power = builder.Power;
            this.Heat = builder.Heat;

            this.AffectsWater = builder.AffectsWater;
            this.AffectsSolids = builder.AffectsSolids;
            this.AffectsGases = builder.AffectsGases;

            this.ExplosionResidues = builder.ExplosionResidues;
        }

        public void Reset()
        {
            this.Position = Point.Zero;

            this.Radius = 0.0f;
            this.Power = 0.0f;
            this.Heat = 0.0f;

            this.AffectsWater = false;
            this.AffectsSolids = false;
            this.AffectsGases = false;

            this.ExplosionResidues = [];
        }
    }
}
