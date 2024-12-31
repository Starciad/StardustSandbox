using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Components;
using StardustSandbox.Core.Components.Common.World;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.IO.Files.World.Data;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.General;
using StardustSandbox.Core.World.Slots;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld : SGameObject, ISWorld
    {
        public SWorldInfo Infos { get; } = new();
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public int ActiveEntitiesCount => this.instantiatedEntities.Count;

        private SWorldSlot[,] slots;
        private uint currentFramesUpdateDelay;
        private SWorldSaveFile currentlySelectedWorldSaveFile;

        // World
        private readonly uint totalFramesUpdateDelay = 5;
        private readonly SComponentContainer componentContainer;
        private readonly SWorldChunkingComponent worldChunkingComponent;
        private readonly SElementContext worldElementContext;

        // Entities
        private readonly List<SEntity> instantiatedEntities = new(SEntityConstants.ACTIVE_ENTITIES_LIMIT);

        // Pools
        private readonly SObjectPool worldSlotsPool;
        private readonly Dictionary<string, SObjectPool> entityPools = [];

        public SWorld(ISGame gameInstance) : base(gameInstance)
        {
            this.worldSlotsPool = new();
            this.componentContainer = new(gameInstance);

            this.worldChunkingComponent = this.componentContainer.AddComponent(new SWorldChunkingComponent(gameInstance, this));
            _ = this.componentContainer.AddComponent(new SWorldUpdatingComponent(gameInstance, this));
            _ = this.componentContainer.AddComponent(new SWorldRenderingComponent(gameInstance, this));

            this.worldElementContext = new(this);
        }

        public override void Initialize()
        {
            this.componentContainer.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            UpdateWorld(gameTime);
            UpdateEntities(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsVisible)
            {
                return;
            }

            DrawWorld(gameTime, spriteBatch);
            DrawEntities(gameTime, spriteBatch);
        }

        private void UpdateWorld(GameTime gameTime)
        {
            if (this.currentFramesUpdateDelay > 0)
            {
                this.currentFramesUpdateDelay--;
                return;
            }

            this.currentFramesUpdateDelay = this.totalFramesUpdateDelay;
            this.componentContainer.Update(gameTime);
        }

        private void UpdateEntities(GameTime gameTime)
        {
            this.instantiatedEntities.ForEach(entity =>
            {
                if (entity == null)
                {
                    return;
                }

                entity.Update(gameTime);
            });
        }

        private void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.componentContainer.Draw(gameTime, spriteBatch);
        }

        private void DrawEntities(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.instantiatedEntities.ForEach(entity =>
            {
                if (entity == null)
                {
                    return;
                }

                entity.Draw(gameTime, spriteBatch);
            });
        }

        public void StartNew()
        {
            StartNew(this.Infos.Size);
        }
        public void StartNew(SSize2 size)
        {
            this.Infos.Identifier = Guid.NewGuid().ToString();

            this.IsActive = true;
            this.IsVisible = true;

            if (this.Infos.Size != size)
            {
                Resize(size);
            }

            Reset();
        }

        public void LoadFromWorldSaveFile(SWorldSaveFile worldSaveFile)
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = true;

            // World
            StartNew(worldSaveFile.World.Size);

            // Cache
            this.currentlySelectedWorldSaveFile = worldSaveFile;

            // Metadata
            this.Infos.Identifier = worldSaveFile.Metadata.Identifier;
            this.Infos.Name = worldSaveFile.Metadata.Name;
            this.Infos.Description = worldSaveFile.Metadata.Description;

            // Allocate Elements
            foreach (SWorldSlotData worldSlotData in worldSaveFile.World.Slots)
            {
                if (worldSlotData.ForegroundLayer != null)
                {
                    LoadWorldSlotLayerData(SWorldLayer.Foreground, worldSlotData.Position, worldSlotData.ForegroundLayer);
                }

                if (worldSlotData.BackgroundLayer != null)
                {
                    LoadWorldSlotLayerData(SWorldLayer.Background, worldSlotData.Position, worldSlotData.BackgroundLayer);
                }
            }
        }
        private void LoadWorldSlotLayerData(SWorldLayer worldLayer, Point position, SWorldSlotLayerData worldSlotLayerData)
        {
            InstantiateElement(position, worldLayer, worldSlotLayerData.ElementIdentifier);

            SWorldSlot worldSlot = GetWorldSlot(position);

            worldSlot.SetTemperatureValue(worldLayer, worldSlotLayerData.Temperature);
            worldSlot.SetFreeFalling(worldLayer, worldSlotLayerData.FreeFalling);
            worldSlot.SetColorModifier(worldLayer, worldSlotLayerData.ColorModifier);
        }

        public void Resize(SSize2 size)
        {
            DestroyWorldSlots();

            this.Infos.Size = size;
            this.slots = new SWorldSlot[size.Width, size.Height];

            InstantiateWorldSlots();
        }

        public void Reload()
        {
            if (this.currentlySelectedWorldSaveFile != null)
            {
                LoadFromWorldSaveFile(this.currentlySelectedWorldSaveFile);
            }
            else
            {
                Clear();
            }
        }

        public void Reset()
        {
            this.Infos.Name = string.Empty;
            this.Infos.Description = string.Empty;
            this.currentlySelectedWorldSaveFile = null;
            this.componentContainer.Reset();
            Clear();
        }

        public void Clear()
        {
            ClearSlots();
            ClearEntities();
        }

        private void ClearSlots()
        {
            if (this.slots == null)
            {
                return;
            }

            for (int x = 0; x < this.Infos.Size.Width; x++)
            {
                for (int y = 0; y < this.Infos.Size.Height; y++)
                {
                    if (IsEmptyWorldSlot(new(x, y)))
                    {
                        continue;
                    }

                    DestroyElement(new(x, y), SWorldLayer.Foreground);
                    DestroyElement(new(x, y), SWorldLayer.Background);
                }
            }
        }

        private void ClearEntities()
        {
            RemoveAllEntity();
        }

        public bool InsideTheWorldDimensions(Point position)
        {
            return position.X >= 0 && position.X < this.Infos.Size.Width &&
                   position.Y >= 0 && position.Y < this.Infos.Size.Height;
        }

        private void InstantiateWorldSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.Infos.Size.Width; x++)
                {
                    this.slots[x, y] = this.worldSlotsPool.TryGet(out ISPoolableObject value) ? (SWorldSlot)value : new();
                }
            }
        }

        private void DestroyWorldSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.Infos.Size.Width; x++)
                {
                    if (this.slots[x, y] == null)
                    {
                        continue;
                    }

                    this.worldSlotsPool.Add(this.slots[x, y]);
                    this.slots[x, y] = null;
                }
            }
        }

        public static Vector2 ToWorldPosition(Vector2 globalPosition)
        {
            return new Vector2(
                (int)(globalPosition.X / SWorldConstants.GRID_SCALE),
                (int)(globalPosition.Y / SWorldConstants.GRID_SCALE)
            );
        }

        public static Vector2 ToGlobalPosition(Vector2 worldPosition)
        {
            return new Vector2(
                (int)(worldPosition.X * SWorldConstants.GRID_SCALE),
                (int)(worldPosition.Y * SWorldConstants.GRID_SCALE)
            );
        }
    }
}