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

using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.UI.Builders;
using StardustSandbox.Core.UI.Handlers;

namespace StardustSandbox.Core.UI
{
    internal abstract class UIBase<TDependencies, TModel>(TDependencies dependencies, UIElementHandler elementHandler) : IUI
        where TDependencies : IUIDependencies
        where TModel : IUIModel
    {
        internal bool IsActive { get; private set; }

        protected TDependencies Dependencies => dependencies;

        internal void Open(TModel model)
        {
            if (this.IsActive)
            {
                return;
            }

            OnBuild(new(elementHandler), model);
            OnOpened();

            this.IsActive = true;
        }

        internal void Close()
        {
            if (!this.IsActive)
            {
                return;
            }

            elementHandler.ReleaseAllElements();
            OnClosed();

            this.IsActive = false;
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            elementHandler.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.IsActive)
            {
                return;
            }

            elementHandler.Draw(spriteBatch);
        }

        protected abstract void OnBuild(UIBuildContext context, TModel model);
        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }
    }
}

