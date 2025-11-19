using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.UISystem
{
    internal abstract class UI(UIIndex index)
    {
        internal UIIndex Index => this.index;
        internal bool IsActive => this.isActive;
        internal bool IsOpened => this.isOpened;

        private bool isActive;
        private bool isOpened;

        private readonly UIIndex index = index;
        private readonly Layout layout = new();

        internal virtual void Initialize()
        {
            OnBuild(this.layout);
            this.layout.Initialize();
        }

        internal virtual void Update(GameTime gameTime)
        {
            this.layout.Update(gameTime);
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            this.layout.Draw(spriteBatch);
        }

        internal void Open()
        {
            this.isActive = true;
            this.isOpened = true;

            OnOpened();
        }

        internal void Close()
        {
            this.isActive = false;
            this.isOpened = false;

            OnClosed();
        }

        protected abstract void OnBuild(Layout layout);
        protected virtual void OnOpened() { return; }
        protected virtual void OnClosed() { return; }
    }
}