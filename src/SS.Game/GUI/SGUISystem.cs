﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.Interfaces.GUI;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.GUI
{
    public abstract class SGUISystem : SGameObject
    {
        public string Name { get; protected set; }
        public int ZIndex { get; protected set; }
        public bool IsActive => this.isActive;
        public bool IsShowing => this.isShowing;
        public bool HasEvents => this.GUIEvents != null;
        protected SGUIEvents GUIEvents { get; private set; }

        private SGUILayout layout;

        private bool isActive;
        private bool isShowing;

        internal void Configure(SGUIEvents guiEvents, SGUILayoutPool layoutPool)
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

            OnLoad();
        }

        public void Close()
        {
            this.isActive = false;
            this.isShowing = false;

            this.layout.Unload();

            OnUnload();
        }

        protected abstract void OnBuild(ISGUILayoutBuilder layout);
        protected virtual void OnLoad() { return; }
        protected virtual void OnUnload() { return; }
    }
}