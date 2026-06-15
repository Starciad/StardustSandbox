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
using StardustSandbox.Core.UI.Elements.Common;
using StardustSandbox.Core.UI.Handlers;

using System;

namespace StardustSandbox.Core.UI.Builders
{
    internal sealed class UIBuildScope : IDisposable
    {
        private readonly UIElementHandler elementHandler;
        private readonly Container rootContainer;

        internal UIBuildScope(UIElementHandler elementHandler, GameScreen gameScreen)
        {
            this.elementHandler = elementHandler;

            // Create a root container for this scope.
            // All elements created within this scope will be added to this container.

            this.rootContainer = elementHandler.AddElement<Container>();
            this.rootContainer.Size = gameScreen.Viewport;
        }

        private void AddToRootContainer(UIElement element)
        {
            if (!element.HasParent)
            {
                this.rootContainer.AddChild(element);
            }
        }

        private T AddElement<T>() where T : UIElement, new()
        {
            T element = this.elementHandler.AddElement<T>();
            AddToRootContainer(element);
            return element;
        }

        internal Container AddContainer()
        {
            return AddElement<Container>();
        }

        internal Image AddImage()
        {
            return AddElement<Image>();
        }

        internal Label AddLabel()
        {
            return AddElement<Label>();
        }

        internal SliceImage AddSliceImage()
        {
            return AddElement<SliceImage>();
        }

        internal Text AddText()
        {
            return AddElement<Text>();
        }

        // This method is called when the scope is disposed, which typically happens at the end of a using block.
        // It resets the root container, which in turn should reset all child elements, effectively cleaning up the UI elements created within this scope.
        public void Dispose()
        {
            this.rootContainer.Initialize();
        }
    }
}
