using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.Managers;

using System.Collections.Generic;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SGUIManager(ISGame gameInstance) : SManager(gameInstance), ISGUIManager
    {
        public SGUIEvents GUIEvents => this.guiEvents;
        public SGUISystem CurrentGUI => this.currentGUI;

        private SGUISystem currentGUI;

        private readonly Stack<SGUISystem> guiStack = [];
        private readonly SGUIEvents guiEvents = new(gameInstance.InputManager);
        private readonly ISGUIDatabase guiDatabase = gameInstance.GUIDatabase;

        public override void Update(GameTime gameTime)
        {
            if (this.currentGUI != null && this.currentGUI.IsActive)
            {
                this.currentGUI.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.currentGUI != null && this.currentGUI.IsActive)
            {
                this.currentGUI.Draw(gameTime, spriteBatch);
            }
        }

        public void OpenGUI(string identifier)
        {
            if (TryGetGUIById(identifier, out SGUISystem guiSystem))
            {
                this.currentGUI?.Close();

                this.guiStack.Push(guiSystem);
                this.currentGUI = guiSystem;

                guiSystem.Open();
            }
        }

        public void CloseGUI()
        {
            if (this.currentGUI == null)
            {
                return;
            }

            this.currentGUI.Close();
            _ = this.guiStack.Pop();

            this.currentGUI = this.guiStack.Count > 0 ? this.guiStack.Peek() : null;

            this.currentGUI?.Open();
        }

        public SGUISystem GetGUIById(string identifier)
        {
            _ = TryGetGUIById(identifier, out SGUISystem guiSystem);
            return guiSystem;
        }

        public bool TryGetGUIById(string identifier, out SGUISystem guiSystem)
        {
            SGUISystem target = this.guiDatabase.GetGUISystemById(identifier);
            guiSystem = target;

            return target != null;
        }

        public void Reset()
        {
            while (this.guiStack.Count > 0)
            {
                SGUISystem gui = this.guiStack.Pop();
                gui.Close();
            }

            this.currentGUI = null;
        }
    }
}
