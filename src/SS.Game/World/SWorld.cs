using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.General;
using StardustSandbox.Game.Interfaces.General;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.Objects;
using StardustSandbox.Game.World.Components;
using StardustSandbox.Game.World.Components.Common;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.World
{
    public sealed partial class SWorld(SElementDatabase elementDatabase, SAssetDatabase assetDatabase, SCameraManager camera) : SGameObject, ISReset
    {
        public SWorldState States { get; private set; } = new();
        public SWorldInfo Infos { get; private set; } = new();
        public SElementDatabase ElementDatabase => elementDatabase;

        private readonly STimer updateTimer = new(0.35f);
        private readonly SWorldComponent[] _components =
        [
            new SWorldChunkingComponent(assetDatabase),
            new SWorldUpdatingComponent(),
            new SWorldRenderingComponent(elementDatabase, camera)
        ];

        private SWorldSlot[,] slots;

        protected override void OnAwake()
        {
            base.OnAwake();

            this.slots = new SWorldSlot[(int)this.Infos.Size.Width, (int)this.Infos.Size.Height];
            Reset();

            foreach (SWorldComponent component in this._components)
            {
                component.SetWorldInstance(this);
                component.Initialize(this.Game);
            }

            this.States.IsActive = true;
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // Check current game status
            if (!this.States.IsActive || this.States.IsPaused)
            {
                return;
            }

            // Check update delay for the current game world
            this.updateTimer.Update();
            if (this.updateTimer.IsFinished)
            {
                this.updateTimer.Restart();
            }
            else
            {
                return;
            }

            // Update world
            foreach (SWorldComponent component in this._components)
            {
                component.Update(gameTime);
            }
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.OnDraw(gameTime, spriteBatch);

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
            this.updateTimer.Restart();
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
    }
}