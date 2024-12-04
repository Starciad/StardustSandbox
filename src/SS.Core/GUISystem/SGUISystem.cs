using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.GUISystem
{
    public abstract class SGUISystem : SGameObject
    {
        public string Identifier => this.identifier;
        public int ZIndex => this.zIndex;
        public bool IsActive => this.isActive;
        public bool IsShowing => this.isShowing;
        public bool HasEvents => this.GUIEvents != null;
        protected SGUIEvents GUIEvents => this.guiEvents;

        protected int zIndex;

        private bool isActive;
        private bool isShowing;

        private readonly string identifier;
        private readonly SGUILayout layout;
        private readonly SGUIEvents guiEvents;

        public SGUISystem(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance)
        {
            this.identifier = identifier;
            this.guiEvents = guiEvents;
            this.layout = new(this.SGameInstance);
        }

        public override void Initialize()
        {
            OnBuild(this.layout);
            this.layout.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.layout.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.layout.Draw(gameTime, spriteBatch);
        }

        public void Show()
        {
            this.isActive = true;
            this.isShowing = true;

            OnLoad();
        }

        public void Close()
        {
            this.isActive = false;
            this.isShowing = false;

            OnUnload();
        }

        protected abstract void OnBuild(ISGUILayoutBuilder layout);
        protected virtual void OnLoad() { return; }
        protected virtual void OnUnload() { return; }
    }
}