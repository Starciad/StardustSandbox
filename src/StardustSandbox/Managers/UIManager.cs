using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.Enums.UI;
using StardustSandbox.Interfaces;
using StardustSandbox.UI;

using System.Collections.Generic;

namespace StardustSandbox.Managers
{
    internal sealed class UIManager : IResettable
    {
        private UIBase currentUI;

        private readonly Stack<UIBase> uiStack = [];

        public void Reset()
        {
            while (this.uiStack.Count > 0)
            {
                UIBase gui = this.uiStack.Pop();
                gui.Close();
            }

            this.currentUI = null;
        }

        internal void Update(GameTime gameTime)
        {
            if (this.currentUI != null && this.currentUI.IsActive)
            {
                this.currentUI.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (this.currentUI != null && this.currentUI.IsActive)
            {
                this.currentUI.Draw(spriteBatch);
            }
        }

        internal void OpenGUI(UIIndex index)
        {
            UIBase ui = UIDatabase.GetUI(index);

            if (ui == null)
            {
                return;
            }

            this.currentUI?.Close();

            this.uiStack.Push(ui);
            this.currentUI = ui;

            ui.Open();
        }

        internal void CloseGUI()
        {
            if (this.currentUI == null)
            {
                return;
            }

            this.currentUI.Close();
            _ = this.uiStack.Pop();

            this.currentUI = this.uiStack.Count > 0 ? this.uiStack.Peek() : null;

            this.currentUI?.Open();
        }
    }
}
