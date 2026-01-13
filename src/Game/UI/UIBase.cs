using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.UI;
using StardustSandbox.UI.Elements;

using System;

namespace StardustSandbox.UI
{
    internal abstract class UIBase
    {
        internal UIIndex Index { get; }
        internal bool IsActive { get; private set; }
        internal bool IsInitialized { get; private set; }

        protected Container Root { get; }

        protected UIBase(UIIndex index)
        {
            this.Index = index;

            this.Root = new Container
            {
                CanDraw = false,
                CanUpdate = false,
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2()
            };
        }

        internal void Initialize()
        {
            if (this.IsInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} is already initialized.");
            }

            OnBuild(this.Root);
            this.Root.Initialize();

            this.IsInitialized = true;
        }

        internal void Open()
        {
            EnsureInitialized();

            if (this.IsActive)
            {
                return;
            }

            this.IsActive = true;

            this.Root.CanUpdate = true;
            this.Root.CanDraw = true;

            OnOpened();
        }

        internal void Close()
        {
            if (!this.IsActive)
            {
                return;
            }

            this.IsActive = false;

            this.Root.CanUpdate = false;
            this.Root.CanDraw = false;

            OnClosed();
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            this.Root.Update(gameTime);
            OnUpdate(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.IsActive)
            {
                return;
            }

            this.Root.Draw(spriteBatch);
            OnDraw(spriteBatch);
        }

        protected abstract void OnBuild(Container root);
        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(SpriteBatch spriteBatch) { }

        private void EnsureInitialized()
        {
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} must be initialized before being opened."
                );
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} ({this.Index})";
        }
    }
}
