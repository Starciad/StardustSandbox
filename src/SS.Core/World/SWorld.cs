using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Components;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.Components;
using StardustSandbox.Core.World.Data;

namespace StardustSandbox.Core.World
{
    public sealed partial class SWorld : SGameObject, ISReset
    {
        public SWorldState States { get; private set; } = new();
        public SWorldInfo Infos { get; private set; } = new();

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
            this.States.IsActive = false;
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
            this.componentContainer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (!this.States.IsActive)
            {
                return;
            }

            this.componentContainer.Draw(gameTime, spriteBatch);
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
    }
}