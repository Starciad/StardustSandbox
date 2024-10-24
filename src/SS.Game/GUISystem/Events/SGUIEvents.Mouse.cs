﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Game.Mathematics.Primitives;

using System;

namespace StardustSandbox.Game.GUI.Events
{
    public sealed partial class SGUIEvents
    {
        /// <summary>
        /// Checks if the left mouse button was clicked within the specified area.
        /// </summary>
        public bool OnMouseClick(Vector2 targetPosition, SSize2 areaSize)
        {
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();

            return this._inputManager.MouseState.LeftButton == ButtonState.Released &&
                   this._inputManager.PreviousMouseState.LeftButton == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is pressed within the specified area.
        /// </summary>
        public bool OnMouseDown(Vector2 targetPosition, SSize2 areaSize)
        {
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();

            return this._inputManager.MouseState.LeftButton == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the left mouse button is released within the specified area.
        /// </summary>
        public bool OnMouseUp(Vector2 targetPosition, SSize2 areaSize)
        {
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();

            return this._inputManager.MouseState.LeftButton == ButtonState.Released &&
                   this._inputManager.PreviousMouseState.LeftButton == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor enters the specified area.
        /// </summary>
        public bool OnMouseEnter(Vector2 targetPosition, SSize2 areaSize)
        {
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();
            Vector2 previousMousePosition = this._inputManager.PreviousMouseState.Position.ToVector2();

            bool mouseWasOutside = !IsMouseWithinArea(previousMousePosition, targetPosition, areaSize);
            bool mouseIsInside = IsMouseWithinArea(mousePosition, targetPosition, areaSize);

            return mouseWasOutside && mouseIsInside;
        }

        /// <summary>
        /// Checks if the mouse cursor is over the specified area.
        /// </summary>
        public bool OnMouseOver(Vector2 targetPosition, SSize2 areaSize)
        {
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();

            return IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        /// <summary>
        /// Checks if the mouse cursor leaves the specified area.
        /// </summary>
        public bool OnMouseLeave(Vector2 targetPosition, SSize2 areaSize)
        {
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();
            Vector2 previousMousePosition = this._inputManager.PreviousMouseState.Position.ToVector2();

            bool mouseWasInside = IsMouseWithinArea(previousMousePosition, targetPosition, areaSize);
            bool mouseIsOutside = !IsMouseWithinArea(mousePosition, targetPosition, areaSize);

            return mouseWasInside && mouseIsOutside;
        }

        private static bool IsMouseWithinArea(Vector2 mousePosition, Vector2 targetPosition, SSize2 areaSize)
        {
            bool withinHorizontalBounds = MathF.Abs(mousePosition.X - targetPosition.X) < areaSize.Width;
            bool withinVerticalBounds = MathF.Abs(mousePosition.Y - targetPosition.Y) < areaSize.Height;

            return withinHorizontalBounds && withinVerticalBounds;
        }
    }
}
