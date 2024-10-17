using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.Interfaces.GUI;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.GUISystem
{
    public abstract class SGUISystem : SGameObject
    {
        public string Name { get; protected set; }
        public int ZIndex { get; protected set; }
        public bool IsActive => this.isActive;
        public bool IsShowing => this.isShowing;
        public bool HasEvents => this.GUIEvents != null;
        protected SGUIEvents GUIEvents { get; private set; }

        private readonly SGUILayout layout;

        private bool isActive;
        private bool isShowing;

        public SGUISystem(SGame gameInstance, SGUIEvents guiEvents) : base(gameInstance)
        {
            this.GUIEvents = guiEvents;
            this.layout = new(this.SGameInstance);
        }

        public override void Initialize()
        {
            this.layout.Initialize();
            OnBuild(this.layout);
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