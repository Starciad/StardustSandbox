using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI.Events;
using PixelDust.Game.Interfaces.GUI;
using PixelDust.Game.Objects;

namespace PixelDust.Game.GUI
{
    public abstract class PGUISystem : PGameObject
    {
        public string Name { get; protected set; }
        public int ZIndex { get; protected set; }
        public bool IsActive => this.isActive;
        public bool IsShowing => this.isShowing;
        public bool HasEvents => this.GUIEvents != null;
        protected PGUIEvents GUIEvents { get; private set; }

        private PGUILayout layout;

        private bool isActive;
        private bool isShowing;

        internal void Configure(PGUIEvents guiEvents, PGUILayoutPool layoutPool)
        {
            this.GUIEvents = guiEvents;
            this.layout = new(layoutPool);
        }

        protected override void OnAwake()
        {
            this.layout.Initialize(this.Game);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.layout.Update(gameTime);
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.layout.Draw(gameTime, spriteBatch);
        }

        public void Show()
        {
            this.layout.Load();
            OnBuild(this.layout);
            this.layout.Configure();

            this.isActive = true;
            this.isShowing = true;
        }

        public void Close()
        {
            this.isActive = false;
            this.isShowing = false;

            this.layout.Unload();
        }

        protected abstract void OnBuild(IPGUILayoutBuilder layout);
    }
}