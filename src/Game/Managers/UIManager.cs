/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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
        // Stack represents navigation/history. Top = currently active UI.
        private readonly Stack<UIBase> uiStack = new();

        // Expose current UI for callers (read-only).
        internal UIBase CurrentUI => this.uiStack.Count > 0 ? this.uiStack.Peek() : null;

        public void Reset()
        {
            while (this.uiStack.Count > 0)
            {
                UIBase ui = this.uiStack.Pop();
                ui.Close();
            }

            // Ensure no lingering reference.
            // CurrentUI property will reflect empty stack.
        }

        internal void Update(GameTime gameTime)
        {
            UIBase current = this.CurrentUI;
            if (current != null && current.IsActive)
            {
                current.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            UIBase current = this.CurrentUI;
            if (current != null && current.IsActive)
            {
                current.Draw(spriteBatch);
            }
        }

        internal void OpenUI(UIIndex index)
        {
            UIBase ui = UIDatabase.GetUI(index);
            if (ui == null)
            {
                return;
            }

            // If the requested UI is already the active one and active, nothing to do.
            if (this.uiStack.Count > 0 && this.uiStack.Peek() == ui)
            {
                if (ui.IsActive)
                {
                    return;
                }

                // If it's top but currently not active, (re)open it.
                ui.Open();
                return;
            }

            // If the requested UI exists somewhere in the stack (history),
            // pop and close everything above it, then reopen it as current.
            if (this.uiStack.Contains(ui))
            {
                // Close and remove entries above the requested UI.
                while (this.uiStack.Count > 0 && this.uiStack.Peek() != ui)
                {
                    UIBase top = this.uiStack.Pop();
                    top.Close();
                }

                // Now top == ui
                UIBase current = this.uiStack.Peek();
                current.Open(); // ensure it's active
                return;
            }

            // New UI: close current top, push new UI and open it.
            if (this.uiStack.Count > 0)
            {
                UIBase top = this.uiStack.Peek();
                top.Close();
            }

            this.uiStack.Push(ui);
            ui.Open();
        }

        internal void CloseUI()
        {
            if (this.uiStack.Count == 0)
            {
                return;
            }

            // Close and remove current UI.
            UIBase top = this.uiStack.Pop();
            top.Close();

            // If there's a previous UI, make it current and reopen it.
            if (this.uiStack.Count > 0)
            {
                UIBase previous = this.uiStack.Peek();
                previous.Open();
            }
        }
    }
}

