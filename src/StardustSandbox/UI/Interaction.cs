using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

            Interaction.inputManager = inputManager;
            isInitialized = true;
        }

        #region MOUSE EVENTS
        /// <summary>
        /// Checks if the left mouse button was clicked within the specified area.
        /// </summary>
        internal static bool OnMouseClick(Vector2 targetPosition, Vector2 areaSize)
        {
            Vector2 mousePosition = inputManager.GetScaledMousePosition();

            return inputManager.MouseState.LeftButton == ButtonState.Released &&
                   inputManager.PreviousMouseState.LeftButton == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is pressed within the specified area.
        /// </summary>
        internal static bool OnMouseDown(Vector2 targetPosition, Vector2 areaSize)
        {
            Vector2 mousePosition = inputManager.GetScaledMousePosition();

            return inputManager.MouseState.LeftButton == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is released within the specified area.
        /// </summary>
        internal static bool OnMouseUp(Vector2 targetPosition, Vector2 areaSize)
        {
            Vector2 mousePosition = inputManager.GetScaledMousePosition();

            return inputManager.MouseState.LeftButton == ButtonState.Released &&
                   inputManager.PreviousMouseState.LeftButton == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor enters the specified area.
        /// </summary>
        internal static bool OnMouseEnter(Vector2 targetPosition, Vector2 areaSize)
        {
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
            Vector2 mousePosition = inputManager.GetScaledMousePosition();

            return IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor leaves the specified area.
        /// </summary>
        internal static bool OnMouseLeave(Vector2 targetPosition, Vector2 areaSize)
        {
            Vector2 mousePosition = inputManager.GetScaledMousePosition();
            Vector2 previousMousePosition = inputManager.GetScaledPreviousMousePosition();

            bool mouseWasInside = IsMouseWithinArea(previousMousePosition, targetPosition, areaSize);
            bool mouseIsOutside = !IsMouseWithinArea(mousePosition, targetPosition, areaSize);

            return mouseWasInside && mouseIsOutside;
        }

        private static bool IsMouseWithinArea(Vector2 mousePosition, Vector2 targetPosition, Vector2 areaSize)
        {
            bool withinHorizontalBounds = MathF.Abs(mousePosition.X - targetPosition.X) < areaSize.X;
            bool withinVerticalBounds = MathF.Abs(mousePosition.Y - targetPosition.Y) < areaSize.Y;

            return withinHorizontalBounds && withinVerticalBounds;
        }
        #endregion
    }
}
