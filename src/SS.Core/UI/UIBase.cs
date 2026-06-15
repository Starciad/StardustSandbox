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

using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.UI.Builders;
using StardustSandbox.Core.UI.Handlers;

namespace StardustSandbox.Core.UI
{
    internal abstract class UIBase<TDependencies, TModel>(TDependencies dependencies, UIElementHandler elementHandler) : IUI
        where TDependencies : IUIDependencies
        where TModel : IUIModel
    {
        public bool IsActive => this.isActive;
        protected TDependencies Dependencies => dependencies;

        private bool isActive = false;
        private TModel model;

        private void Instantiate(TModel model)
        {
            OnBuild(new(elementHandler), model);
        }

        private void Destroy()
        {
            elementHandler.ReleaseAllElements();
        }

        public void Open(TModel model)
        {
            if (this.isActive)
            {
                return;
            }

            Destroy();
            Instantiate(model);
            OnOpened();

            this.isActive = true;
            this.model = model;
        }

        public void Open(IUIModel model)
        {
            Open(model);
        }

        public void Refresh()
        {
            if (!this.isActive)
            {
                return;
            }

            // Rebuild the UI with the same model.
            // This allows for dynamic updates without needing to close and reopen.

            Destroy();
            Instantiate(this.model);
        }

        public void Reopen()
        {
            Refresh();
            OnOpened();
            this.isActive = true;
        }

        public void Close()
        {
            if (!this.isActive)
            {
                return;
            }

            Destroy();
            OnClosed();

            this.isActive = false;
        }

        protected abstract void OnBuild(UIBuildContext context, TModel model);
        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }
    }
}

