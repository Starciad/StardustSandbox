using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Elements;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Managers;
using PixelDust.Game.Objects;
using PixelDust.Game.Utilities;
using PixelDust.Game.Worlding;
using PixelDust.Game.Worlding.Components;
using PixelDust.Game.Worlding.Components.Chunking;
using PixelDust.Game.Worlding.Components.Threading;
using PixelDust.Game.Worlding.World.Data;
using PixelDust.Game.Worlding.World.Slots;

using System;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld(PElementDatabase elementDatabase, Texture2D particleTexture) : PGameObject
    {
        public PWorldStates States { get; private set; } = new();
        public PWorldInfos Infos { get; private set; } = new();
        public PWorldElementSlot[,] Elements { get; private set; }
        public PElementDatabase ElementDatabase => elementDatabase;

        private readonly PTimer updateTimer = new(0.35f);

        private readonly PWorldComponent[] _components =
        [
            new PWorldChunkingComponent(),
            new PWorldThreadingComponent(),
        ];

        protected override void OnAwake()
        {
            base.OnAwake();

            Restart();

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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, PWorldCamera.Camera.GetViewMatrix());
            DrawSlots(gameTime, spriteBatch);
            foreach (PWorldComponent component in this._components)
            {
                component.Draw(gameTime,spriteBatch);
            }
            spriteBatch.End();
        }
        private void DrawSlots(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int x = 0; x < this.Infos.Size.Width; x++)
            {
                for (int y = 0; y < this.Infos.Size.Height; y++)
                {
                    if (IsEmptyElementSlot(new(x, y)))
                    {
                        spriteBatch.Draw(particleTexture, new Vector2(x, y) * PWorldConstants.GRID_SCALE, null, Color.Black, 0f, Vector2.Zero, PWorldConstants.GRID_SCALE, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        PElement element = elementDatabase.GetElementById(this.Elements[x, y].Id);

                        element.Context = new PElementContext(this, this.Elements[x, y], new(x, y));
                        element.Draw(gameTime, spriteBatch);
                    }
                }
            }
        }

        public T GetComponent<T>() where T : PWorldComponent
        {
            return (T)Array.Find(this._components, x => x.GetType() == typeof(T));
        }
        public void Restart()
        {
            this.Elements = new PWorldElementSlot[this.Infos.Size.Width, this.Infos.Size.Height];
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