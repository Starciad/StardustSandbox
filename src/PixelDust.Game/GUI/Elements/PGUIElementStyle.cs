using Microsoft.Xna.Framework;

using PixelDust.Game.Constants;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.GUI.Elements
{
    public sealed class PGUIElementStyle(PGUIElement element)
    {
        public PPositioningType PositioningType { get; set; } = PPositioningType.Relative;
        public PCardinalDirection PositionAnchor { get; set; } = PCardinalDirection.Northwest;
        public Size2 Size { get; set; } = Size2.Zero;
        public Color Color { get; set; } = Color.White;
        public Vector2 Margin { get; set; } = Vector2.Zero;

        private readonly PGUIElement _element = element;

        public Vector2 GetPosition()
        {
            Size2 screenSize = new(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT);

            Size2 targetSize = screenSize;
            Vector2 targetPosition = Vector2.Zero;

            if (this.PositioningType == PPositioningType.Relative && this._element.Parent != null)
            {
                targetSize = this._element.Parent.Style.Size;
                targetPosition = this._element.Parent.Position;
            }

            return this.PositionAnchor switch
            {
                PCardinalDirection.Center => targetPosition + this.Margin + (new Vector2(targetSize.Width, targetSize.Height) / 2),
                PCardinalDirection.North => targetPosition + this.Margin + new Vector2(targetSize.Width / 2, 0),
                PCardinalDirection.Northeast => targetPosition + this.Margin + new Vector2(targetSize.Width, 0),
                PCardinalDirection.East => targetPosition + this.Margin + new Vector2(targetSize.Width, targetSize.Height / 2),
                PCardinalDirection.Southeast => targetPosition + this.Margin + new Vector2(targetSize.Width, targetSize.Height),
                PCardinalDirection.South => targetPosition + this.Margin + new Vector2(targetSize.Width / 2, targetSize.Height),
                PCardinalDirection.Southwest => targetPosition + this.Margin + new Vector2(0, targetSize.Height),
                PCardinalDirection.West => targetPosition + this.Margin + new Vector2(0, targetSize.Height / 2),
                PCardinalDirection.Northwest => targetPosition + this.Margin,
                _ => targetPosition,
            };
        }
    }
}
