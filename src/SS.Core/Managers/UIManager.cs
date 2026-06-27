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

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.UI.Handlers;

using System.Collections.Generic;

namespace StardustSandbox.Core.Managers
{
    internal sealed class UIManager : IResettable
    {
        internal bool HasActiveUI => this.uiStack.Count > 0 && this.uiStack.Peek().IsActive;

        // Expose current UI for callers (read-only).
        internal IUI CurrentUI => this.uiStack.Count > 0 ? this.uiStack.Peek() : null;

        // Stack represents navigation/history. Top = currently active UI.
        private readonly Stack<IUI> uiStack = new();

        private readonly UIDatabase uiDatabase;
        private readonly UIElementHandler elementHandler;

        internal UIManager(UIElementHandler elementHandler, UIDatabase uiDatabase)
        {
            this.elementHandler = elementHandler;
            this.uiDatabase = uiDatabase;
        }

        public void Reset()
        {
            while (this.uiStack.Count > 0)
            {
                IUI ui = this.uiStack.Pop();
                ui.Close();
            }

            // Ensure no lingering reference.
            // CurrentUI property will reflect empty stack.
        }

        private bool TryGetActiveUI(out IUI ui)
        {
            IUI current = this.CurrentUI;

            if (current != null && current.IsActive)
            {
                ui = current;
                return true;
            }

            ui = null;
            return false;
        }

        internal void Update(GameTime gameTime)
        {
            if (TryGetActiveUI(out IUI _))
            {
                this.elementHandler.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (TryGetActiveUI(out IUI _))
            {
                this.elementHandler.Draw(spriteBatch);
            }
        }

        internal void OpenUI<TGui, TModel>(TModel model)
            where TGui : IUI
            where TModel : IUIModel
        {
            IUI ui = this.uiDatabase.GetUI<TGui>();

            // If the requested UI is already the active one and active, nothing to do.
            if (this.uiStack.Count > 0 && this.uiStack.Peek() == ui)
            {
                if (ui.IsActive)
                {
                    return;
                }

                // If it's top but currently not active, (re)open it.
                ui.Open(model);
                return;
            }

            // New UI: hide current top, push new UI and open it.
            if (this.uiStack.Count > 0)
            {
                IUI top = this.uiStack.Peek();
                top.Hide();
            }

            this.uiStack.Push(ui);
            ui.Open(model);
        }

        internal void OpenUI<TGui, TModel>()
            where TGui : IUI
            where TModel : IUIModel, new()
        {
            OpenUI<TGui, TModel>(new());
        }

        internal void CloseUI()
        {
            if (this.uiStack.Count == 0)
            {
                return;
            }

            // Close and remove current UI.
            IUI top = this.uiStack.Pop();
            top.Close();

            // If there's a previous UI, make it current and reopen it.
            if (this.uiStack.Count > 0)
            {
                IUI previous = this.uiStack.Peek();
                previous.Show();
            }
        }

        internal void RefreshUI()
        {
            if (TryGetActiveUI(out IUI ui))
            {
                ui.Refresh();
            }
        }
    }
}
