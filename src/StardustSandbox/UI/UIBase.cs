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
        internal UIIndex Index => this.index;
        internal bool IsActive => this.isActive;

        private bool isActive;
        private bool isInitialized;

        private readonly Container root;
        private readonly UIIndex index;

        internal UIBase(UIIndex index)
        {
            this.index = index;

            this.root = new()
            {
                CanDraw = true,
                CanUpdate = true,
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2()
            };
        }

        internal virtual void Initialize()
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} has already been initialized.");
            }

            OnBuild(this.root);
            this.root.Initialize();

            this.isInitialized = true;
        }

        internal virtual void Update(GameTime gameTime)
        {
            this.root.Update(gameTime);
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            this.root.Draw(spriteBatch);
        }

        internal void Open()
        {
            this.isActive = true;
            OnOpened();
        }

        internal void Close()
        {
            this.isActive = false;
            OnClosed();
        }

        protected abstract void OnBuild(Container root);
        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }

        public override string ToString()
        {
            return this.root.ToString();
        }
    }
}
