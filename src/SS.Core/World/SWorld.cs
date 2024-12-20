﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Components;
using StardustSandbox.Core.Components.Common.World;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.Data;

namespace StardustSandbox.Core.World
{
    public sealed partial class SWorld : SGameObject, ISReset
    {
        public SWorldInfo Infos { get; private set; } = new();

        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }

        private readonly SObjectPool worldSlotsPool = new();
        private readonly SComponentContainer componentContainer;

        private readonly SWorldChunkingComponent worldChunkingComponent;

        private SWorldSlot[,] slots;

        private readonly uint totalFramesUpdateDelay = 5;
        private uint currentFramesUpdateDelay;

        public SWorld(ISGame gameInstance) : base(gameInstance)
        {
            this.componentContainer = new(gameInstance);

            this.worldChunkingComponent = this.componentContainer.AddComponent(new SWorldChunkingComponent(gameInstance, this));
            _ = this.componentContainer.AddComponent(new SWorldUpdatingComponent(gameInstance, this));
            _ = this.componentContainer.AddComponent(new SWorldRenderingComponent(gameInstance, this));
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

        public void Reset()
        {
            this.componentContainer.Reset();
            Clear();
        }

        public void Resize(SSize2 size)
        {
            DestroyWorldSlots();

            this.Infos.SetSize(size);
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
                    if (IsEmptyElementSlot(new(x, y)))
                    {
                        continue;
                    }

                    DestroyElement(new(x, y));
                }
            }
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