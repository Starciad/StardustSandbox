using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Objects;

namespace PixelDust.Game.GUI
{
    public abstract class PGUISystem : PGameObject
    {
        public int ZIndex { get; protected set; }
        public bool IsActive => this.isActive;
        public bool IsShowing => this.isShowing;

        private readonly PGUILayout layout;
        private bool isActive;
        private bool isShowing;

        public PGUISystem()
        {
            Activate();
            Close();

            this.layout = new();
        }

        protected override void OnAwake()
        {
            this.layout.Initialize(this.Game);
            OnBuild(this.layout);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (!this.isActive)
            {
                return;
            }

            this.layout.Update(gameTime);
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.isActive || !this.isShowing)
            {
                return;
            }

            this.layout.Draw(gameTime, spriteBatch);
        }

        public void Activate()
        {
            this.isActive = true;
            OnActivated();
        }

        public void Disable()
        {
            this.isActive = false;
            OnDisabled();
        }

        public void Show()
        {
            this.isShowing = true;
            OnShowed();
        }

        public void Close()
        {
            this.isShowing = false;
            OnClosed();
        }

        protected abstract void OnBuild(IPGUILayoutBuilder layout);
        protected virtual void OnActivated() { return; }
        protected virtual void OnDisabled() { return; }
        protected virtual void OnShowed() { return; }
        protected virtual void OnClosed() { return; }
    }
}
