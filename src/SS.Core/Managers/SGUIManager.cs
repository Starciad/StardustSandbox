using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.Managers;

using System;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SGUIManager(ISGame gameInstance) : SManager(gameInstance), ISGUIManager
    {
        public SGUIEvents GUIEvents => this._guiEvents;

        private readonly SGUIEvents _guiEvents = new(gameInstance.InputManager);

        private readonly ISGUIDatabase _guiDatabase = gameInstance.GUIDatabase;

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
                if (guiSystem.IsActive || guiSystem.IsOpened)
                {
                    guiSystem.Draw(gameTime, spriteBatch);
                }
            }
        }

        public void OpenGUI(string identifier)
        {
            if (TryGetGUIById(identifier, out SGUISystem guiSystem))
            {
                guiSystem.Open();
            }
        }

        public void CloseGUI(string identifier)
        {
            if (TryGetGUIById(identifier, out SGUISystem guiSystem))
            {
                guiSystem.Close();
            }
        }

        public SGUISystem GetGUIById(string identifier)
        {
            _ = TryGetGUIById(identifier, out SGUISystem guiSystem);
            return guiSystem;
        }

        public bool TryGetGUIById(string identifier, out SGUISystem guiSystem)
        {
            SGUISystem target = this._guiDatabase.GetGUISystemById(identifier);
            guiSystem = target;

            return target != null;
        }

        public void Reset()
        {
            return;
        }
    }
}
