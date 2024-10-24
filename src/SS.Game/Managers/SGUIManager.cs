using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.GUISystem;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Managers
{
    public sealed class SGUIManager(SGame gameInstance) : SGameObject(gameInstance)
    {
        public SGUIEvents GUIEvents => this._guiEvents;

        private readonly SGUIEvents _guiEvents = new(gameInstance.InputManager);
        private readonly SGUIDatabase _guiDatabase = gameInstance.GUIDatabase;

        public override void Update(GameTime gameTime)
        {
            foreach (SGUISystem guiSystem in this._guiDatabase.RegisteredGUIs)
            {
                if (guiSystem.IsActive)
                {
                    guiSystem.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
