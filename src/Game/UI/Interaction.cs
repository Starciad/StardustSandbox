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
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Enums.Inputs;
using StardustSandbox.InputSystem;
using StardustSandbox.UI.Elements;

using System;

namespace StardustSandbox.UI
{
    internal static class Interaction
    {
        /// <summary>
        /// Checks if the specified mouse button was clicked within the given area.
        /// </summary>
        internal static bool OnMouseClick(in MouseButton button, in Vector2 targetPosition, in Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();

            return GetButtonState(button, Input.MouseState) == ButtonState.Released &&
                   GetButtonState(button, Input.PreviousMouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the specified mouse button was clicked within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseClick(in MouseButton button, in UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseClick(button, element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the specified mouse button is pressed within the given area.
        /// </summary>
        internal static bool OnMouseDown(in MouseButton button, in Vector2 targetPosition, in Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();

            return GetButtonState(button, Input.MouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the specified mouse button is pressed within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseDown(in MouseButton button, in UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseDown(button, element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the specified mouse button is released within the given area.
        /// </summary>
        internal static bool OnMouseUp(in MouseButton button, in Vector2 targetPosition, in Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();

            return GetButtonState(button, Input.MouseState) == ButtonState.Released &&
                   GetButtonState(button, Input.PreviousMouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the specified mouse button is released within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseUp(in MouseButton button, in UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseUp(button, element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the mouse cursor enters the specified area.
        /// </summary>
        internal static bool OnMouseEnter(in Vector2 targetPosition, in Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();
            Vector2 previousMousePosition = Input.GetScaledPreviousMousePosition();

            bool mouseWasOutside = !IsMouseWithinArea(previousMousePosition, targetPosition, areaSize);
            bool mouseIsInside = IsMouseWithinArea(mousePosition, targetPosition, areaSize);

            return mouseWasOutside && mouseIsInside;
        }

        /// <summary>
        /// Checks if the mouse cursor enters the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseEnter(in UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseEnter(element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the mouse cursor is over the specified area.
        /// </summary>
        internal static bool OnMouseOver(in Vector2 targetPosition, in Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();

            return IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor is over the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseOver(in UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseOver(element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the mouse cursor leaves the specified area.
        /// </summary>
        internal static bool OnMouseLeave(in Vector2 targetPosition, in Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();
            Vector2 previousMousePosition = Input.GetScaledPreviousMousePosition();

            bool mouseWasInside = IsMouseWithinArea(previousMousePosition, targetPosition, areaSize);
            bool mouseIsOutside = !IsMouseWithinArea(mousePosition, targetPosition, areaSize);

            return mouseWasInside && mouseIsOutside;
        }

        /// <summary>
        /// Checks if the mouse cursor leaves the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseLeave(in UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseLeave(element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the left mouse button was clicked within the specified area.
        /// </summary>
        internal static bool OnMouseLeftClick(in Vector2 targetPosition, in Vector2 areaSize)
        {
            return OnMouseClick(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button was clicked within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseLeftClick(in UIElement element)
        {
            return OnMouseClick(MouseButton.Left, element);
        }

        /// <summary>
        /// Checks if the left mouse button is pressed within the specified area.
        /// </summary>
        internal static bool OnMouseLeftDown(in Vector2 targetPosition, in Vector2 areaSize)
        {
            return OnMouseDown(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is pressed within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseLeftDown(in UIElement element)
        {
            return OnMouseDown(MouseButton.Left, element);
        }

        /// <summary>
        /// Checks if the left mouse button is released within the specified area.
        /// </summary>
        internal static bool OnMouseLeftUp(in Vector2 targetPosition, in Vector2 areaSize)
        {
            return OnMouseUp(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is released within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseLeftUp(in UIElement element)
        {
            return OnMouseUp(MouseButton.Left, element);
        }

        /// <summary>
        /// Checks if the right mouse button was clicked within the specified area.
        /// </summary>
        internal static bool OnMouseRightClick(in Vector2 targetPosition, in Vector2 areaSize)
        {
            return OnMouseClick(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button was clicked within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseRightClick(in UIElement element)
        {
            return OnMouseClick(MouseButton.Right, element);
        }

        /// <summary>
        /// Checks if the right mouse button is pressed within the specified area.
        /// </summary>
        internal static bool OnMouseRightDown(in Vector2 targetPosition, in Vector2 areaSize)
        {
            return OnMouseDown(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button is pressed within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseRightDown(in UIElement element)
        {
            return OnMouseDown(MouseButton.Right, element);
        }

        /// <summary>
        /// Checks if the right mouse button is released within the specified area.
        /// </summary>
        internal static bool OnMouseRightUp(in Vector2 targetPosition, in Vector2 areaSize)
        {
            return OnMouseUp(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button is released within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseRightUp(in UIElement element)
        {
            return OnMouseUp(MouseButton.Right, element);
        }

        internal static bool OnMouseScrollUp()
        {
            return Input.GetDeltaScrollWheel() > 0;
        }

        internal static bool OnMouseScrollDown()
        {
            return Input.GetDeltaScrollWheel() < 0;
        }

        /// <summary>
        /// Determines if the mouse position is within the specified area.
        /// </summary>
        private static bool IsMouseWithinArea(in Vector2 mousePosition, in Vector2 targetPosition, in Vector2 areaSize)
        {
            float left = targetPosition.X;
            float right = targetPosition.X + areaSize.X;
            float top = targetPosition.Y;
            float bottom = targetPosition.Y + areaSize.Y;

            bool withinHorizontalBounds = mousePosition.X >= left && mousePosition.X <= right;
            bool withinVerticalBounds = mousePosition.Y >= top && mousePosition.Y <= bottom;

            return withinHorizontalBounds && withinVerticalBounds;
        }

        /// <summary>
        /// Gets the state of the specified mouse button from the given MouseState.
        /// </summary>
        private static ButtonState GetButtonState(in MouseButton button, in MouseState state)
        {
            return button switch
            {
                MouseButton.Left => state.LeftButton,
                MouseButton.Right => state.RightButton,
                MouseButton.Middle => state.MiddleButton,
                _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
            };
        }
    }
}

