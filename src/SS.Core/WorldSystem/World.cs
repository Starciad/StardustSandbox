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

using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem.Components;
using StardustSandbox.Core.WorldSystem.Handlers;
using StardustSandbox.Core.WorldSystem.Information;

namespace StardustSandbox.Core.WorldSystem
{
    internal sealed class World : IResettable
    {
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal Point Size { get; set; }

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        internal Temperature Temperature => this.temperature;
        internal TileMap TileMap => this.tileMap;
        internal Time Time => this.time;

        internal RenderingHandler RenderingHandler => this.renderingHandler;

        private readonly Simulation simulation;
        private readonly Temperature temperature;
        private readonly TileMap tileMap;
        private readonly Time time;

        private readonly ChunkHandler chunkHandler;
        private readonly ExplosionHandler explosionHandler;
        private readonly RenderingHandler renderingHandler;
        private readonly StatisticsHandler statisticsHandler;
        private readonly UpdateHandler updateHandler;

        private readonly ElementContext worldElementContext;
        private readonly WorldSerializer worldSerializer;

        private readonly AssetDatabase assetDatabase;

        internal World(
            AchievementManager achievementManager,
            AssetDatabase assetDatabase,
            ElementDatabase elementDatabase,
            PlayerInputController playerInputController
        )
        {
            this.assetDatabase = assetDatabase;

            this.simulation = new();
            this.time = new();
            this.temperature = new(this.time);

            this.tileMap = new(elementDatabase);

            this.chunkHandler = new(this.TileMap);
            this.explosionHandler = new(this.tileMap);
            this.renderingHandler = new(assetDatabase, playerInputController, this);
            this.statisticsHandler = new(achievementManager);
            this.updateHandler = new(this);

            this.worldElementContext = new(this);
            this.worldSerializer = new(this);

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            this.tileMap.OnElementInstantiatedHandler += (position, layer, index) =>
            {
                this.chunkHandler.NotifyChunk(position);
            };

            this.tileMap.OnElementPositionUpdatedHandler += (oldPosition, newPosition, layer) =>
            {
                this.chunkHandler.NotifyChunk(oldPosition);
                this.chunkHandler.NotifyChunk(newPosition);
            };

            this.tileMap.OnElementSwappedHandler += (position1, position2, layer) =>
            {
                this.chunkHandler.NotifyChunk(position1);
                this.chunkHandler.NotifyChunk(position2);
            };

            this.tileMap.OnElementDestroyedHandler += (position, layer) =>
            {
                this.chunkHandler.NotifyChunk(position);
            };

            this.tileMap.OnElementRemovedHandler += (position, layer) =>
            {
                this.chunkHandler.NotifyChunk(position);
            };

            this.tileMap.OnElementReplacedHandler += (position, layer, oldIndex, index) =>
            {
                this.chunkHandler.NotifyChunk(position);
            };

            this.tileMap.OnElementTemperatureChangedHandler += (position, layer, temperature) =>
            {
                this.chunkHandler.NotifyChunk(position);
            };
        }

        public void Reset()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;

            this.statisticsHandler.ResetWorldStatistics();

            this.chunkHandler.Reset();
            this.temperature.Reset();
            this.updateHandler.Reset();

            this.tileMap.Clear();
        }

        internal void StartNew(Point size)
        {
            this.CanUpdate = true;
            this.CanDraw = true;

            if (this.Size != size)
            {
                this.tileMap.Resize(size);
            }

            Reset();
        }

        internal void StartNew()
        {
            StartNew(this.Size);
        }

        internal void Reload(bool hasSaveFileLoaded, string loadedSaveFileName)
        {
            if (hasSaveFileLoaded)
            {
                this.worldSerializer.Deserialize(loadedSaveFileName);
                return;
            }

            this.tileMap.Clear();
        }

        internal void SetSpeed(SimulationSpeed speed)
        {
            this.time.SetSpeed(speed);
            this.simulation.SetSpeed(speed);
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.CanUpdate)
            {
                return;
            }

            this.time.Update(gameTime);
            this.simulation.Update(gameTime);
            this.temperature.Update();

            if (this.simulation.CanContinueExecution())
            {
                this.chunkHandler.Update();
                this.updateHandler.Update(gameTime);
            }

            this.explosionHandler.HandleExplosions();
        }

        internal void Draw(SpriteBatch spriteBatch, Camera2D camera, GameLaunchOptions options)
        {
            if (!this.CanDraw)
            {
                return;
            }

            if (options.ShowChunks)
            {
                this.chunkHandler.Draw(spriteBatch, this.assetDatabase);
            }

            this.renderingHandler.Draw(spriteBatch, this.assetDatabase, camera);
        }
    }
}
