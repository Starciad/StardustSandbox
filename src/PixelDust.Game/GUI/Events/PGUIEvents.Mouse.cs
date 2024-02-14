using PixelDust.Game.GUI.Elements;
using PixelDust.Game.Mathematics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace PixelDust.Game.GUI.Events
{
    public sealed partial class PGUIEvents
    {
        public void OnClick()
        {

        }

        public bool OnMouseDown(PGUIElement element, Size2 areaSize)
        {
            Vector2 targetPosition = element.Position;
            Vector2 mousePosition = this._inputManager.MouseState.Position.ToVector2();

            if (this._inputManager.MouseState.LeftButton == ButtonState.Pressed &&
                IsMouseWithinArea(mousePosition, targetPosition, areaSize))
            {
                return true;
            }

            return false;
        }

        public void OnDoubleClick()
        {

        }

        public void OnMouseDown()
        {

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
