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
        internal static bool OnMouseClick(MouseButton button, Vector2 targetPosition, Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();

            return GetButtonState(button, Input.MouseState) == ButtonState.Released &&
                   GetButtonState(button, Input.PreviousMouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the specified mouse button was clicked within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseClick(MouseButton button, UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseClick(button, element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the specified mouse button is pressed within the given area.
        /// </summary>
        internal static bool OnMouseDown(MouseButton button, Vector2 targetPosition, Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();

            return GetButtonState(button, Input.MouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the specified mouse button is pressed within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseDown(MouseButton button, UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseDown(button, element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the specified mouse button is released within the given area.
        /// </summary>
        internal static bool OnMouseUp(MouseButton button, Vector2 targetPosition, Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();

            return GetButtonState(button, Input.MouseState) == ButtonState.Released &&
                   GetButtonState(button, Input.PreviousMouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the specified mouse button is released within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseUp(MouseButton button, UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseUp(button, element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the mouse cursor enters the specified area.
        /// </summary>
        internal static bool OnMouseEnter(Vector2 targetPosition, Vector2 areaSize)
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
        internal static bool OnMouseEnter(UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseEnter(element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the mouse cursor is over the specified area.
        /// </summary>
        internal static bool OnMouseOver(Vector2 targetPosition, Vector2 areaSize)
        {
            Vector2 mousePosition = Input.GetScaledMousePosition();

            return IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor is over the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseOver(UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseOver(element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the mouse cursor leaves the specified area.
        /// </summary>
        internal static bool OnMouseLeave(Vector2 targetPosition, Vector2 areaSize)
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
        internal static bool OnMouseLeave(UIElement element)
        {
            return element is null ? throw new ArgumentNullException(nameof(element)) : OnMouseLeave(element.Position, element.Size);
        }

        /// <summary>
        /// Checks if the left mouse button was clicked within the specified area.
        /// </summary>
        internal static bool OnMouseLeftClick(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseClick(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button was clicked within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseLeftClick(UIElement element)
        {
            return OnMouseClick(MouseButton.Left, element);
        }

        /// <summary>
        /// Checks if the left mouse button is pressed within the specified area.
        /// </summary>
        internal static bool OnMouseLeftDown(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseDown(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is pressed within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseLeftDown(UIElement element)
        {
            return OnMouseDown(MouseButton.Left, element);
        }

        /// <summary>
        /// Checks if the left mouse button is released within the specified area.
        /// </summary>
        internal static bool OnMouseLeftUp(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseUp(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is released within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseLeftUp(UIElement element)
        {
            return OnMouseUp(MouseButton.Left, element);
        }

        /// <summary>
        /// Checks if the right mouse button was clicked within the specified area.
        /// </summary>
        internal static bool OnMouseRightClick(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseClick(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button was clicked within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseRightClick(UIElement element)
        {
            return OnMouseClick(MouseButton.Right, element);
        }

        /// <summary>
        /// Checks if the right mouse button is pressed within the specified area.
        /// </summary>
        internal static bool OnMouseRightDown(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseDown(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button is pressed within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseRightDown(UIElement element)
        {
            return OnMouseDown(MouseButton.Right, element);
        }

        /// <summary>
        /// Checks if the right mouse button is released within the specified area.
        /// </summary>
        internal static bool OnMouseRightUp(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseUp(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button is released within the area of the given <see cref="UIElement"/>.
        /// </summary>
        internal static bool OnMouseRightUp(UIElement element)
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
        private static bool IsMouseWithinArea(Vector2 mousePosition, Vector2 targetPosition, Vector2 areaSize)
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
        private static ButtonState GetButtonState(MouseButton button, MouseState state)
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
