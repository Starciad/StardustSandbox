using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Game.Mathematics;

using System;

namespace PixelDust.Game.GUI.Events
{
    public sealed partial class PGUIEvents
    {
        public bool OnMouseClick(Vector2 targetPosition, Size2 areaSize)
        {
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();

            return this._inputManager.MouseState.LeftButton == ButtonState.Released &&
                   this._inputManager.PreviousMouseState.LeftButton == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        public bool OnMouseDown(Vector2 targetPosition, Size2 areaSize)
        {
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();

            return this._inputManager.MouseState.LeftButton == ButtonState.Pressed &&
                   IsMouseWithinArea(mousePosition, targetPosition, areaSize);
        }

        public void OnMouseEnter()
        {

        }

        public void OnMouseLeave()
        {

        }

        public void OnMouseMove()
        {

        }

        public void OnMouseOut()
        {

        }

        public void OnMouseOver()
        {

        }

        public void OnMouseUp()
        {

        }

        private static bool IsMouseWithinArea(Vector2 mousePosition, Vector2 targetPosition, Size2 areaSize)
        {
            bool withinHorizontalBounds = MathF.Abs(mousePosition.X - targetPosition.X) < areaSize.Width;
            bool withinVerticalBounds = MathF.Abs(mousePosition.Y - targetPosition.Y) < areaSize.Height;

            return withinHorizontalBounds && withinVerticalBounds;
        }
    }
}
