using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Collections;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.GameContent.World.Components;
using StardustSandbox.Game.Interfaces.General;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.Objects;
using StardustSandbox.Game.World.Components;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.World
{
    public sealed partial class SWorld : SGameObject, ISReset
    {
        public SWorldState States { get; private set; }
        public SWorldInfo Infos { get; private set; }
        public SElementDatabase ElementDatabase { get; private set; }

        private readonly SObjectPool worldSlotsPool;
        private readonly SWorldComponent[] _components;

        private SWorldSlot[,] slots;

        private readonly uint totalFramesUpdateDelay = 5;
        private uint currentFramesUpdateDelay;

        public SWorld(SGame gameInstance, SElementDatabase elementDatabase, SAssetDatabase assetDatabase, SCameraManager camera) : base(gameInstance)
        {
            this.States = new();
            this.Infos = new();
            this.ElementDatabase = elementDatabase;

            this.worldSlotsPool = new();

            this._components = [
                new SWorldChunkingComponent(gameInstance, this, assetDatabase),
                new SWorldUpdatingComponent(gameInstance, this),
                new SWorldRenderingComponent(gameInstance, this, elementDatabase, camera)
            ];
        }

        public override void Initialize()
        {
            this.slots = new SWorldSlot[(int)this.Infos.Size.Width, (int)this.Infos.Size.Height];

            InstantiateWorldSlots();
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
            foreach (SWorldComponent component in this._components)
            {
                component.Update(gameTime);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (!this.States.IsActive)
            {
                return;
            }

            foreach (SWorldComponent component in this._components)
            {
                component.Draw(gameTime, spriteBatch);
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