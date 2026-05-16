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

using StardustSandbox.Core.UI.Elements;

namespace StardustSandbox.Core.UI
{
    internal abstract class UIBase
    {
        internal bool IsActive { get; private set; }

        protected GameScreen GameScreen { get; }
        protected Container Root { get; }

        protected UIBase(GameScreen gameScreen)
        {
            this.GameScreen = gameScreen;
            this.Root = new()
            {
                CanDraw = false,
                CanUpdate = false,
                Size = gameScreen.GetViewport()
            };
        }

        internal void Initialize()
        {
            OnBuild(this.Root);
            this.Root.Initialize();
        }

        internal void Open()
        {
            if (this.IsActive)
            {
                return;
            }

            this.IsActive = true;

            this.Root.CanUpdate = true;
            this.Root.CanDraw = true;

            OnOpened();
        }

        internal void Close()
        {
            if (!this.IsActive)
            {
                return;
            }

            this.IsActive = false;

            this.Root.CanUpdate = false;
            this.Root.CanDraw = false;

            OnClosed();
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            this.Root.Update(gameTime);

            OnUpdate(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.IsActive)
            {
                return;
            }

            this.Root.Draw(spriteBatch);
        }

        internal void Resize()
        {
            this.Root.Size = this.GameScreen.GetViewport();
            OnScreenResize();
        }

        protected abstract void OnBuild(Container root);
        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnScreenResize() { }
    }
}

