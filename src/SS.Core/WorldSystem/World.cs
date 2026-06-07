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
using StardustSandbox.Core.Events.Elements;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Serialization.Data.Worlds;
using StardustSandbox.Core.Serialization.Settings.Common;
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

        internal WorldSerializationHelper SerializationHelper => this.serializationHelper;

        private readonly Simulation simulation;
        private readonly Temperature temperature;
        private readonly TileMap tileMap;
        private readonly Time time;

        private readonly ChunkHandler chunkHandler;
        private readonly ExplosionHandler explosionHandler;
        private readonly RenderingHandler renderingHandler;
        private readonly UpdateHandler updateHandler;

        private readonly ElementContext elementContext;
        private readonly WorldSerializationHelper serializationHelper;

        private readonly AssetDatabase assetDatabase;
        private readonly ElementDatabase elementDatabase;

        internal World(
            AssetDatabase assetDatabase,
            ElementDatabase elementDatabase,
            GameEvents gameEvents,
            GameplaySettings gameplaySettings,
            PlayerInputController playerInputController,
            WorldSerializer worldSerializer
        )
        {
            this.assetDatabase = assetDatabase;
            this.elementDatabase = elementDatabase;

            this.simulation = new();
            this.time = new();
            this.temperature = new(this.time);

            this.tileMap = new(elementDatabase, gameEvents);

            this.chunkHandler = new(this.TileMap);
            this.explosionHandler = new(gameEvents, this.tileMap);
            this.renderingHandler = new(assetDatabase, gameplaySettings, playerInputController, this);
            this.updateHandler = new(this);

            this.elementContext = new(this);
            this.serializationHelper = new(this, worldSerializer);

            RegisterEvents(gameEvents);
        }

        private void RegisterEvents(GameEvents gameEvents)
        {
            gameEvents.Subscribe<ElementDestroyedEvent>(OnElementDestroyed);
            gameEvents.Subscribe<ElementInstantiatedEvent>(OnElementInstantiated);
            gameEvents.Subscribe<ElementPositionUpdatedEvent>(OnElementPositionUpdated);
            gameEvents.Subscribe<ElementRemovedEvent>(OnElementRemoved);
            gameEvents.Subscribe<ElementReplacedEvent>(OnElementReplaced);
            gameEvents.Subscribe<ElementSwappedEvent>(OnElementSwapped);
            gameEvents.Subscribe<ElementTemperatureChangedEvent>(OnElementTemperatureChanged);
        }

        #region EVENTS

        private void OnElementDestroyed(ElementDestroyedEvent e)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(e.Position);

            // Element Context
            this.elementContext.Initialize(e.Position, e.Layer);

            Element element = this.elementDatabase.GetElement(e.Index);
            element.SetContext(this.elementContext);
            element.Destroy();
        }
        private void OnElementInstantiated(ElementInstantiatedEvent e)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(e.Position);

            // Element Context
            this.elementContext.Initialize(e.Position, e.Layer);

            Element element = this.elementDatabase.GetElement(e.Index);
            element.SetContext(this.elementContext);
            element.Instantiate();
        }
        private void OnElementPositionUpdated(ElementPositionUpdatedEvent e)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(e.OldPosition);
            this.chunkHandler.NotifyChunk(e.NewPosition);
        }
        private void OnElementRemoved(ElementRemovedEvent e)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(e.Position);
        }
        private void OnElementReplaced(ElementReplacedEvent e)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(e.Position);

            this.elementContext.Initialize(e.Position, e.Layer);

            Element newElement = this.elementDatabase.GetElement(e.NewIndex);
            newElement.SetContext(this.elementContext);
            newElement.Instantiate();
        }
        private void OnElementSwapped(ElementSwappedEvent e)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(e.Position1);
            this.chunkHandler.NotifyChunk(e.Position2);
        }
        private void OnElementTemperatureChanged(ElementTemperatureChangedEvent e)
        {
            // Chunk System
            this.chunkHandler.NotifyChunk(e.Position);
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
                this.serializationHelper.Deserialize(loadedSaveFileName);
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
