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

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Scenario;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Managers
{
    internal sealed class AmbientManager : IResettable
    {
        internal BackgroundHandler BackgroundHandler => this.backgroundHandler;
        internal CelestialBodyHandler CelestialBodyHandler => this.celestialBodyHandler;
        internal CloudHandler CloudHandler => this.cloudHandler;

        private BackgroundHandler backgroundHandler;
        private CelestialBodyHandler celestialBodyHandler;
        private CloudHandler cloudHandler;
        private TimeHandler timeHandler;

        public void Reset()
        {
            this.cloudHandler.Reset();
        }

        internal void Initialize(World world)
        {
            this.backgroundHandler = new();
            this.cloudHandler = new();
            this.timeHandler = new(world.Time);
            this.celestialBodyHandler = new(this.timeHandler, world);
        }

        internal void Update(GameTime gameTime)
        {
            this.timeHandler.Update();
            this.backgroundHandler.Update(gameTime);
            this.celestialBodyHandler.Update();
            this.cloudHandler.Update(gameTime);
        }
    }
}
