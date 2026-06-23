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

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Elements.Common;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.UI.Handlers
{
    internal sealed class UIElementHandler
    {
        private int activeElementCount = 0;

        private readonly List<UIElement> activeElements = [];
        private readonly Dictionary<Type, ObjectPool> elementPools = new()
        {
            // Standard
            [typeof(Container)] = new(),
            [typeof(Image)] = new(),
            [typeof(Label)] = new(),
            [typeof(SliceImage)] = new(),
            [typeof(Text)] = new(),

            // Specials
            [typeof(NotificationBox)] = new(),
            [typeof(TooltipBox)] = new()
        };

        internal UIElementHandler()
        {

        }

        internal T AddElement<T>() where T : UIElement, new()
        {
            if (!this.elementPools.TryGetValue(typeof(T), out ObjectPool pool))
            {
                throw new InvalidOperationException($"No pool found for element type {typeof(T).FullName}.");
            }

            T value = pool.TryDequeue(out IPoolableObject poolableObject) ? (T)poolableObject : new();

            this.activeElements.Add(value);
            this.activeElementCount++;

            return value;
        }

        internal T AddElement<T>(Action<T> configure) where T : UIElement, new()
        {
            T element = AddElement<T>();
            configure?.Invoke(element);
            return element;
        }

        internal void ReleaseAllElements()
        {
            foreach (UIElement element in this.activeElements)
            {
                if (this.elementPools.TryGetValue(element.GetType(), out ObjectPool pool))
                {
                    pool.Enqueue(element);
                }
            }

            this.activeElements.Clear();
            this.activeElementCount = 0;
        }

        internal void Update(GameTime gameTime)
        {
            if (this.activeElementCount == 0)
            {
                return;
            }

            this.activeElements[0].Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (this.activeElementCount == 0)
            {
                return;
            }

            this.activeElements[0].Draw(spriteBatch);
        }
    }
}
