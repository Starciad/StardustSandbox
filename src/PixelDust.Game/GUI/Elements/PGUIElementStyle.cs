using PixelDust.Game.Mathematics;
using PixelDust.Game.Enums.General;
using Microsoft.Xna.Framework;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.Constants;

namespace PixelDust.Game.GUI.Elements
{
    public sealed class PGUIElementStyle(PGUIElement element)
    {
        public PPositioningType PositioningType { get; set; } = PPositioningType.Relative;
        public Size2 Size { get; set; } = Size2.Zero;
        public PCardinalDirection PositionAnchor { get; set; } = PCardinalDirection.Northwest;
        public Color Color { get; set; } = Color.White;

        private readonly PGUIElement _element = element;

        public Vector2 GetPosition()
        {
            Size2 targetSize = new(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT);
            Vector2 targetPosition = Vector2.Zero;
            Vector2 elementPosition = this._element.Position;

            if (this.PositioningType == PPositioningType.Relative && this._element.Parent != null)
            {
                targetSize = this._element.Parent.Style.Size;
                targetPosition = this._element.Parent.Style.GetPosition();
            }

            return this.PositionAnchor switch
            {
                PCardinalDirection.Center    => targetPosition + elementPosition + new Vector2(targetSize.Width, targetSize.Height) / 2,
                PCardinalDirection.North     => targetPosition + elementPosition + new Vector2(targetSize.Width / 2, 0),
                PCardinalDirection.Northeast => targetPosition + elementPosition + new Vector2(targetSize.Width, 0),
                PCardinalDirection.East      => targetPosition + elementPosition + new Vector2(targetSize.Width, targetSize.Height / 2),
                PCardinalDirection.Southeast => targetPosition + elementPosition + new Vector2(targetSize.Width, targetSize.Height),
                PCardinalDirection.South     => targetPosition + elementPosition + new Vector2(targetSize.Width / 2, targetSize.Height),
                PCardinalDirection.Southwest => targetPosition + elementPosition + new Vector2(0, targetSize.Height),
                PCardinalDirection.West      => targetPosition + elementPosition + new Vector2(0, targetSize.Height / 2),
                PCardinalDirection.Northwest => targetPosition + elementPosition,
                _ => elementPosition,
            };
        }
    }
}
