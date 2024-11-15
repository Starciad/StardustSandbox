using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.Components;
using StardustSandbox.Core.World.Data;

using System;

namespace StardustSandbox.Core.World
{
    public sealed partial class SWorld : SGameObject, ISReset
    {
        public SWorldState States { get; private set; } = new();
        public SWorldInfo Infos { get; private set; } = new();

        private readonly SObjectPool worldSlotsPool = new();
        private readonly SWorldComponent[] _components;

        private SWorldSlot[,] slots;

        private readonly uint totalFramesUpdateDelay = 5;
        private uint currentFramesUpdateDelay;

        public SWorld(ISGame gameInstance) : base(gameInstance)
        {
            this._components = [
                new SWorldChunkingComponent(gameInstance, this),
                new SWorldUpdatingComponent(gameInstance, this),
                new SWorldRenderingComponent(gameInstance, this)
            ];
        }

        public override void Initialize()
        {
            Resize(SWorldConstants.WORLD_SIZES_TEMPLATE[2]);
            Reset();

            foreach (SWorldComponent component in this._components)
            {
                component.Initialize();
            }

            this.States.IsActive = true;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Check current game status
            if (!this.States.IsActive || this.States.IsPaused)
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
            for (int i = 0; i < this._components.Length; i++)
            {
                this._components[i].Update(gameTime);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (!this.States.IsActive)
            {
                return;
            }

            for (int i = 0; i < this._components.Length; i++)
            {
                this._components[i].Draw(gameTime, spriteBatch);
            }
        }

        public T GetComponent<T>() where T : SWorldComponent
        {
            return (T)Array.Find(this._components, x => x.GetType() == typeof(T));
        }
        public void Reset()
        {
            Clear();
        }
        public void Pause()
        {
            this.States.IsPaused = true;
        }
        public void Resume()
        {
            this.States.IsPaused = false;
        }
        public void Resize(SSize2 size)
        {
            this.Infos.SetSize(size);

            DestroyWorldSlots();
            this.slots = new SWorldSlot[(int)size.Width, (int)size.Height];
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
    }
}