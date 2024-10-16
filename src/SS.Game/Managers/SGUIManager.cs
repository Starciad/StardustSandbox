using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.GUI;
using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Managers
{
    public sealed class SGUIManager : SGameObject
    {
        public SGUIEvents GUIEvents => this._guiEvents;

        private readonly SGUIEvents _guiEvents;
        private readonly SGUIDatabase _guiDatabase;

        public SGUIManager(SGame gameInstance, SGUIDatabase guiDatabase, SInputManager inputManager) : base(gameInstance)
        {
            this._guiDatabase = guiDatabase;
            this._guiEvents = new(inputManager);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (SGUISystem guiSystem in this._guiDatabase.RegisteredGUIs)
            {
                if (guiSystem.IsActive)
                {
                    guiSystem.Update(gameTime);
                }
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (SGUISystem guiSystem in this._guiDatabase.RegisteredGUIs)
            {
                if (guiSystem.IsActive || guiSystem.IsShowing)
                {
                    guiSystem.Draw(gameTime, spriteBatch);
                }
            }
        }

        public void ShowGUI(string id)
        {
            if (TryGetGUIByName(id, out SGUISystem guiSystem))
            {
                guiSystem.Show();
            }
        }

        public void CloseGUI(string id)
        {
            if (TryGetGUIByName(id, out SGUISystem guiSystem))
            {
                guiSystem.Close();
            }
        }

        public SGUISystem GetGUIByName(string name)
        {
            _ = TryGetGUIByName(name, out SGUISystem guiSystem);
            return guiSystem;
        }

        public bool TryGetGUIByName(string name, out SGUISystem guiSystem)
        {
            SGUISystem target = this._guiDatabase.Find(name);
            guiSystem = target;

            return target != null;
        }
    }
}
