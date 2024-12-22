using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Components;
using StardustSandbox.Core.Components.Common.World;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.IO.Files.World.Data;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.Data;

using System;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld : SGameObject, ISWorld
    {
        public SWorldInfo Infos { get; private set; } = new();

        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }

        private SWorldSlot[,] slots;
        private uint currentFramesUpdateDelay;

        private readonly uint totalFramesUpdateDelay = 5;

        private readonly SObjectPool worldSlotsPool;
        private readonly SComponentContainer componentContainer;
        private readonly SWorldChunkingComponent worldChunkingComponent;
        private readonly SElementContext worldElementContext;

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

            // Delay
            if (this.currentFramesUpdateDelay == 0)
            {
                this.currentFramesUpdateDelay = this.totalFramesUpdateDelay;
            }
            else
            {
                this.currentFramesUpdateDelay--;
                return;
            }

            // Update world
            this.componentContainer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsVisible)
            {
                return;
            }

            base.Draw(gameTime, spriteBatch);
            this.componentContainer.Draw(gameTime, spriteBatch);
        }

        public void StartNew()
        {
            StartNew(this.Infos.Size);
        }
        public void StartNew(SSize2 size)
        {
            this.Infos.Identifier = Guid.NewGuid().ToByteArray();

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
            // World
            StartNew(worldSaveFile.World.Size);

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
            InstantiateElement(position, worldLayer, worldSlotLayerData.ElementId);

            SWorldSlot worldSlot = (SWorldSlot)GetWorldSlot(position);

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

        public void Clear()
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

        public void Reset()
        {
            this.Infos.Name = "Untitled";
            this.Infos.Description = "No description was provided.";

            this.componentContainer.Reset();
            Clear();
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