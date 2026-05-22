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
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem.Components;
using StardustSandbox.Core.WorldSystem.Handlers;

namespace StardustSandbox.Core.WorldSystem
{
    internal sealed class World : IResettable
    {
        internal string Name { get; set; }
        internal string Description { get; set; }

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        internal Temperature Temperature => this.temperature;
        internal TileMap TileMap => this.tileMap;
        internal Time Time => this.time;

        internal ChunkHandler ChunkHandler => this.chunkHandler;
        internal ExplosionHandler ExplosionHandler => this.explosionHandler;
        internal RenderingHandler RenderingHandler => this.renderingHandler;

        internal WorldSerializer Serializer => this.serializer;

        private readonly Simulation simulation;
        private readonly Temperature temperature;
        private readonly TileMap tileMap;
        private readonly Time time;

        private readonly ChunkHandler chunkHandler;
        private readonly ExplosionHandler explosionHandler;
        private readonly RenderingHandler renderingHandler;
        private readonly StatisticsHandler statisticsHandler;
        private readonly UpdateHandler updateHandler;

        private readonly ElementContext elementContext;
        private readonly WorldSerializer serializer;

        private readonly AssetDatabase assetDatabase;
        private readonly ElementDatabase elementDatabase;

        internal World(
            AchievementManager achievementManager,
            AssetDatabase assetDatabase,
            ElementDatabase elementDatabase,
            GameEvents gameEvents,
            PlayerInputController playerInputController
        )
        {
            this.assetDatabase = assetDatabase;
            this.elementDatabase = elementDatabase;

            this.simulation = new();
            this.time = new();
            this.temperature = new(this.time);

            this.tileMap = new(elementDatabase);

            this.chunkHandler = new(this.TileMap);
            this.explosionHandler = new(gameEvents, this.tileMap);
            this.renderingHandler = new(assetDatabase, playerInputController, this);
            this.statisticsHandler = new(achievementManager);
            this.updateHandler = new(this);

            this.elementContext = new(this);
            this.serializer = new(this);

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            this.tileMap.OnElementInstantiated += OnElementInstantiated;
            this.tileMap.OnElementPositionUpdated += OnElementPositionUpdated;
            this.tileMap.OnElementSwapped += OnElementSwapped;
            this.tileMap.OnElementDestroyed += OnElementDestroyed;
            this.tileMap.OnElementRemoved += OnElementRemoved;
            this.tileMap.OnElementReplaced += OnElementReplaced;
            this.tileMap.OnElementTemperatureChanged += OnElementTemperatureChanged;
        }

        #region EVENTS

        private void OnElementInstantiated(Point position, Layer layer, ElementIndex index)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(position);

            // Statistics System
            this.statisticsHandler.RegisterInstantiatedElement(index);

            // Element Context
            this.elementContext.Initialize(position, layer);

            Element element = this.elementDatabase.GetElement(index);
            element.SetContext(this.elementContext);
            element.Instantiate();
        }

        private void OnElementPositionUpdated(Point oldPosition, Point newPosition, Layer layer)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(oldPosition);
            this.chunkHandler.NotifyChunk(newPosition);
        }

        private void OnElementSwapped(Point position1, Point position2, Layer layer)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(position1);
            this.chunkHandler.NotifyChunk(position2);
        }

        private void OnElementDestroyed(Point position, Layer layer, ElementIndex index)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(position);

            // Element Context
            this.elementContext.Initialize(position, layer);

            Element element = this.elementDatabase.GetElement(index);
            element.SetContext(this.elementContext);
            element.Destroy();
        }

        private void OnElementRemoved(Point position, Layer layer)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(position);
        }

        private void OnElementReplaced(Point position, Layer layer, ElementIndex oldIndex, ElementIndex index)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(position);
        }

        private void OnElementTemperatureChanged(Point position, Layer layer, float temperature)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(position);
        }

        #endregion

        internal void Clear()
        {
            this.tileMap.Clear();
        }

        public void Reset()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;

            this.statisticsHandler.ResetWorldStatistics();

            this.chunkHandler.Reset();
            this.temperature.Reset();
            this.updateHandler.Reset();

            Clear();
        }

        internal void StartNew(Point size)
        {
            this.CanUpdate = true;
            this.CanDraw = true;

            if (this.tileMap.Size != size)
            {
                this.tileMap.Resize(size);
            }

            Reset();
        }

        internal void StartNew()
        {
            StartNew(this.tileMap.Size);
        }

        internal void Reload(bool hasSaveFileLoaded, string loadedSaveFileName)
        {
            if (hasSaveFileLoaded)
            {
                this.serializer.Deserialize(loadedSaveFileName);
                return;
            }

            Clear();
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
