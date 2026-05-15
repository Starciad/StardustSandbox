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

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Scenario;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Managers
{
    internal sealed class AmbientManager
    {
        internal BackgroundHandler BackgroundHandler => this.backgroundHandler;
        internal CelestialBodyHandler CelestialBodyHandler => this.celestialBodyHandler;

        private BackgroundHandler backgroundHandler;
        private CelestialBodyHandler celestialBodyHandler;
        private TimeHandler timeHandler;

        private readonly BackgroundDatabase backgroundDatabase;
        private readonly World world;

        internal AmbientManager(BackgroundDatabase backgroundDatabase, World world)
        {
            this.backgroundDatabase = backgroundDatabase;
            this.world = world;
        }

        internal void Initialize()
        {
            this.backgroundHandler = new(this.backgroundDatabase);
            this.timeHandler = new(this.world.Time);
            this.celestialBodyHandler = new(this.timeHandler, this.world);
        }

        internal void Update(GameTime gameTime)
        {
            this.timeHandler.Update();
            this.backgroundHandler.Update(gameTime);
            this.celestialBodyHandler.Update();
        }
    }
}
