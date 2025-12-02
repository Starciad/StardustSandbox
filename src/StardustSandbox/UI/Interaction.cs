using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Enums.InputSystem;
using StardustSandbox.Managers;

using System;

namespace StardustSandbox.UI
{
    internal static class Interaction
    {
        private static InputManager inputManager;

        private static bool isInitialized = false;

        internal static void Initialize(InputManager inputManager)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(Interaction)} system has already been initialized.");
            }

            Interaction.inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
            isInitialized = true;
        }

        #region MOUSE EVENTS

        /// <summary>
        /// Checks if the specified mouse button was clicked within the given area.
        /// </summary>
        internal static bool OnMouseClick(MouseButton button, Vector2 targetPosition, Vector2 areaSize)
        {
            EnsureInitialized();

            Vector2 mousePosition = inputManager.GetScaledMousePosition();

            return GetButtonState(button, inputManager.MouseState) == ButtonState.Released &&
                   GetButtonState(button, inputManager.PreviousMouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the specified mouse button is pressed within the given area.
        /// </summary>
        internal static bool OnMouseDown(MouseButton button, Vector2 targetPosition, Vector2 areaSize)
        {
            EnsureInitialized();

            Vector2 mousePosition = inputManager.GetScaledMousePosition();

            return GetButtonState(button, inputManager.MouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the specified mouse button is released within the given area.
        /// </summary>
        internal static bool OnMouseUp(MouseButton button, Vector2 targetPosition, Vector2 areaSize)
        {
            EnsureInitialized();

            Vector2 mousePosition = inputManager.GetScaledMousePosition();

            return GetButtonState(button, inputManager.MouseState) == ButtonState.Released &&
                   GetButtonState(button, inputManager.PreviousMouseState) == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor enters the specified area.
        /// </summary>
        internal static bool OnMouseEnter(Vector2 targetPosition, Vector2 areaSize)
        {
            EnsureInitialized();

            Vector2 mousePosition = inputManager.GetScaledMousePosition();
            Vector2 previousMousePosition = inputManager.GetScaledPreviousMousePosition();

            bool mouseWasOutside = !IsMouseWithinArea(previousMousePosition, targetPosition, areaSize);
            bool mouseIsInside = IsMouseWithinArea(mousePosition, targetPosition, areaSize);

            return mouseWasOutside && mouseIsInside;
        }

        /// <summary>
        /// Checks if the mouse cursor is over the specified area.
        /// </summary>
        internal static bool OnMouseOver(Vector2 targetPosition, Vector2 areaSize)
        {
            EnsureInitialized();

            Vector2 mousePosition = inputManager.GetScaledMousePosition();

            return IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor leaves the specified area.
        /// </summary>
        internal static bool OnMouseLeave(Vector2 targetPosition, Vector2 areaSize)
        {
            EnsureInitialized();

            Vector2 mousePosition = inputManager.GetScaledMousePosition();
            Vector2 previousMousePosition = inputManager.GetScaledPreviousMousePosition();

            bool mouseWasInside = IsMouseWithinArea(previousMousePosition, targetPosition, areaSize);
            bool mouseIsOutside = !IsMouseWithinArea(mousePosition, targetPosition, areaSize);

            return mouseWasInside && mouseIsOutside;
        }

        /// <summary>
        /// Checks if the left mouse button was clicked within the specified area.
        /// </summary>
        internal static bool OnMouseLeftClick(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseClick(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is pressed within the specified area.
        /// </summary>
        internal static bool OnMouseLeftDown(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseDown(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is released within the specified area.
        /// </summary>
        internal static bool OnMouseLeftUp(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseUp(MouseButton.Left, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor enters the specified area (left button context).
        /// </summary>
        internal static bool OnMouseLeftEnter(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseEnter(targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor is over the specified area (left button context).
        /// </summary>
        internal static bool OnMouseLeftOver(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseOver(targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor leaves the specified area (left button context).
        /// </summary>
        internal static bool OnMouseLeftLeave(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseLeave(targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button was clicked within the specified area.
        /// </summary>
        internal static bool OnMouseRightClick(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseClick(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button is pressed within the specified area.
        /// </summary>
        internal static bool OnMouseRightDown(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseDown(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the right mouse button is released within the specified area.
        /// </summary>
        internal static bool OnMouseRightUp(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseUp(MouseButton.Right, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor enters the specified area (right button context).
        /// </summary>
        internal static bool OnMouseRightEnter(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseEnter(targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor is over the specified area (right button context).
        /// </summary>
        internal static bool OnMouseRightOver(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseOver(targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor leaves the specified area (right button context).
        /// </summary>
        internal static bool OnMouseRightLeave(Vector2 targetPosition, Vector2 areaSize)
        {
            return OnMouseLeave(targetPosition, areaSize);
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
        /// Ensures the interaction system is initialized.
        /// </summary>
        private static void EnsureInitialized()
        {
            if (!isInitialized || inputManager is null)
            {
                throw new InvalidOperationException($"{nameof(Interaction)} system is not initialized.");
            }
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

        #endregion
    }
}
