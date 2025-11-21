using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Interfaces;
using StardustSandbox.UISystem;

using System.Collections.Generic;

namespace StardustSandbox.Managers
{
    internal sealed class UIManager : IResettable
    {
        private UI currentUI;

        private readonly Stack<UI> uiStack = [];

        public void Reset()
        {
            while (this.uiStack.Count > 0)
            {
                UI gui = this.uiStack.Pop();
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
            if (TryGetGUIById(index, out UI guiSystem))
            {
                this.currentUI?.Close();

                this.uiStack.Push(guiSystem);
                this.currentUI = guiSystem;

                guiSystem.Open();
            }
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

        internal UI GetGUIById(UIIndex index)
        {
            _ = TryGetGUIById(index, out UI guiSystem);
            return guiSystem;
        }

        internal bool TryGetGUIById(UIIndex index, out UI guiSystem)
        {
            UI target = UIDatabase.GetUIByIndex(index);
            guiSystem = target;

            return target != null;
        }
    }
}
