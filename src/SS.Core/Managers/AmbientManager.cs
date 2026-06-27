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

using StardustSandbox.Core.Backgrounds;
using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Scenario;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Managers
{
    internal sealed class AmbientManager
    {
        internal bool CanDrawSky { get; set; }
        internal bool CanDrawCelestialBodies { get; set; }
        internal bool CanDrawBackground { get; set; }

        private readonly BackgroundHandler backgroundHandler;
        private readonly CelestialBodyHandler celestialBodyHandler;
        private readonly TimeHandler timeHandler;

        private readonly AssetDatabase assetDatabase;
        private readonly BackgroundDatabase backgroundDatabase;
        private readonly Camera2D camera;
        private readonly GameScreen gameScreen;
        private readonly World world;

        internal AmbientManager(AssetDatabase assetDatabase, BackgroundDatabase backgroundDatabase, Camera2D camera, GameScreen gameScreen, World world)
        {
            this.assetDatabase = assetDatabase;
            this.backgroundDatabase = backgroundDatabase;
            this.camera = camera;
            this.gameScreen = gameScreen;
            this.world = world;

            this.backgroundHandler = new();
            this.timeHandler = new(this.world.Time);
            this.celestialBodyHandler = new(this.assetDatabase, this.gameScreen, this.timeHandler, this.world);
        }

        internal void SetBackground(BackgroundIndex backgroundIndex)
        {
            this.backgroundHandler.Background = this.backgroundDatabase.GetBackground(backgroundIndex);
        }

        internal bool TryGetBackground(out Background background)
        {
            background = this.backgroundHandler.Background;
            return background != null;
        }

        internal void Update(GameTime gameTime)
        {
            this.timeHandler.Update();
            this.backgroundHandler.Update(gameTime);
            this.celestialBodyHandler.Update();
        }

        internal void DrawCelestialBodies(SpriteBatch spriteBatch)
        {
            this.celestialBodyHandler.Draw(spriteBatch);
        }

        internal void DrawBackground(SpriteBatch spriteBatch)
        {
            this.backgroundHandler.Draw(spriteBatch, this.camera, this.gameScreen);
        }
    }
}
