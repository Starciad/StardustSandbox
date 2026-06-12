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

using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Elements.Simples;
using StardustSandbox.Core.UI.Handlers;

using System;

namespace StardustSandbox.Core.UI.Builders
{
    internal sealed class UIBuildScope : IDisposable
    {
        private readonly UIElementHandler elementHandler;
        private readonly Container rootContainer;

        internal UIBuildScope(UIElementHandler elementHandler)
        {
            this.elementHandler = elementHandler;
            this.rootContainer = CreateElement<Container>();
        }
        
        private void AddToRootContainer(UIElement element)
        {
            if (!element.HasParent)
            {
                this.rootContainer.AddChild(element);
            }
        }

        internal T CreateElement<T>() where T : UIElement, new()
        {
            T element = this.elementHandler.CreateElement<T>();
            AddToRootContainer(element);
            return element;
        }

        internal T CreateElement<T>(Action<T> configure) where T : UIElement, new()
        {
            T element = this.elementHandler.CreateElement<T>(configure);
            AddToRootContainer(element);
            return element;
        }

        // This method is called when the scope is disposed, which typically happens at the end of a using block.
        // It resets the root container, which in turn should reset all child elements, effectively cleaning up the UI elements created within this scope.
        public void Dispose()
        {
            this.rootContainer.Initialize();
        }
    }
}
