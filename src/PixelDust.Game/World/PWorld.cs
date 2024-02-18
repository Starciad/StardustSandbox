using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Databases;
using PixelDust.Game.Interfaces;
using PixelDust.Game.Managers;
using PixelDust.Game.Objects;
using PixelDust.Game.Utilities;
using PixelDust.Game.World.Components;
using PixelDust.Game.World.Components.Chunking;
using PixelDust.Game.World.Components.Rendering;
using PixelDust.Game.World.Components.Threading;
using PixelDust.Game.World.Data;
using PixelDust.Game.World.Slots;

using System;

namespace PixelDust.Game.World
{
    public sealed partial class PWorld(PElementDatabase elementDatabase, PAssetDatabase assetDatabase, PCameraManager camera) : PGameObject, IReset
    {
        public PWorldStates States { get; private set; } = new();
        public PWorldInfos Infos { get; private set; } = new();
        public PWorldElementSlot[,] Elements { get; private set; }
        public PElementDatabase ElementDatabase => elementDatabase;

        private readonly PTimer updateTimer = new(0.35f);
        private readonly PWorldComponent[] _components =
        [
            new PWorldChunkingComponent(assetDatabase),
            new PWorldThreadingComponent(),
            new PWorldRenderingComponent(elementDatabase, camera)
        ];

        protected override void OnAwake()
        {
            base.OnAwake();

            this.Elements = new PWorldElementSlot[this.Infos.Size.Width, this.Infos.Size.Height];
            Reset();

            foreach (PWorldComponent component in this._components)
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
            foreach (PWorldComponent component in this._components)
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

            foreach (PWorldComponent component in this._components)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

        public T GetComponent<T>() where T : PWorldComponent
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
            if (this.Elements == null)
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