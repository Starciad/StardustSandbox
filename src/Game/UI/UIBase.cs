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

using StardustSandbox.Constants;
using StardustSandbox.UI.Elements;

using System;

namespace StardustSandbox.UI
{
    internal abstract class UIBase
    {
        internal bool IsActive { get; private set; }
        internal bool IsInitialized { get; private set; }

        protected Container Root { get; }

        protected UIBase()
        {
            this.Root = new Container
            {
                CanDraw = false,
                CanUpdate = false,
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2()
            };
        }

        internal void Initialize()
        {
            if (this.IsInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} is already initialized.");
            }

            OnBuild(this.Root);
            this.Root.Initialize();

            this.IsInitialized = true;
        }

        internal void Open()
        {
            EnsureInitialized();

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

        protected abstract void OnBuild(Container root);
        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }
        protected virtual void OnUpdate(GameTime gameTime) { }

        private void EnsureInitialized()
        {
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} must be initialized before being opened."
                );
            }
        }
    }
}

